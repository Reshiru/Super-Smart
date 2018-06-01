using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class UserPersistence : IUserPersistence
    {
        public UserViewModel GetUserByLoginToken(string loginToken)
        {

            if (string.IsNullOrEmpty(loginToken))
            {
                throw new PropertyExceptionCollection(nameof(loginToken), "Parameter cannot be null or empty");
            }

            using (var db = new SuperSmartDb())
            {
                var user = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);
                if (user == null)
                {
                    throw new PropertyExceptionCollection(nameof(user), "User not found");
                }

                return new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName
                };
            }
        }
    }
}
