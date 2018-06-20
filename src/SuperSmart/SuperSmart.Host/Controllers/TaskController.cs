using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Extension;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Host.Authentication;
using SuperSmart.Host.Helper;
using System;
using System.IO;
using System.Web;
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
        public ActionResult Create(CreateTaskViewModel createTaskViewModel, HttpPostedFileBase file)
        {
            try
            {
                Int64 taskId = taskPersistence.Create(createTaskViewModel, User.Identity.Name);

                if (file != null && file.ContentLength > 0)
                {
                    try
                    {
                        MemoryStream target = new MemoryStream();
                        file.InputStream.CopyTo(target);
                        byte[] data = target.ToArray();

                        documentPersistence.Create(new CreateDocumentViewModel()
                        {
                            DocumentType = Core.Data.Enumeration.DocumentType.Document,
                            File = data,
                            FileName = file.FileName,
                            TaskId = taskId
                        }, this.User.Identity.Name);
                    }
                    catch (Exception ex)
                    {
                        throw new PropertyExceptionCollection("FileUpload", ex.Message);
                    }
                }

                return RedirectToAction("Overview", new { subjectId = createTaskViewModel.SubjectId });

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
                return RedirectToAction("Overview", new { subjectId = manageTaskViewModel.SubjectId });
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