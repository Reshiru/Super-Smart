using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperSmart.Host.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDashboardPersistence dashboardPersistence = new DashboardPersistence();


        // GET: Home
        public ActionResult Index()
        {
            DashboardViewModel vm = dashboardPersistence.GetDashboardData(User.Identity.Name);
            return View("Index", vm);
        }
    }
}