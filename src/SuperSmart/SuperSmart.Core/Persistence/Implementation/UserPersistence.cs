using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    /// <summary>
    /// The user persistence to manage user data
    /// </summary>
    public class UserPersistence : IUserPersistence
    {
        /// <summary>
        /// Get user by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        public UserViewModel GetUserByLoginToken(string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var user = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (user == null)
                {
                    throw new PropertyExceptionCollection(nameof(user), "Account not found");
                }

                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName
                };

                return userViewModel;
            }
        }

        /// <summary>
        /// Get full name by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        public UserNameViewModel GetFullNameFromUser(string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var user = db.Accounts.SingleOrDefault(u => u.LoginToken == loginToken);

                if (user == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }

                var userNameViewModel = new UserNameViewModel()
                {
                    Firstname = user.FirstName,
                    Lastname = user.LastName
                };

                return userNameViewModel;
            }
        }
    }
}
