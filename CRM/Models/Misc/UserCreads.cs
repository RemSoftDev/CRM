namespace CRM.Models.Misc
{
    public sealed class UserCreads
    {
        public UserCreads(string name, string email)
        {
            UserName = name;
            Email = email;
        }

        public string UserName { get; private set; }

        public string Email { get; private set; }
    }
}