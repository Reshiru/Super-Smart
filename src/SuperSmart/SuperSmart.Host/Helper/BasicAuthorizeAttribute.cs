using SuperSmart.Core.Data.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SuperSmart.Host.Helper
{

    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
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