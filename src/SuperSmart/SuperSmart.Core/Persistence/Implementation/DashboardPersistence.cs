using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class DashboardPersistence : IDashboardPersistence
    {
        public DashboardViewModel GetDashboardData(int userId)
        {
            using (SuperSmartDb db = new SuperSmartDb())
            {
                var appointmentsQuery = from appointments in db.Appointments
                                        where appointments.Subject.TeachingClass.AssignedAccounts.Any(itm => itm.Id == userId)
                                        select appointments;

                var taskQuery = from task in db.Tasks
                                where task.Subject.TeachingClass.AssignedAccounts.Any(itm => itm.Id == userId) &&
                                task.Finished > DateTime.Now &&
                                task.Finished < DateTime.Now.AddDays(7)
                                select task;

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<Appointment, DashboardAppointmentViewModel>()
                     .ForMember(vm => vm.Classroom, map => map.MapFrom(m => m.Classroom))
                     .ForMember(vm => vm.From, map => map.MapFrom(m => m.From))
                     .ForMember(vm => vm.Until, map => map.MapFrom(m => m.Until))
                     .ForMember(vm => vm.SubjectId, map => map.MapFrom(m => m.Subject.Id))
                     .ForMember(vm => vm.Day, map => map.MapFrom(m => m.Day));

                    cfg.CreateMap<Task, DashboardTaskViewModel>()
                     .ForMember(vm => vm.Designation, map => map.MapFrom(m => m.Designation))
                     .ForMember(vm => vm.Finished, map => map.MapFrom(m => m.Finished))
                     .ForMember(vm => vm.SubjectName, map => map.MapFrom(m => m.Subject.Designation))
                     .ForMember(vm => vm.TaskId, map => map.MapFrom(m => m.Id));
                });

                return new DashboardViewModel()
                {
                    Appointments = Mapper.Map<List<DashboardAppointmentViewModel>>(appointmentsQuery),
                    Tasks = Mapper.Map<List<DashboardTaskViewModel>>(taskQuery)
                };
            }
        }
    }
}
