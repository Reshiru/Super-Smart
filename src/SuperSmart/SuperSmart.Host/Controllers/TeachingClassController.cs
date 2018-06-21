﻿using SuperSmart.Core.Data.ViewModels;
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
    public class TeachingClassController : Controller
    {
        private readonly ITeachingClassPersistence teachingClassPersistence = new TeachingClassPersistence();

        [HttpGet]
        public ActionResult Create()
        {
            return View("CreateTeachingClass");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTeachingClassViewModel createTeachingClassViewModel)
        {
            try
            {
                teachingClassPersistence.Create(createTeachingClassViewModel, User.Identity.Name);
                return RedirectToAction("Overview");
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("CreateTeachingClass");
        }


        [HttpGet]
        public ActionResult Join(string referral)
        {
            try
            {
                teachingClassPersistence.Join(referral, User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("OverviewTeachingClass");
        }


        [HttpGet]
        public ActionResult Manage(int teachingClassId)
        {
            ManageTeachingClassViewModel vm = new ManageTeachingClassViewModel();

            try
            {
                vm = teachingClassPersistence.GetManagedTeachingClass(teachingClassId, this.User.Identity.Name);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }

            return View("ManageTeachingClass", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ManageTeachingClassViewModel manageTeachingClassViewModel)
        {
            try
            {
                teachingClassPersistence.Manage(manageTeachingClassViewModel, User.Identity.Name);
                return RedirectToAction("Overview");
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);
            }
            return View("ManageTeachingClass");
        }

        [HttpGet]
        public ActionResult Overview()
        {
            OverviewTeachingClassViewModel vm = teachingClassPersistence.GetOverview(User.Identity.Name);
            return View("OverviewTeachingClass", vm);
        }
        
    }
}