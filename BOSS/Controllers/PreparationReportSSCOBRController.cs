using System.Web.Mvc;

namespace BOSS.Controllers
{
    public class PreparationReportSSCOBRController : Controller
    {
        // GET: PreparationReportSSCOBR
        [Authorize]
        public ActionResult ReportSSCOBR()
        {
            return View();
        }
    }
}