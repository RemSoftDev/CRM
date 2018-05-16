using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CRM.Extentions;
using System.Threading;
using System.Threading.Tasks;
using CRM.Services.Interfaces;

namespace CRM.Hubs
{
    public class PhoneHub : Hub
    {
        private readonly IUserConnectionStorage _userConnectionStorage;

        public PhoneHub(IUserConnectionStorage userConnectionStorage)
        {
            this._userConnectionStorage = userConnectionStorage.ValidateNotDefault(nameof(userConnectionStorage));
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
                    Clients.Clients(connId)
                        .onReciveCall();

                    //$"{Context.Request.Url.GetLeftPart(UriPartial.Authority)}/lead/edit/1"
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
    }
}