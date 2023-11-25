using System.Diagnostics;

namespace CaptureOneAutomation
{
    class ProcessHandler
    {
        public static string GetWindowTitleByName(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            Console.WriteLine(processes.Length);
            foreach (Process process in processes)
            {
                string windowTitle = process.MainWindowTitle;
                Console.WriteLine(windowTitle);
            }
            return "null";
        }

        public static Process? GetProcessByName(string name)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.MainWindowTitle == name) return process;
            }

            return null;
        }
    }
}
