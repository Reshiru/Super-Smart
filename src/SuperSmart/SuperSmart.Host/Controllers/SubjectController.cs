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
    public class SubjectController : Controller
    {
        private readonly ISubjectPersistence subjectPersistence = new SubjectPersistence();

        [HttpGet]
        public ActionResult Create(Int64 classId)
        {
            var createSubjectViewModel = new CreateSubjectViewModel()
            {
                TeachingClassId = classId
            };

            return View("CreateSubject", createSubjectViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateSubjectViewModel createSubjectViewModel)
        {
            try
            {
                subjectPersistence.Create(createSubjectViewModel, User.Identity.Name);

                var classId = new { classId = createSubjectViewModel.TeachingClassId };

                return RedirectToAction("Overview", classId);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("CreateSubject");
        }


        [HttpGet]
        public ActionResult Manage(int subjectId)
        {
            try
            {
                if (!subjectPersistence.IsAccountClassAdminOfSubject(subjectId, User.Identity.Name))
                {
                    throw new PropertyExceptionCollection(nameof(subjectId), "User has no rights to manage subject");
                }
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("ManageSubject", new ManageSubjectViewModel()
            {
                Id = subjectId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageSubjectViewModel manageSubjectViewModel)
        {
            try
            {
                Int64 teachingClassId = subjectPersistence.Manage(manageSubjectViewModel, User.Identity.Name);
                return RedirectToAction("Overview", new { classId = teachingClassId });
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("ManageSubject");
        }

        [HttpGet]
        public ActionResult Overview(int classId)
        {
            OverviewSubjectViewModel overviewSubjectViewModel = null;

            try
            {
                overviewSubjectViewModel = subjectPersistence.GetOverview(User.Identity.Name, classId);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("OverviewSubject", overviewSubjectViewModel);
        }
    }
}