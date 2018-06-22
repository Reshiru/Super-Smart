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
    public class DocumentController : Controller
    {
        private readonly IDocumentPersistence documentPersistence = new DocumentPersistence();

        [HttpGet]
        public ActionResult Create(Int64 taskId)
        {
            return View("CreateDocument", new CreateDocumentViewModel()
            {
                TaskId = taskId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateDocumentViewModel createDocumentViewModel)
        {
            try
            {
                if (createDocumentViewModel.FileUpload != null && createDocumentViewModel.FileUpload.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    createDocumentViewModel.FileUpload.InputStream.CopyTo(target);
                    createDocumentViewModel.File = target.ToArray();
                    createDocumentViewModel.FileName = createDocumentViewModel.FileUpload.FileName;
                    createDocumentViewModel.ContentType = createDocumentViewModel.FileUpload.ContentType;
                }
                else
                {
                    throw new PropertyExceptionCollection(nameof(createDocumentViewModel.FileUpload), "File is required");
                }

                documentPersistence.Create(createDocumentViewModel, User.Identity.Name);

            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return RedirectToAction("Overview", new { taskId = createDocumentViewModel.TaskId });
        }

        [HttpGet]
        public ActionResult Delete(Int64 documentId, Int64 taskId)
        {
            try
            {
                documentPersistence.Delete(documentId, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return RedirectToAction("Overview", new { taskId = taskId });
        }

        [HttpGet]
        public ActionResult Overview(Int64 taskId)
        {
            OverviewDocumentViewModel vm = documentPersistence.GetOverview(taskId, this.User.Identity.Name);
            return View("OverviewDocument", vm);
        }

        [HttpGet]
        public ActionResult Manage(Int64 documentId)
        {
            ManageDocumentViewModel vm = new ManageDocumentViewModel();

            try
            {

                vm = documentPersistence.GetManagedDocument(documentId, this.User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("ManageDocument", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageDocumentViewModel manageDocumentViewModel)
        {
            try
            {
                if (manageDocumentViewModel.FileUpload != null && manageDocumentViewModel.FileUpload.ContentLength > 0)
                {
                    MemoryStream target = new MemoryStream();
                    manageDocumentViewModel.FileUpload.InputStream.CopyTo(target);
                    manageDocumentViewModel.File = target.ToArray();
                    manageDocumentViewModel.FileName = manageDocumentViewModel.FileUpload.FileName;
                    manageDocumentViewModel.ContentType = manageDocumentViewModel.FileUpload.ContentType;
                }

                documentPersistence.Manage(manageDocumentViewModel, User.Identity.Name);

            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return RedirectToAction("Overview", new { taskId = manageDocumentViewModel.TaskId });
        }


        [HttpGet]
        public FileResult Download(Int64 documentId)
        {
            try
            {
                DownloadDocumentViewModel document = documentPersistence.Download(documentId, this.User.Identity.Name);
                return File(document.File, document.ContentType, document.FileName);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return null;
        }
    }
}