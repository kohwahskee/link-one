namespace CaptureOneAutomation.CaptureOneAutomation
{
    public class ElementNotFoundException : Exception
    {
        public ElementNotFoundException() { }
        public ElementNotFoundException(string message) : base(message) { }
        public ElementNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UninvokeableButtonException : Exception
    {
        public UninvokeableButtonException() { }
        public UninvokeableButtonException(string message) : base(message) { }
        public UninvokeableButtonException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class CaptureOneNotOpenedException : Exception
    {
        public CaptureOneNotOpenedException() { }
        public CaptureOneNotOpenedException(string message) : base(message) { }
        public CaptureOneNotOpenedException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class NotAnActionException : Exception
    {
        public NotAnActionException() { }
        public NotAnActionException(string message) : base(message) { }
        public NotAnActionException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class StaleElementException : Exception
    {
        public StaleElementException() { }
        public StaleElementException(string message) : base(message) { }
        public StaleElementException(string message, Exception inner) : base(message, inner) { }
    }

    public static class ExceptionHandler
    {
        public static AutomationActionResult Handle(Action action)
        {
            try
            {
                action();
                return new AutomationActionResult(AutomationActionResult.ResultStatus.Success);
            }
            catch (ElementNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                return new AutomationActionResult(AutomationActionResult.ResultStatus.Fail, ex.Message);
            }
            catch (CaptureOneNotOpenedException ex)
            {
                Console.WriteLine(ex.Message);
                return new AutomationActionResult(AutomationActionResult.ResultStatus.Fail, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AutomationActionResult(AutomationActionResult.ResultStatus.Fail, ex.Message);
            }
        }
    }
}
