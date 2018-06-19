using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Test.Helper;
using System.Collections.Generic;

namespace SuperSmart.Test.Builder
{
    public class DatabaseBuilder
    {
        private bool secureDatabaseDeleted;
        private List<Account> accounts;
        private List<TeachingClass> teachingClasses;

        public DatabaseBuilder()
        {
            secureDatabaseDeleted = true;

            accounts = new List<Account>();
            teachingClasses = new List<TeachingClass>();
        }

        public DatabaseBuilder WithAccount(Account account)
        {
            this.accounts.Add(account);

            return this;
        }

        public DatabaseBuilder WithTeachingClass(TeachingClass teachingClasse)
        {
            this.teachingClasses.Add(teachingClasse);

            return this;
        }

        public DatabaseBuilder WithSecureDatabaseDeleted(bool secureDatabaseDeleted)
        {
            this.secureDatabaseDeleted = secureDatabaseDeleted;

            return this;
        }

        public void Build()
        {
            if (secureDatabaseDeleted)
            {
                DatabaseHelper.SecureDeleteDatabase();
            }

            using (var db = new SuperSmartDb())
            {
                foreach (var account in accounts)
                {
                    db.Accounts.Add(account);
                }

                foreach (var teachingClass in teachingClasses)
                {
                    db.TeachingClasses.Add(teachingClass);
                }

                db.SaveChanges();
            }
        }
    }
}
