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
        private List<Subject> subjects;
        private List<Appointment> appointments;
        private List<Task> tasks;

        public DatabaseBuilder()
        {
            secureDatabaseDeleted = true;

            accounts = new List<Account>();
            teachingClasses = new List<TeachingClass>();
            subjects = new List<Subject>();
            appointments = new List<Appointment>();
            tasks = new List<Task>();
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

        public DatabaseBuilder WithSubject(Subject subject)
        {
            this.subjects.Add(subject);

            return this;
        }

        public DatabaseBuilder WithAppointment(Appointment appointment)
        {
            this.appointments.Add(appointment);

            return this;
        }

        public DatabaseBuilder WithTask(Task task)
        {
            this.tasks.Add(task);

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
            using (var transaction = db.Database.BeginTransaction())
            {
                accounts.ForEach(a => db.Accounts.Add(a));
                teachingClasses.ForEach(t => db.TeachingClasses.Add(t));
                subjects.ForEach(s => db.Subjects.Add(s));
                appointments.ForEach(a => db.Appointments.Add(a));
                tasks.ForEach(a => db.Tasks.Add(a));

                db.SaveChanges();

                teachingClasses.ForEach(t =>
                {
                    var admin = t.Admin;

                    t.AssignedAccounts.Add(admin);
                    admin.AssignedClasses.Add(t);
                });

                db.SaveChanges();

                transaction.Commit();
            }
        }
    }
}