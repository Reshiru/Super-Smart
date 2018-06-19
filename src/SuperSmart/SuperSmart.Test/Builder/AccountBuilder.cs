using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Extension;
using System;

namespace SuperSmart.Test.Builder
{
    /// <summary>
    /// The account builder class to initialize 
    /// a new account instance on build
    /// </summary>
    public class AccountBuilder
    {
        private string email;
        private string firstname;
        private string lastname;
        private string password;
        private DateTime created;
        private string loginToken;
        private string salt;

        /// <summary>
        /// Initializes a new account builder class
        /// </summary>
        public AccountBuilder()
        {
            var guid = Guid.NewGuid().ToString();

            this.email = guid + "@" + "testaccount.test";
            this.firstname = guid;
            this.lastname = guid;
            this.password = guid;
            this.loginToken = guid;
            this.created = DateTime.Now;
        }

        public AccountBuilder WithEmail(string email)
        {
            this.email = email;

            return this;
        }

        public AccountBuilder WithFirstname(string firstname)
        {
            this.firstname = firstname;

            return this;
        }

        public AccountBuilder WithLastname(string lastname)
        {
            this.lastname = lastname;

            return this;
        }

        public AccountBuilder WithCreated(DateTime created)
        {
            this.created = created;

            return this;
        }

        public AccountBuilder WithLoginToken(string loginToken)
        {
            this.loginToken = loginToken;

            return this;
        }

        public AccountBuilder WithPassword(string password)
        {
            this.password = password;

            return this;
        }

        public AccountBuilder WithSalt(string salt)
        {
            this.salt = salt;

            return this;
        }

        public Account Build()
        {
            var account = new Account()
            {
                Created = this.created,
                Email = this.email,
                FirstName = this.firstname,
                LastName = this.lastname,
                LoginToken = this.loginToken,
                LastLogin = DateTime.Now,
                Password = this.password.GenerateHash(this.salt),
                Salt = this.salt,
            };

            return account;
        }
    }
}