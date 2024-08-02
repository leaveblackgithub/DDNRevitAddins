using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDNGeneralLibrary
{
    public static class ApplicationExtension
    {
        public static string ExtGetStartUpPath()
        {
            return Application.StartupPath.ExtAddBackSplash();
        }
    }
}
