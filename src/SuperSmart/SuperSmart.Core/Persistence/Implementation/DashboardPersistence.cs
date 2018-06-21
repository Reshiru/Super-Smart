using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SuperSmart.Core.Data.Connection;
using SuperSmart.Core.Data.Implementation;
using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Helper;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperSmart.Core.Persistence.Implementation
{
    /// <summary>
    /// The dashboard persistence to manage 
    /// the dashboard data
    /// </summary>
    public class DashboardPersistence : IDashboardPersistence
    {
        private ITaskPersistence taskPersistence = new TaskPersistence();

        /// <summary>
        /// Get DashboardData for given User
        /// </summary>
        /// <param name="loginToken"></param>
        public DashboardViewModel GetDashboardData(string loginToken)
        {
            Guard.NotNullOrEmpty(loginToken);

            using (SuperSmartDb db = new SuperSmartDb())
            {
                var account = db.Accounts.SingleOrDefault(a => a.LoginToken == loginToken);

                if (account == null)
                {
                    throw new PropertyExceptionCollection(nameof(loginToken), "Account not found");
                }
                
                var appointments = db.Appointments.Include(a => a.Subject)
                                                  .ThenInclude(s => s.TeachingClass)
                                                  .ThenInclude(t => t.AssignedAccounts)
                                                  .Where(appointment => appointment.Subject.TeachingClass.AssignedAccounts
                                                        .Any(itm => itm.LoginToken == loginToken));

                var tasks = db.Tasks.Include(t => t.Subject)
                                    .ThenInclude(s => s.TeachingClass)
                                    .ThenInclude(t => t.AssignedAccounts)
                                    .Where(task => task.Subject.TeachingClass.AssignedAccounts
                                        .Any(itm => itm.LoginToken == loginToken) && 
                                            task.Finished > DateTime.Now && task.Finished < DateTime.Now.AddDays(7));


                var convertedAppointments = GetDashboardAppointmentMapper().Map<List<DashboardAppointmentViewModel>>(appointments);
                var convertedTasks = GetDashboardTaskMapper(account.Id).Map<List<DashboardTaskViewModel>>(tasks);

                var dashboardViewModel = new DashboardViewModel()
                {
                    Appointments = convertedAppointments,
                    Tasks = convertedTasks
                };

                return dashboardViewModel;
            }
        }

        /// <summary>
        /// Gets the mapper to map an appointment to a dashboard 
        /// appointment view model
        /// </summary>
        /// <returns></returns>
        private IMapper GetDashboardAppointmentMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Appointment, DashboardAppointmentViewModel>()
                 .ForMember(vm => vm.SubjectId, map => map.MapFrom(m => m.Subject.Id));
            }).CreateMapper();

            return mapper;
        }

        /// <summary>
        /// Gets the mapper to map an task to a dashboard 
        /// task view model
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private IMapper GetDashboardTaskMapper(long accountId)
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Task, DashboardTaskViewModel>()
                 .ForMember(vm => vm.SubjectName, map => map.MapFrom(m => m.Subject.Designation))
                 .ForMember(vm => vm.TaskId, map => map.MapFrom(m => m.Id))
                 .ForMember(vm => vm.Status, map => map.MapFrom(task => taskPersistence.GetTaskStatus(task.Id, accountId)));
            }).CreateMapper();

            return mapper;
        }
    }
}