using SuperSmart.Core.Data.Implementation;
using System;

namespace SuperSmart.Test.Builder
{
    public class AppointmentBuilder
    {
        private DateTime from;
        private DateTime until;
        private string classroom;
        private DayOfWeek day;
        private Subject subject;

        public AppointmentBuilder()
        {
            this.from = DateTime.Now;
            this.until = DateTime.Now.AddMinutes(45);
            this.classroom = Guid.NewGuid().ToString();
            this.day = DateTime.Now.DayOfWeek;
        }

        public AppointmentBuilder WithFrom(DateTime from)
        {
            this.from = from;

            return this;
        }

        public AppointmentBuilder WithUntil(DateTime until)
        {
            this.until = DateTime.Now;

            return this;
        }

        public AppointmentBuilder WithClassroom(string classroom)
        {
            this.classroom = classroom;

            return this;
        }

        public AppointmentBuilder WithDay(DayOfWeek dayOfWeek)
        {
            this.day = dayOfWeek;

            return this;
        }

        public AppointmentBuilder WithSubject(Subject subject)
        {
            this.subject = subject;

            return this;
        }

        public Appointment Build()
        {
            var appointment = new Appointment()
            {
                Day = this.day,
                Classroom = this.classroom,
                From = this.from,
                Until = this.until,
                Subject = this.subject
            };

            return appointment;
        }
    }
}