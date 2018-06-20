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
    public class HomeController : Controller
    {
        private readonly IDashboardPersistence dashboardPersistence = new DashboardPersistence();
        private readonly IUserPersistence userPersistence = new UserPersistence();
        
        public ActionResult Index()
        {
            try
            {
                var dashboardViewModel = dashboardPersistence.GetDashboardData(User.Identity.Name);

                return View(nameof(Index), dashboardViewModel);
            }
            catch (Exception ex)
            {
                ModelState.Merge(ex as PropertyExceptionCollection);

                return View(nameof(Index));
            }
        }

        [ChildActionOnly]
        public ActionResult GetFullNameFromAuthorizedUser()
        {
            try
            {
                var userNameViewModel = userPersistence.GetFullNameFromUser(User?.Identity?.Name);

                return PartialView(userNameViewModel);
            }
            catch
            {
                return null;
            }
        }
    }
}