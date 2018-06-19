using SuperSmart.Core.Data.Connection;
using System.Linq;
using System.Web;

namespace SuperSmart.Host.Authentication
{
    /// <summary>
    /// The DbAuthorizeAttribute to verify that the user is permitted to use the 
    /// called method or / and class
    /// </summary>
    public class DbAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        /// <summary>
        /// The authentication process which needs to be 
        /// completed
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!base.AuthorizeCore(httpContext))
            {
                return false;
            }

            var token = httpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            using (var db = new SuperSmartDb())
            {
                if (db.Accounts.SingleOrDefault(a => a.LoginToken == token) == null)
                {
                    return false;
                }
            }

            return true;
        }
    }
}