using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using System;

namespace SuperSmart.Test.Builder
{
    public class SubjectBuilder
    {
        private string designation;
        private bool active;
        private TeachingClass teachingClass;

        public SubjectBuilder()
        {
            var guid = Guid.NewGuid().ToString();

            designation = guid;
            active = true;
            teachingClass = null;
        }

        public SubjectBuilder WithDesignation(string designation)
        {
            this.designation = designation;

            return this;
        }

        public SubjectBuilder WithActive(bool active)
        {
            this.active = active;

            return this;
        }

        public SubjectBuilder WithTeachingClass(TeachingClass teachingClass)
        {
            this.teachingClass = teachingClass;

            return this;
        }

        public Subject Build()
        {
            using (var db = new SuperSmartDb())
            {
                var subject = new Subject()
                {
                    Active = this.active,
                    Designation = this.designation,
                    TeachingClass = this.teachingClass,
                };

                db.Subjects.Add(subject);
                db.SaveChanges();

                return subject;
            }
        }
    }
}
