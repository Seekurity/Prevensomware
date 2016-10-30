using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Prevensomware.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegisterAppInWindowsRegistry();
            Application.Run(new MainForm());
        }
        public static void RegisterAppInWindowsRegistry()
        {
            if (Registry.GetValue(@"HKEY_CLASSES_ROOT\*\shell\Revert File\command", "", null) != null) return;
            try
            {
                var registryKey = Registry.ClassesRoot.OpenSubKey("*").OpenSubKey("shell", true);
                registryKey = registryKey.CreateSubKey("Revert File");
                registryKey = registryKey.CreateSubKey("command");
                registryKey.SetValue("", Application.ExecutablePath + " \"%1\"");
            }
            catch{}
        }
    }
}
