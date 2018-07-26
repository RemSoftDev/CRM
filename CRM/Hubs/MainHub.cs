using AutoMapper;
using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Enums;
using CRM.Extentions;
using CRM.Models;
using CRM.Models.Misc;
using CRM.Services;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.Hubs
{
    [HubName("phoneHub")]
    public class MainHub : Hub
    {
        private const string LEAD_GROUP_NAME = "lead";
        private const string CUSTOMER_GROUP_NAME = "customer";
        private const string GROUP_NAME_KEY = "action";
        private const string LOCK_BY_POP_UP = "popUpLock";

        private readonly IUserConnectionStorage _userConnectionStorage;
        private readonly IUnitOfWork _unitOfWork;
        private readonly PhoneService _phoneService;

        private static ConcurrentDictionary<string, List<LockInfo>> dataToLock = new ConcurrentDictionary<string, List<LockInfo>>();

        public MainHub(IUserConnectionStorage userConnectionStorage, PhoneService phoneService, IUnitOfWork unitOfWork)
        {
            _userConnectionStorage = userConnectionStorage.ValidateNotDefault(nameof(userConnectionStorage));
            _phoneService = phoneService.ValidateNotDefault(nameof(phoneService));
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        /// <summary>
        /// Add to "lock group"
        /// </summary>
        /// <param name="groupName"></param>
        public async Task AddToGroup(string groupName)
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(groupName, out string validateGroupName))
            {
                await Groups.Add(Context.ConnectionId, validateGroupName);

                dataToLock.TryGetValue(validateGroupName, out List<LockInfo> value);
                if (value != null || value.Count > 0)
                    Clients.Group(validateGroupName).LockList(value);
            }
        }

        /// <summary>
        /// Lock some lead/customer on edit/convert
        /// </summary>
        /// <param name="id">Id of lead/customer</param>
        /// <param name="groupName">lead or customer</param>
        public void Lock(int id, string groupName)
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(groupName, out string validateGroupName))
            {
                var userCreads = Context.User.GetCurrentUserCreads();
                User user = _unitOfWork.UsersRepository.FindById(userCreads.Id);

                var lockInfo = new LockInfo()
                {
                    Id = id,
                    LockName = $"{user.FirstName} {user.LastName}"
                };

                Clients.Group(validateGroupName, Context.ConnectionId).LockEdit(lockInfo);

                Clients.Others.onResetPopUp();

                dataToLock.AddOrUpdate(validateGroupName, new List<LockInfo> { lockInfo }, (key, value) =>
                {
                    value.Add(lockInfo);
                    return value;
                });
            }
        }

        /// <summary>
        /// UnLock some lead/customer on edit/convert
        /// </summary>
        /// <param name="id">Id of lead/customer</param>
        /// <param name="groupName">lead or customer</param>
        public void UnLock(int id, string groupName)
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(groupName, out string validateGroupName))
            {
                Clients.Group(validateGroupName, Context.ConnectionId).UnLockEdit(id);

                dataToLock.TryGetValue(validateGroupName, out List<LockInfo> value);
                value.RemoveAll(e => e.Id == id);
            }
        }

        /// <summary>
        /// Get not seved lead for pop-up
        /// </summary>
        public void GetUnsavedLead()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var userCreads = Context.User.GetCurrentUserCreads();

                if (_unitOfWork.UsersRepository.FindById(userCreads.Id).UserTypeId == (int)UserType.AdminTeamMember)
                {
                    List<Lead> leads = _unitOfWork.LeadsRepository.Get(e => !e.IsSaved).ToList();

                    if (leads != null && leads.Count > 0)
                    {
                        foreach (var lead in leads)
                        {
                            if (dataToLock.TryGetValue(LEAD_GROUP_NAME, out List<LockInfo> lockList) && lockList.Exists(e => e.Id == lead.Id))
                            {
                                if(leads.IndexOf(lead) == leads.Count - 1)
                                {
                                    // hide pop-up if all to show are locked
                                    Clients.Caller.onHidePopUp();
                                }
                                continue;
                            }
                            else
                            {
                                var mapResult = Mapper.Map<PopUpViewModel>(lead);
                                Clients.Caller.onPopUpCall(mapResult);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // hide pop-up if we have not leads to show
                        Clients.Caller.onHidePopUp();
                    }
                }
            }
        }

        /// <summary>
        /// Method to emulate call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phoneNumber"></param>
        public void Call(int id, string phoneNumber)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var connId = _userConnectionStorage.Find(id);

                if (connId == null)
                {
                    Clients.Caller.onCall("User closed browser.");
                }
                else
                {
                    string rightPart = _phoneService.GetRightPartRedirectUrl(phoneNumber);
                    string redirectUrl = BuildFullRedirectUrl(rightPart);

                    Clients.Clients(connId)
                        .onReciveCall(redirectUrl);
                }
            }
        }

        // New connection 
        public async override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                _userConnectionStorage.AddConnection(Context.User.GetCurrentUserCreads().Id, Context.ConnectionId);

                await base.OnConnected();
            }
        }

        // Disconnect
        public async override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                _userConnectionStorage.RemoveConnectId(Context.User.GetCurrentUserCreads().Id, Context.ConnectionId);

                await base.OnDisconnected(stopCalled);
            }
        }

        /// <summary>
        /// Build whole redirect url 
        /// </summary>
        /// <param name="rightParts">Right part of url</param>
        /// <returns>Whole url</returns>
        private string BuildFullRedirectUrl(string rightParts)
        {
            return $"{Context.Request.Url.GetLeftPart(UriPartial.Authority)}/{rightParts}";
        }

        /// <summary>
        /// Validate groupName
        /// </summary>
        /// <param name="recivedGroupName"></param>
        /// <param name="groupName"></param>
        /// <returns>true if correct, otherwise false</returns>
        private bool GetGroupName(string recivedGroupName, out string groupName)
        {
            groupName = null;

            if (string.IsNullOrEmpty(recivedGroupName))
                return false;

            if (recivedGroupName.ToLower() == LEAD_GROUP_NAME)
            {
                groupName = LEAD_GROUP_NAME;
                return true;
            }
            else if (recivedGroupName.ToLower() == CUSTOMER_GROUP_NAME)
            {
                groupName = CUSTOMER_GROUP_NAME;
                return true;
            }

            return false;
        }
    }
}