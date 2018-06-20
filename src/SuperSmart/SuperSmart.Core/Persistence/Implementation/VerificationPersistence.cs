using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    /// <summary>
    /// The verification peristence layer interface
    /// </summary>
    public class VerificationPersistence : IVerificationPersistence
    {
        /// <summary>
        /// Generates new login token for user if model is valid
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns>The login token</returns>
        public string Login(LoginViewModel loginViewModel)
        {
            Guard.ModelStateCheck(loginViewModel);
            
            using (var db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.Email == loginViewModel.Email.ToLower());
                
                if (account == null)
                {
                    throw new PropertyExceptionCollection(new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>(nameof(loginViewModel.Email), "A user with this combination doesn't exist"),
                        new Tuple<string, string>(nameof(loginViewModel.Password), "A user with this combination doesn't exist"),
                    });
                }

                var tmpPass = loginViewModel.Password.GenerateHash(account.Salt);

                if (tmpPass != account.Password)
                {
                    throw new PropertyExceptionCollection(new List<Tuple<string, string>>()
                    {
                        new Tuple<string, string>(nameof(loginViewModel.Email), "A user with this combination doesn't exist"),
                        new Tuple<string, string>(nameof(loginViewModel.Password), "A user with this combination doesn't exist"),
                    });
                }

                if((DateTime.Now - account.LastLogin).TotalHours > 8)
                {
                    account.LoginToken = 256.RandomString();
                }

                account.LastLogin = DateTime.Now;

                db.SaveChanges();

                return account.LoginToken;
            }
        }

        /// <summary>
        /// Registers a new user with the given data if model is valid
        /// </summary>
        /// <param name="registerViewModel"></param>
        public void Register(RegisterViewModel registerViewModel)
        {
            Guard.ModelStateCheck(registerViewModel);

            using (var db = new SuperSmartDb())
            {
                if (db.Accounts.SingleOrDefault(a => a.Email == registerViewModel.Email.ToLower()) != null)
                {
                    throw new PropertyExceptionCollection(nameof(registerViewModel.Email), "This email address is already in use");
                }

                var salt = 512.RandomString();

                db.Accounts.Add(new Account()
                {
                    Created = DateTime.Now,
                    Email = registerViewModel.Email.ToLower(),
                    FirstName = registerViewModel.FirstName,
                    LastName = registerViewModel.LastName,
                    Salt = salt,
                    Password = registerViewModel.Password.GenerateHash(salt),
                });

                db.SaveChanges();
            }
        }
    }
}