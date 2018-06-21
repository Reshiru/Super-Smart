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

        /// <summary>
        /// Get Account by loginToken
        /// </summary>
        /// <param name="loginToken"></param>
        public ManageAccountViewModel GetManagedAccount(string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var user = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (user == null)
                {
                    throw new PropertyExceptionCollection(nameof(user), "Account not found");
                }

                var manageAccountViewModel = new ManageAccountViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                };

                return manageAccountViewModel;
            }
        }

        /// <summary>
        /// Manage acccount
        /// </summary>
        /// <param name="manageAccountViewModel"></param>
        /// <param name="loginToken"></param>
        public void Manage(ManageAccountViewModel manageAccountViewModel, string loginToken)
        {
            Guard.ModelStateCheck(manageAccountViewModel);
            Guard.NotNullOrEmpty(loginToken);

            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "User not found");
                }

                if (account.Id != manageAccountViewModel.Id)
                {
                    throw new PropertyExceptionCollection(nameof(account), "No permissions granted");
                }

                var salt = 512.RandomString();

                account.FirstName = manageAccountViewModel.Firstname;
                account.LastName = manageAccountViewModel.Lastname;
                account.Email = manageAccountViewModel.Email;
                if (!string.IsNullOrWhiteSpace(manageAccountViewModel.Password))
                    account.Password = manageAccountViewModel.Password.GenerateHash(salt);

                db.SaveChanges();
            }
        }
    }
}