using System.Web.Mvc;

namespace SurveyApp.Web.Controllers {
  public class HomeController : Controller {
    public ActionResult Index() {
      return View();
    }
  }
}