using CRM.Services.Interfaces;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CRM.Services
{
    public sealed class UserConnectionStorage : IUserConnectionStorage
    {
        /// <summary>
        /// Storage for logged users
        /// </summary>
        private static ConcurrentDictionary<int, ConcurrentBag<string>> autorzedUsers = new ConcurrentDictionary<int, ConcurrentBag<string>>();

        public void AddUser(int userId)
        {
            autorzedUsers.TryAdd(userId, new ConcurrentBag<string>());
        }

        public void AddConnection(int userId, string connectId)
        {
            if (autorzedUsers.TryGetValue(userId, out ConcurrentBag<string> value))
            {
                value.Add(connectId);
            }
            else
            {
                autorzedUsers.TryAdd(userId, new ConcurrentBag<string>()
                {
                    connectId
                });
            }
        }

        public void RemoveConnectId(int userId, string connctId)
        {
            if (autorzedUsers.TryGetValue(userId, out ConcurrentBag<string> value) && value.Count > 0)
            {
                value.TryTake(out connctId);
            }
        }

        public void RemoveUser(int userId)
        {
            autorzedUsers.TryRemove(userId, out var removedListConnectId);
        }

        public List<string> Find(int userId)
        {
            List<string> result = null;

            if (autorzedUsers.TryGetValue(userId, out ConcurrentBag<string> value))
            {
                result = value.ToList();
            };

            return result?.ToList() ?? new List<string>();
        }

        public List<int> GetCurrentUsers()
        {
            return autorzedUsers
                .Where(e => e.Value != null && e.Value.Any())
                .Select(e => e.Key)
                .ToList();
        }

        public List<int> GetCurrentUsersExceptUser(int userId)
        {
            return autorzedUsers
                .Where(e => e.Key != userId && e.Value != null && e.Value.Any())
                .Select(e => e.Key)
                .ToList();
        }
    }
}