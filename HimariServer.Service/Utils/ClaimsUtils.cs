using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Utils
{
    public static class ClaimsUtils
    {
        public static string GetEmailFromIdentity(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
            }
            return null;
        }
        public static string GetIdFromIdentity(ClaimsIdentity identity)
        {
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return userClaims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            }
            return null;
        }
    }
}
