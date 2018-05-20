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

                if(splitResult != null && splitResult.Length >= 3 && int.TryParse(splitResult[0], out int value))
                {
                    return new UserCreads(value, splitResult[1], splitResult[2]);
                }

                //var identity = ((ClaimsIdentity)principal.Identity);
                //if(identity.HasClaim(e => e.Type == ClaimTypes.Name) && identity.HasClaim(e => e.Type == ClaimTypes.Email))
                //{
                //    return new UserCreads(identity.FindFirst(ClaimTypes.Name).Value, identity.FindFirst(ClaimTypes.Email).Value);
                //}
            }

            return null;
        }

        //public static int? GetCurrentUserId(this IPrincipal principal)
        //{
        //    if (principal?.Identity != null)
        //    {
        //        var identity = ((ClaimsIdentity)principal.Identity);

        //        if (identity.HasClaim(e => e.Type == ClaimTypes.NameIdentifier) && 
        //            int.TryParse(identity.FindFirst(ClaimTypes.NameIdentifier).Value, out int value))
        //        {
        //            return value;
        //        }
        //    }

        //    return null;
        //}
    }
}