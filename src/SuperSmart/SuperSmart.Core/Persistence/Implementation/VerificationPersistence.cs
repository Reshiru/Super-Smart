using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    public class VerificationPersistence : IVerificationPersistence
    {
        public string Login(LoginViewModel loginViewModel)
        {
            if (loginViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(loginViewModel), "Parameter cannot be null");
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(loginViewModel, new ValidationContext(loginViewModel, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }
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

        public void Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
            {
                throw new PropertyExceptionCollection(nameof(registerViewModel), "Parameter cannot be null");
            }
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(registerViewModel, new ValidationContext(registerViewModel, serviceProvider: null, items: null), validationResults, true))
            {
                throw new PropertyExceptionCollection(validationResults);
            }
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
