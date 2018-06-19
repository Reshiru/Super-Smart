using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Host.Helper;
using System;
using System.Web.Mvc;

namespace SuperSmart.Host.Controllers
{
    [BasicAuthorize]
    public class TaskController : Controller
    {
        private readonly ITaskPersistence taskPersistence = new TaskPersistence();

        [HttpGet]
        public ActionResult Create(long subjectId)
        {
            return View("CreateTask", new CreateTaskViewModel()
            {
                SubjectId = subjectId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTaskViewModel createTaskViewModel)
        {
            try
            {
                taskPersistence.Create(createTaskViewModel, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("CreateTask");
        }


        [HttpGet]
        public ActionResult Manage(int taskId)
        {
            try
            {
                if (!taskPersistence.HasAccountRightsForTask(taskId, this.User.Identity.Name))
                    throw new PropertyExceptionCollection(nameof(taskId), "User has no rights to manage task");
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("ManageTask", new ManageTaskViewModel()
            {
                TaskId = taskId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageTaskViewModel manageTaskViewModel)
        {
            try
            {
                taskPersistence.Manage(manageTaskViewModel, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("ManageTask");
        }

        [HttpGet]
        public ActionResult Overview(int subjectId)
        {
            OverviewTaskViewModel vm = taskPersistence.GetOverview(User.Identity.Name, subjectId);
            return View("OverviewTask", vm);
        }

    }
}