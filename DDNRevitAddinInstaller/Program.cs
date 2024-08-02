

using System;
using System.Windows.Forms;
using DDNGeneralLibrary;
using DDNRevitAddins.General;

namespace DDNRevitAddinInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                DDNRevitAddinHandler handler = new DDNRevitAddinHandler();
                handler.InstallAddins();
            }
            catch (ExceptionToCancel e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }
    }
}
