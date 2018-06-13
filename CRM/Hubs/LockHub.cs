using CRM.DAL.Entities;
using CRM.DAL.Repository;
using CRM.Extentions;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace CRM.Hubs
{
    public class LockInfo
    {
        public int Id { get; set; }

        public string LockName { get; set; }
    }

    public interface ILockHub
    {
        void LockEdit(LockInfo id);

        void UnLockEdit(int id);
    }

    public class LockHub : Hub
    {
        private const string LEAD_GROUP_NAME = "lead";
        private const string CUSTOMER_GROUP_NAME = "customer";
        private const string GROUP_NAME_KEY = "action";

        private readonly IUnitOfWork _unitOfWork;

        public LockHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork.ValidateNotDefault(nameof(unitOfWork));
        }

        public void Lock(int id)
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(out string groupName))
            {
                var userCreads = Context.User.GetCurrentUserCreads();
                User user = _unitOfWork.UsersRepository.FindById(userCreads.Id);

                Clients.Group(groupName, Context.ConnectionId).LockEdit(new LockInfo()
                {
                    Id = id,
                    LockName = $"{user.FirstName} {user.LastName}"
                });
            }
        }

        public void UnLock(int id)
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(out string groupName))
            {
                 Clients.Group(groupName, Context.ConnectionId).UnLockEdit(id);
            }
        }

        // New connection 
        public override async Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated && GetGroupName(out string groupName))
            {
                await Groups.Add(Context.ConnectionId, groupName);

                await base.OnConnected();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private bool GetGroupName(out string groupName)
        {
            string action = Context.QueryString[GROUP_NAME_KEY]?.ToLower();

            if (action == LEAD_GROUP_NAME)
            {
                groupName = LEAD_GROUP_NAME;
                return true;
            }
            else if (action == CUSTOMER_GROUP_NAME)
            {
                groupName = CUSTOMER_GROUP_NAME;
                return true;
            }

            groupName = null;
            return false;
        }
    }
}