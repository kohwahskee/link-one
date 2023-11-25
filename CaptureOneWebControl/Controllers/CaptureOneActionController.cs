using CaptureOneAutomation;
using CaptureOneWebControl.AutomatorBridge;
using CaptureOneWebControl.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaptureOneWebControl.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CaptureOneActionController : ControllerBase
    {
        private readonly RequestHandler requestHandler;

        public CaptureOneActionController(Automator automator)
        {
            requestHandler = new(automator);
        }

        [HttpPost]
        public ActionResult<AutomationActionResult> Post(CaptureOneActionModel request)
        {
            var response = requestHandler.From(request);

            return new OkObjectResult(response);
        }
    }
}