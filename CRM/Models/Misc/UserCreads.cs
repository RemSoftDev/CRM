//using Newtonsoft.Json;

namespace CRM.Models.Misc
{
    public sealed class UserCreads
    {
        public UserCreads(int id, string name, string email)
        {
            Id = id;
            UserName = name;
            Email = email;
        }

        public int Id { get; private set; }

        public string UserName { get; private set; }

        public string Email { get; private set; }
    }

    //public sealed class AutenticateUser
    //{
        //[JsonConstructor]
        //public AutenticateUser(int Id, string FirstName, string Email, int Role)
        //{

        //}
        //public int Id { get; set; }

        //public string FirstName { get; set; }

        //public string Email { get; set; }

        //public int Role { get; set; }
    //}
}