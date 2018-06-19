using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using System;

namespace SuperSmart.Test.Builder
{
    public class TaskBuilder
    {
        private string designation;
        private DateTime finished;
        private bool active;
        private Subject subject;
        private Account owner;

        public TaskBuilder()
        {
            var guid = Guid.NewGuid().ToString();

            this.designation = guid;
            this.finished = DateTime.Now.AddDays(2);
            this.active = true;
            this.subject = null;
            this.owner = null;
        }

        public TaskBuilder WithDesignation(string designation)
        {
            this.designation = designation;

            return this;
        }

        public TaskBuilder WithFinished(DateTime finished)
        {
            this.finished = finished;

            return this;
        }

        public TaskBuilder WithActive(bool active)
        {
            this.active = active;

            return this;
        }

        public TaskBuilder WithSubject(Subject subject)
        {
            this.subject = subject;

            return this;
        }

        public TaskBuilder WithOwner(Account owner)
        {
            this.owner = owner;

            return this;
        }

        public Task Build()
        {
            var task = new Task()
            {
                Active = this.active,
                Designation = this.designation,
                Subject = this.subject,
                Owner = this.owner,
                Finished = this.finished
            };

            return task;
        }
    }
}
