using CRM.Models.Misc;
using System.Security.Principal;

namespace CRM.Extentions
{
    public static class PrincipalExtention
    {
        public static UserCreads GetCurrentUserCreads(this IPrincipal principal)
        {
            if (principal?.Identity != null)
            {
                var splitResult = principal.Identity.Name.Split('|');

                return new UserCreads(splitResult[0], splitResult[1]);
            }

            return null;
        }
    }
}