using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDNGeneralLibrary;
using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;

namespace DDNRevitAddinInstaller
{
    public class DDNRevitAddinLoader
    {
        private IDictionary<string, FileInfo> _classesInstalled;
        private string _strAddinPath;
        private string _addinDllPath;
        private string _loaderFileFullName;
        public Dictionary<string, RevitAddinInfo> NewRevitAddinInfos { get; } =new Dictionary<string, RevitAddinInfo>();
        public StringBuilder StrErrorLine { get; } = new StringBuilder();
        public StringBuilder StrDuplicated { get; } = new StringBuilder();
        public StringBuilder StrInstalled { get; } = new StringBuilder();
        public StringBuilder StrUninstalled { get; } = new StringBuilder();
        public List<string> DllUsed { get; } = new List<string>();

        public DDNRevitAddinLoader(string loaderFileFullName, string addinDllPath)
        {
            _loaderFileFullName = loaderFileFullName;
            IList<string> lines = ReadLoaderLines();
            _addinDllPath = addinDllPath;
            int i = 0;
            
            foreach (string line in lines)
            {
                i = i + 1;
                var revitAddinInfo = CreateValidRevitAddinInfoFromLine(line, i);
                if (CheckErrorOrDuplicate(line, i, revitAddinInfo)) continue;
                NewRevitAddinInfos.Add(revitAddinInfo.FullClassName, revitAddinInfo);
            }
        }

        public RevitAddinInfo CreateValidRevitAddinInfoFromLine(string line, int i)
        {
            return RevitAddinInfo.CreateValidFromLine(line, _addinDllPath);
        }

        public bool CheckErrorOrDuplicate(string line, int i, RevitAddinInfo revitAddinInfo)
        {
            if (revitAddinInfo == null)
            {
                StringLineGenerator(StrErrorLine, line, i);
                return true;
            }
            DllUsed.Add(revitAddinInfo.DllFullName);
            if (NewRevitAddinInfos.ContainsKey(revitAddinInfo.FullClassName))
            {
                StringLineGenerator(StrDuplicated, line, i);
                return true;
            }
            return false;
        }

        public IList<string> ReadLoaderLines()
        {
            return StreamReaderExtension.ReadLines(_loaderFileFullName);
        }


        private static void StringLineGenerator(StringBuilder stringBuilder,string line, int number)
        {
            stringBuilder.AppendLine("Line " + number.ToString() + ": " + line);
        }
        private static void StringClassGenerator(StringBuilder stringBuilder, string name, int number)
        {
            stringBuilder.AppendLine(number + ": " + name);
        }

        public void InstallAddins(IDictionary<string, FileInfo> classesInstalled, string strAddinPath)
        {
            GetInstalled(classesInstalled, strAddinPath);
            var i = 1;
            foreach (KeyValuePair<string, RevitAddinInfo> pair in NewRevitAddinInfos)
            {
                if (AddinIsInstalled(pair.Key))
                {
                   ;
                    continue;
                }
                InstallAddin(pair.Value, i);
                i = i + 1;
            }
            UnInstallAddins();
        }

        public void GetInstalled(IDictionary<string, FileInfo> classesInstalled, string strAddinPath)
        {
            _classesInstalled = classesInstalled;
            _strAddinPath = strAddinPath;
        }

        public void InstallAddin(RevitAddinInfo revitAddinInfo, int i)
        {
            RevitAddInManifestWrapper.CreateRevitAddInManifestWrapper(revitAddinInfo, _strAddinPath);
            StringClassGenerator(StrInstalled, revitAddinInfo.FullClassName, i);
        }

        public bool AddinIsInstalled(string addinName)
        {
            bool addinIsInstalled = _classesInstalled.ContainsKey(addinName);
            if(addinIsInstalled) _classesInstalled.Remove(addinName);
            return addinIsInstalled;
            
        }

        public void UnInstallAddins()
        {
            var i = 1;
            foreach (var classInstalled in _classesInstalled)
            {
                classInstalled.Value.ExtDelete();
                StringClassGenerator(StrUninstalled, classInstalled.Key, i);
                i = i + 1;
            }
        }

        public const string CommandTypeName = "com";
    }
}
