using CaptureOneAutomation.CaptureOneAutomation;
using System.Diagnostics;
using System.Text.Json.Serialization;
using UIAutomationClient;

namespace CaptureOneAutomation
{
    public class Automator
    {
        private readonly string CAPTUREONE_PROCESS_NAME = "CaptureOne";
        private IUIAutomationElement? LiveViewButton;
        private IUIAutomationElement? CaptureButton;
        private IUIAutomationElement? LiveViewWindow;
        private IUIAutomationElement? CaptureOneTopLevelWindow;
        private readonly CUIAutomation Automation;
        private readonly ElementFinder ElementFinder;
        private bool IsCaptureOneRunning { get; set; }

        public Automator()
        {
            IsCaptureOneRunning = false;
            Automation = new();
            ElementFinder = new ElementFinder(Automation);
            Init();
        }

        public AutomationActionResult InvokeCapture()
        {
            return ExceptionHandler.Handle(() =>
            {
                var captureButton = CaptureButton ?? ElementFinder.GetCaptureButton();
                if (captureButton != null) InvokeButton(captureButton);
            });
        }

        /**
         <summary>
         Show Live View. If Live View was already opened, click Live View button.
         Otherwise, either restore or maximize its window
         </summary>
        **/
        public AutomationActionResult InvokeLiveView()
        {
            return ExceptionHandler.Handle(() =>
            {
                try
                {
                    if (LiveViewWindow == null || LiveViewWindow.CurrentName.Length == 0)
                    {
                        throw new StaleElementException("Live Window no longer exists");
                    }

                    WindowManager.RestoreWindow(LiveViewWindow);
                }
                catch
                {
                    var liveViewButton = LiveViewButton ?? ElementFinder.GetLiveViewButton();

                    if (liveViewButton != null) InvokeButton(liveViewButton);

                    LiveViewWindow = ElementFinder.GetLiveViewWindow();
                }
            });
        }

        private void InvokeButton(IUIAutomationElement element)
        {
            var mainWindow = (ElementFinder.Has(CaptureOneTopLevelWindow) ? CaptureOneTopLevelWindow : ElementFinder.GetMainWindow()) ?? throw new CaptureOneNotOpenedException("Capture One not found");

            // Maximize main window to ensure button is interactable
            WindowManager.MaximizeWindow(mainWindow);

            var invokableButton = element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId) as IUIAutomationInvokePattern;

            try
            {
                invokableButton?.Invoke();
            }
            catch
            {
                throw new UninvokeableButtonException("Could not invoke button");
            }
        }

        /**
        <summary>
        Cache Capture One Elements for the 1st time and start listening to events
        </summary>
         */
        private void Init()
        {
            CacheElements();
            InitEvents();
        }

        /**
        <summary>
        Start listening to WindowOpened and WindowClosed Events
        <br/>
        On start, <see cref="CaptureOneTopLevelWindow"/> being neither null nor stale means an instance of Capture One is already opened
        </summary>
         */
        private void InitEvents()
        {
            var eventListener = new WindowEventListener(Automation);
            // Capture one opened
            if (CaptureOneTopLevelWindow != null && ElementFinder.Has(CaptureOneTopLevelWindow))
            {
                eventListener.AddWindowClosedEvent(CaptureOneTopLevelWindow, windowClosedEventHandler);
            }
            else
            {
                eventListener.AddWindowOpenedEvent(windowOpenedEventHandler);
            }

            // When Capture One is closed, empty all elements and start a new windowOpened event
            void windowClosedEventHandler(IUIAutomationElement _)
            {
                Automation.RemoveAllEventHandlers();
                IsCaptureOneRunning = false;
                DiscardElements();
                eventListener.AddWindowOpenedEvent(windowOpenedEventHandler);
            }

            // When an application is opened, try Caching elements. If elements are cached, that means 
            // an instance of Capture One has been opened, and attach a windowClosed event
            void windowOpenedEventHandler(IUIAutomationElement sender)
            {
                // Check if newly opened window is an instance of Capture One
                string? currentProcessname = ProcessHelper.GetNameByProcessId(sender.CurrentProcessId);

                if (currentProcessname != CAPTUREONE_PROCESS_NAME) return;

                try
                {
                    // Check if the instance is the splash screen
                    if (sender.CurrentAutomationId != "ContainerWindow")
                    {
                        CacheElements(sender);
                        Automation.RemoveAllEventHandlers();
                        eventListener.AddWindowClosedEvent(sender, windowClosedEventHandler);
                    }
                }
                catch
                {
                    IsCaptureOneRunning = false;
                }
            }
        }

        /**
        <summary>
        Cache <see cref="CaptureOneTopLevelWindow"/>, <see cref="LiveViewButton"/>, and <see cref="CaptureButton"/>
        </summary>
        <param name="mainWindow">The main Capture One window. If available, <see cref="ElementFinder.CaptureOneTopLevelWindow"/> will be set and cached to be used later</param>
         */
        private void CacheElements(IUIAutomationElement? mainWindow = null)
        {
            if (mainWindow != null)
            {
                ElementFinder.CaptureOneTopLevelWindow = mainWindow;
            }

            ExceptionHandler.Handle(() =>
            {
                CaptureOneTopLevelWindow = ElementFinder.GetMainWindow();
                LiveViewButton = ElementFinder.GetLiveViewButton();
                CaptureButton = ElementFinder.GetCaptureButton();
            });
        }

        private void DiscardElements()
        {
            ElementFinder.CaptureOneTopLevelWindow = null;
            CaptureOneTopLevelWindow = null;
            LiveViewButton = null;
            LiveViewWindow = null;
            CaptureButton = null;
        }
    }

    public class AutomationActionResult
    {
        public enum ResultStatus
        {
            Success,
            Fail,
            Timeout
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ResultStatus Status { get; set; }

        public string Reason { get; set; }

        public AutomationActionResult(ResultStatus status, string reason = "No reason given")
        {
            Status = status;
            Reason = reason;
        }
    }

    static class ProcessHelper
    {
        public static string? GetNameByProcessId(int processId)
        {
            try
            {
                var process = Process.GetProcessById(processId);
                return process.ProcessName;
            }
            catch
            {
                return null;
            }
        }
    }
}
