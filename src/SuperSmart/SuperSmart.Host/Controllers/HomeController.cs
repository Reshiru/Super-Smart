using SuperSmart.Core.Data.ViewModels;
using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using SuperSmart.Host.Authentication;
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
            DashboardViewModel vm = dashboardPersistence.GetDashboardData(User.Identity.Name);
            return View("Index", vm);
        }

        [ChildActionOnly]
        public ActionResult GetFullNameFromAuthorizedUser()
        {
            try
            {
                var userNameViewModel = userPersistence.GetFullNameFromUser(User?.Identity?.Name);

                return PartialView(userNameViewModel);
            }
            catch (Exception ex)
            {                
                return PartialView(new UserNameViewModel());
            }
        }
    }
}