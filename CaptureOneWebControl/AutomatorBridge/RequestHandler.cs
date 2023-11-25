using CaptureOneAutomation;
using CaptureOneWebControl.Models;

namespace CaptureOneWebControl.AutomatorBridge
{
    public class RequestHandler
    {
        readonly Automator _automator;
        public RequestHandler(Automator automator)
        {
            _automator = automator;
        }
        public AutomationActionResult From(CaptureOneActionModel request)
        {
            string? action = request.Action;
            // Should probably handle exceptions here instead of inside Automator
            if (action != null)
            {
                AutomationActionResult result;
                switch (action)
                {
                    case "invokeLiveView":
                        result = _automator.InvokeLiveView();
                        Console.WriteLine($"Action was a {result.Status.ToString().ToLower()}");
                        return result;
                    case "invokeCapture":
                        result = _automator.InvokeCapture();
                        Console.WriteLine($"Action was a {result.Status.ToString().ToLower()}");
                        return result;
                    default:
                        Console.WriteLine("No aciton found");
                        return new AutomationActionResult(AutomationActionResult.ResultStatus.Success, "No action found");
                }
            }

            return new AutomationActionResult(AutomationActionResult.ResultStatus.Fail, "An action is required");
        }
    }
}
