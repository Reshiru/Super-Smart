using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Host.Authentication;
using SuperSmart.Host.Helper;
using System;
using System.Web.Mvc;

namespace SuperSmart.Host.Controllers
{
    [DbAuthorize]
    public class TaskController : Controller
    {
        private readonly ITaskPersistence taskPersistence = new TaskPersistence();
        private readonly IDocumentPersistence documentPersistence = new DocumentPersistence();

        [HttpGet]
        public ActionResult Create(Int64 subjectId)
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

                return RedirectToAction("Overview", new { subjectId = createTaskViewModel.SubjectId });

            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("CreateTask");
        }


        [HttpGet]
        public ActionResult Manage(Int64 taskId)
        {
            ManageTaskViewModel vm = new ManageTaskViewModel();

            try
            {

                vm = taskPersistence.GetManagedTask(taskId, this.User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("ManageTask", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageTaskViewModel manageTaskViewModel)
        {
            try
            {
                taskPersistence.Manage(manageTaskViewModel, User.Identity.Name);
                return RedirectToAction("Overview", new { subjectId = manageTaskViewModel.SubjectId });
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("ManageTask");
        }

        [HttpGet]
        public ActionResult Overview(Int64 subjectId)
        {
            OverviewTaskViewModel vm = taskPersistence.GetOverview(User.Identity.Name, subjectId);
            return View("OverviewTask", vm);
        }

        [HttpGet]
        public ActionResult InvertStatus(Int64 taskId, Int64 subjectId, bool home = false)
        {
            try
            {
                taskPersistence.InvertTaskStatus(taskId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            if (home)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction(nameof(Overview), new { @subjectId = subjectId });
        }
    }
}