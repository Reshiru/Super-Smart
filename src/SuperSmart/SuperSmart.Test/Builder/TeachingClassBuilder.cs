using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using System;
using System.Collections.Generic;

namespace SuperSmart.Test.Builder
{
    public class TeachingClassBuilder
    {
        private string referral;
        private string designation;
        private int numberOfEducationYears;
        private DateTime started;
        private Account admin;
        private bool active;
        private List<Account> assignedAccounts;

        public TeachingClassBuilder()
        {
            var guid = Guid.NewGuid().ToString();

            this.referral = guid;
            this.designation = guid;
            this.numberOfEducationYears = 4;
            this.started = DateTime.Now;
            this.admin = null;
            this.active = true;
            this.assignedAccounts = new List<Account>();
        }

        public TeachingClassBuilder WithReferral(string referral)
        {
            this.referral = referral;

            return this;
        }

        public TeachingClassBuilder WithDesignation(string designation)
        {
            this.designation = designation;

            return this;
        }

        public TeachingClassBuilder WithNumberOfEducationYears(int numberOfEducationYears)
        {
            this.numberOfEducationYears = numberOfEducationYears;

            return this;
        }

        public TeachingClassBuilder WithStarted(DateTime started)
        {
            this.started = started;

            return this;
        }

        public TeachingClassBuilder WithAdmin(Account admin)
        {
            this.admin = admin;

            return this;
        }

        public TeachingClassBuilder WithActive(bool active)
        {
            this.active = active;

            return this;
        }

        public TeachingClassBuilder WithAssignedAccount(Account account)
        {
            this.assignedAccounts.Add(account);

            return this;
        }

        public TeachingClass Build()
        {
            using (var db = new SuperSmartDb())
            {
                var teachingClass = new TeachingClass()
                {
                    AssignedAccounts = this.assignedAccounts,
                    Admin = this.admin,
                    Active = this.active,
                    Designation = this.designation,
                    NumberOfEducationYears = this.numberOfEducationYears,
                    Referral = this.referral,
                    Started = this.started
                };

                db.TeachingClasses.Add(teachingClass);
                db.SaveChanges();

                return teachingClass;
            }
        }
    }
}