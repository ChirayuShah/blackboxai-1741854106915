using System;
using System.Windows.Forms;
using System.Threading;

namespace ChirayuAntivirus
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Enable visual styles
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Set up global exception handling
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // Run the application
            Application.Run(new MainForm());
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            LogManager.LogError("Thread Exception", e.Exception);
            MessageBox.Show("An error occurred. Please check the log file for details.", 
                          "ChirayuAntivirus Error", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Error);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            LogManager.LogError("Unhandled Exception", (Exception)e.ExceptionObject);
            MessageBox.Show("A critical error occurred. The application needs to close. Please check the log file for details.", 
                          "ChirayuAntivirus Critical Error", 
                          MessageBoxButtons.OK, 
                          MessageBoxIcon.Error);
        }
    }
}
