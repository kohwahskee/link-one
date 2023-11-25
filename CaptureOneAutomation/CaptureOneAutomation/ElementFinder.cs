using CaptureOneAutomation.CaptureOneAutomation;
using UIAutomationClient;
namespace CaptureOneAutomation
{
    internal class ElementFinder
    {
        private readonly IUIAutomation Automation;
        public IUIAutomationElement? CaptureOneTopLevelWindow { get; set; }

        public ElementFinder(IUIAutomation automation)
        {
            Automation = automation;
        }

        private IUIAutomationElement? GetParentPanel()
        {
            var localizedControlTypeCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "pane");
            var automationIdCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "windowLayoutControl1");
            var andCondition = Automation.CreateAndCondition(automationIdCondition, localizedControlTypeCondition);

            var captureOneTopLevelWindow = CaptureOneTopLevelWindow ?? GetMainWindow();

            if (captureOneTopLevelWindow != null && Has(captureOneTopLevelWindow))
            {
                WindowManager.MaximizeWindow(captureOneTopLevelWindow);
                return captureOneTopLevelWindow.FindFirst(TreeScope.TreeScope_Children, andCondition);
            }
            else
            {
                throw new ElementNotFoundException("No Parent Panel found");
            }
        }

        private IUIAutomationElement? GetCameraToolPanel()
        {
            var parent = GetParentPanel();
            var automationIdCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "ThisCameraTool");

            return parent?.FindFirst(TreeScope.TreeScope_Descendants, automationIdCondition);
        }

        public IUIAutomationElement? GetLiveViewButton()
        {
            var parent = GetCameraToolPanel();
            var localizedControlTypeCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "button");

            if (parent == null) throw new ElementNotFoundException("No parent found");

            var allButtons = parent.FindAll(TreeScope.TreeScope_Children, localizedControlTypeCondition);

            // Get button whose child is an image type
            for (int i = 0; i < allButtons.Length; i++)
            {
                var imageTypeCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "image");
                var liveViewButton = allButtons.GetElement(i).FindFirst(TreeScope.TreeScope_Children, imageTypeCondition);
                if (liveViewButton != null)
                {
                    return allButtons.GetElement(i);
                }
            }

            return null;
        }

        public IUIAutomationElement? GetCaptureButton()
        {
            var parent = GetCameraToolPanel();
            var localizedControlTypeCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "button");

            if (parent == null) throw new ElementNotFoundException("No parent found");

            var allButtons = parent.FindAll(TreeScope.TreeScope_Children, localizedControlTypeCondition);

            // Get button which contains no child
            for (int i = 0; i < allButtons.Length; i++)
            {
                var currentButton = allButtons.GetElement(i);
                var trueCondition = Automation.CreateTrueCondition();
                var currentButtonChildren = currentButton.FindAll(TreeScope.TreeScope_Children, trueCondition);

                if (currentButtonChildren.Length == 0)
                {
                    return currentButton;
                }
            }

            return null;
        }

        public IUIAutomationElement? GetMainWindow()
        {
            if (CaptureOneTopLevelWindow != null) return CaptureOneTopLevelWindow;

            var allTopLevelElements = Automation.GetRootElement().FindAll(TreeScope.TreeScope_Children, Automation.CreateTrueCondition());

            for (int i = 0; i < allTopLevelElements.Length; i++)
            {
                var currentElement = allTopLevelElements.GetElement(i);
                var localizedControlTypeCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_LocalizedControlTypePropertyId, "pane");
                var automationIdCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "tableLayoutPanelMenuStrip");
                var andCondition = Automation.CreateAndCondition(automationIdCondition, localizedControlTypeCondition);
                var nearestDirectChild = currentElement.FindFirst(TreeScope.TreeScope_Children, andCondition);
                if (nearestDirectChild != null)
                {
                    return allTopLevelElements.GetElement(i);
                }
            }

            throw new CaptureOneNotOpenedException("No Capture One window found");
        }

        public IUIAutomationElement? GetLiveViewWindow()
        {
            var nameCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "Live View");
            var automationIdCondition = Automation.CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "LivePreviewWindow3");
            var andCondition = Automation.CreateAndCondition(automationIdCondition, nameCondition);
            var liveViewWindow = Automation.GetRootElement().FindFirst(TreeScope.TreeScope_Children, andCondition);

            return liveViewWindow;
        }

        /**
         <summary>
        Check whether an Element exists or is not stale by accessing its CurrentName property. If an exception is thrown, the element no longer exists or has become stale.
         </summary>
        **/
        public static bool Has(IUIAutomationElement? element)
        {
            if (element == null) return false;

            try
            {
                _ = element.CurrentName;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
