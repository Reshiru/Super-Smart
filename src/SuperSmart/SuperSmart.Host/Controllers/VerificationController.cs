using SuperSmart.Core.Persistence.Implementation;
using SuperSmart.Core.Persistence.Interface;
using System.Web.Mvc;

namespace SuperSmart.Host.Controllers
{
    public class VerificationController : Controller
    {
        public VerificationController()
        {
            verificationPersistence = new VerificationPersistence();
        }

        IVerificationPersistence verificationPersistence;

        public ActionResult Login()
        {
            var sessionId = verificationPersistence.Login("", "");
            return View();
        }
    }
}