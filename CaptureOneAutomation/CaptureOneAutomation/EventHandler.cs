using UIAutomationClient;

namespace CaptureOneAutomation.CaptureOneAutomation
{
    internal class WindowEventListener
    {
        private readonly IUIAutomation _Automation;

        public WindowEventListener(IUIAutomation automation)
        {
            _Automation = automation;
        }

        public void AddWindowClosedEvent(IUIAutomationElement targetElement, EventAction action)
        {
            _Automation.AddAutomationEventHandler(
                eventId: UIA_EventIds.UIA_Window_WindowClosedEventId,
                element: targetElement,
                scope: TreeScope.TreeScope_Element,
                cacheRequest: null,
                handler: new WindowEventHandler(action)
                );

            Console.WriteLine("Added WindowClosed event");
        }

        public void AddWindowOpenedEvent(EventAction action)
        {
            var desktop = _Automation.GetRootElement();
            _Automation.AddAutomationEventHandler(
                eventId: UIA_EventIds.UIA_Window_WindowOpenedEventId,
                element: desktop,
                scope: TreeScope.TreeScope_Children,
                cacheRequest: null,
                handler: new WindowEventHandler(action)
                );

            Console.WriteLine("Added WindowOpened event");
        }
    }

    internal class WindowEventHandler : IUIAutomationEventHandler
    {
        private readonly EventAction Action;

        public WindowEventHandler(EventAction action)
        {
            Action = action;
        }

        public void HandleAutomationEvent(IUIAutomationElement sender, int eventId)
        {
            // FIXME: Event not triggered when closed (triggered when new window opened or on window reposition)
            // if sender.CurrentName == "", then window was moved, not opened
            switch (eventId)
            {
                case UIA_EventIds.UIA_Window_WindowOpenedEventId:
                    HandleWindowOpenedEvent(sender, Action);
                    break;
                case UIA_EventIds.UIA_Window_WindowClosedEventId:
                    HandleWindowClosedEvent(sender, Action);
                    break;
            }
        }

        private void HandleWindowOpenedEvent(IUIAutomationElement sender, EventAction action)
        {
            action(sender);
        }

        private void HandleWindowClosedEvent(IUIAutomationElement sender, EventAction action)
        {
            action(sender);
        }
    }

    public delegate void EventAction(IUIAutomationElement sender);
}
