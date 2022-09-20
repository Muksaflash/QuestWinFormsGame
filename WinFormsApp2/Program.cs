namespace WinFormsApp2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            var form = new Form1();
            form.WindowState = FormWindowState.Maximized;
            form.Size = Screen.PrimaryScreen.Bounds.Size;
            Application.Run(form);
        }
    }
}