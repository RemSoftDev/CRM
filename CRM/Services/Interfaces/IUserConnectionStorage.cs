using System.Collections.Generic;

namespace CRM.Services.Interfaces
{
    /// <summary>
    /// Store logged users and their connection (page-server)
    /// </summary>
    public interface IUserConnectionStorage
    {
        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="userId"></param>
        void AddUser(int userId);

        /// <summary>
        /// Add connectionId to logged user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connectId"></param>
        void AddConnection(int userId, string connectId);

        /// <summary>
        /// Remove connectionId from logged user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="connctId"></param>
        void RemoveConnectId(int userId, string connctId);

        /// <summary>
        /// Remove user from list of logged users
        /// </summary>
        /// <param name="userId"></param>
        void RemoveUser(int userId);

        /// <summary>
        /// Find all connectionIds for logged user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<string> Find(int userId);

        /// <summary>
        /// Get all logged users
        /// </summary>
        /// <returns></returns>
        List<int> GetCurrentUsers();

        /// <summary>
        /// Get all user except one
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<int> GetCurrentUsersExceptUser(int userId);
    }
}
