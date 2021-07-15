using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace de_exceptional_closures.Controllers
{
    [Authorize]
    public class ClosureController : Controller
    {
        [HttpGet]
        public IActionResult PreApproved()
        {

            return View();
        }

        [HttpGet]
        public IActionResult ApprovalRequired()
        {
            return View();
        }
    }
}
