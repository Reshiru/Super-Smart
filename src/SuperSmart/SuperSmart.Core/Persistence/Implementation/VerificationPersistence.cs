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
        public Guid Login(LoginViewModel loginViewModel)
        {
            throw new NotImplementedException();
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
