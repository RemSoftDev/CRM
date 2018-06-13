using CRM.Extentions;
using CRM.Services;
using CRM.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using System;
using System.Threading.Tasks;

namespace CRM.Hubs
{
    public class PhoneHub : Hub
    {
        private readonly IUserConnectionStorage _userConnectionStorage;
        private readonly PhoneService _phoneService;

        public PhoneHub(IUserConnectionStorage userConnectionStorage, PhoneService phoneService)
        {
            _userConnectionStorage = userConnectionStorage.ValidateNotDefault(nameof(userConnectionStorage));
            _phoneService = phoneService.ValidateNotDefault(nameof(phoneService));
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
                var connectid = Context.ConnectionId;

                _userConnectionStorage.AddConnection(Context.User.GetCurrentUserCreads().Id, connectid);

                await base.OnConnected();
            }
        }


        // Disconnect
        public async override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var connectid = Context.ConnectionId;

                _userConnectionStorage.RemoveConnectId(Context.User.GetCurrentUserCreads().Id, connectid);

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
    }
}