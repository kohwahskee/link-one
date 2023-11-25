using System.Diagnostics;
using UIAutomationClient;

namespace CaptureOneAutomation
{
    class WindowManager
    {
        /**
         <summary>
         Sets the visual state of the specified window.
         </summary>
         <param name="window">The <see cref="IUIAutomationElement"/> representing the window to set the state for.</param>
         <param name="state">The desired <see cref="WindowVisualState"/>.</param>
        */
        public static void SetWindowState(IUIAutomationElement window, WindowVisualState state)
        {
            IUIAutomationWindowPattern? windowPattern = window.GetCurrentPattern(UIA_PatternIds.UIA_WindowPatternId) as IUIAutomationWindowPattern;
            windowPattern?.SetWindowVisualState(state);
        }

        public static void MaximizeWindow(IUIAutomationElement window)
        {
            SetWindowState(window, WindowVisualState.WindowVisualState_Maximized);
        }

        public static void RestoreWindow(IUIAutomationElement window)
        {
            SetWindowState(window, WindowVisualState.WindowVisualState_Normal);
        }
    }
}
