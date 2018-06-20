using AutoMapper;
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

                var appointmentsQuery = from appointment in db.Appointments
                                        where appointment.Subject.TeachingClass.AssignedAccounts.Any(itm => itm.LoginToken == loginToken)
                                        select appointment;

                var taskQuery = from task in db.Tasks
                                where task.Subject.TeachingClass.AssignedAccounts.Any(itm => itm.LoginToken == loginToken) &&
                                task.Finished > DateTime.Now &&
                                task.Finished < DateTime.Now.AddDays(7)
                                select task;

                var config = new MapperConfiguration(cfg =>
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

                IMapper mapper = config.CreateMapper();

                List<DashboardAppointmentViewModel> appointments = mapper.Map<List<DashboardAppointmentViewModel>>(appointmentsQuery);
                List<DashboardTaskViewModel> tasks = mapper.Map<List<DashboardTaskViewModel>>(taskQuery);
                
                tasks.ForEach(itm =>
                {
                    itm.Status = taskPersistence.GetTaskStatus(itm.TaskId, account.Id);
                });

                var dashboardViewModel = new DashboardViewModel()
                {
                    Appointments = appointments,
                    Tasks = tasks
                };

                return dashboardViewModel;
            }
        }
    }
}
