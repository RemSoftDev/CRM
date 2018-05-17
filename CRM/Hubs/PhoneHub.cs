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

        public void Call(int id, string phoneNumber)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var connId = _userConnectionStorage.Find(id);

                if (connId == null)
                {
                    Clients.Caller.onCall("User close browser");
                }
                else
                {
                    string rightPart = _phoneService.GetRightPartRedirectUrlByPhoneNum(phoneNumber);
                    string redirectUrl = BuildFullRedirectUrl(rightPart);

                    Clients.Clients(connId)
                        .onReciveCall(redirectUrl);
                }
            }
        }

        // Подключение нового пользователя
        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var connectid = Context.ConnectionId;

                _userConnectionStorage.AddConnection(Context.User.GetCurrentUserCreads().Id, connectid);

                return base.OnConnected();
            }

            return Task.CompletedTask;
        }


        // Отключение пользователя
        public override Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var connectid = Context.ConnectionId;

                _userConnectionStorage.RemoveConnectId(Context.User.GetCurrentUserCreads().Id, connectid);

                return base.OnDisconnected(stopCalled);
            }

            return Task.CompletedTask;
        }

        private string BuildFullRedirectUrl(string rightParts)
        {
            return $"{Context.Request.Url.GetLeftPart(UriPartial.Authority)}/{rightParts}";
        }
    }
}