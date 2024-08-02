using System.Collections;
using System.Collections.Generic;
using System.IO;
using Autodesk.RevitAddIns;
using DDNGeneralLibrary;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public class RevitAddInManifestWrapper
    {
        private RevitAddInManifest _addin;
        private FileInfo _addinFileInfo;
        private IDictionary<string, FileInfo> _installedClasses;
        public const string VendorId = "DDNC";

        public RevitAddInManifestWrapper()
        {
            _addin = new RevitAddInManifest();
        }


        private RevitAddInManifestWrapper(FileInfo addinFileInfo)
        {
            _addinFileInfo = addinFileInfo;
            _addin = RevitAddInManifestUtilityExtension.ExtGetRevitAddInManifest(_addinFileInfo.FullName);
        }

        public static void CreateRevitAddInManifestWrapper(RevitAddinInfo revitAddinInfoNew, string strAddinPath)
        {
            var addinWrapper = revitAddinInfoNew.CreateRevitAddInManifestWrapper();
            addinWrapper.SaveAs(strAddinPath + revitAddinInfoNew.CommandText.AddAddinExtension());
        }

        public void AddCommand(RevitAddInCommand command1)
        {
            _addin.AddInCommands.Add(command1);
        }

        public void AddApplication(RevitAddInApplication app1)
        {
            _addin.AddInApplications.Add(app1);

        }

        public void SaveAs(string path)
        {
            _addin.SaveAs(path);
        }

        public static RevitAddInManifestWrapper CreateFromAddinFileInfo(FileInfo fileInfo)
        {
            try
            {
                return new RevitAddInManifestWrapper(fileInfo);
            }
            catch
            {
                return null;
            }
        }

        public IDictionary<string, FileInfo> InstalledClasses => _installedClasses ??
                                                                 (_installedClasses = GetInstalledClasses());

        private IDictionary<string, FileInfo> GetInstalledClasses()
        {
            var classesInstalled = new Dictionary<string, FileInfo>();
            foreach (RevitAddInCommand command in _addin.AddInCommands)
            {
                string strClass = command.FullClassName;
                if (classesInstalled.ContainsKey(strClass)) continue;
                classesInstalled.Add(strClass, _addinFileInfo);
            }
            foreach (RevitAddInApplication application in _addin.AddInApplications)
            {
                string strClass = application.FullClassName;
                if (classesInstalled.ContainsKey(strClass)) continue;
                classesInstalled.Add(strClass, _addinFileInfo);
            }
            return classesInstalled;
        }

        public List<RevitAddInCommand> AddinCommands => _addin.AddInCommands;
        public IList AddinApplications => _addin.AddInApplications;
    }
}
