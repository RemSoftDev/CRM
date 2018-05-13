using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CRM.Extentions;

namespace CRM.Hubs
{
    public static class AutorizedUsers
    {
        private static ConcurrentDictionary<int, string> autorzedUsers = new ConcurrentDictionary<int, string>();

        public static void Add(int id)
        {
            autorzedUsers.TryAdd(id, null);
        }

        public static void Update(int id, string connctId)
        {
            if(autorzedUsers.TryGetValue(id, out string value))
            {
                autorzedUsers[id] = connctId;
            }
        }

        public static void Remove(int id)
        {
            autorzedUsers.TryRemove(id, out string removedConnectId);
        }

        public static string Find(int id)
        {
            if (autorzedUsers.TryGetValue(id, out string value))
            {
                return value;
            };

            return null;
        }

    }


    public class PhoneHub : Hub
    {
        public void Call(int id, string phoneNumber)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                string connId = AutorizedUsers.Find(id);

                if (connId == null)
                {
                    Clients.Caller.onCall("User close browser");
                }
                else
                {
                    Clients.Client(connId)
                        .onReciveCall(new List<OnlineUser>
                        {
                        new OnlineUser() { Name = "TEST", ConnectionId = "1"},
                        new OnlineUser() { Name = "TEST2", ConnectionId = "2"}
                        });

                    //$"{Context.Request.Url.GetLeftPart(UriPartial.Authority)}/lead/edit/1"
                }
            }
        }

        // Подключение нового пользователя
        public void Connect()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                var id = Context.ConnectionId;

                AutorizedUsers.Update(Context.User.GetCurrentUserCreads().Id, id);
            }
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AutorizedUsers.Update(Context.User.GetCurrentUserCreads().Id, null);

            }

            return base.OnDisconnected(stopCalled);
        }
    }

    public class OnlineUser
    {
        public string ConnectionId { get; set; }

        public string Name { get; set; }
    }
}