using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.RevitAddIns;
using DDNGeneralLibrary;
using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;

namespace DDNRevitAddinInstaller
{
    public class RevitAddinInfo
    {
        public RevitAddinInfo()
        {
        }
        public string AssemblyPath { get; set; }

        public string DllName { get; set; }

        public string FullClassName { get; set; }

        public string CommandText { get; set; }

        public string Type { get; set; }
        public string DllFullName { get; set; }

        public static RevitAddinInfo CreateValidFromLine(string line, string addinDllPath)
        {
            var revitAddinInfo = CreateFromLine(line, addinDllPath);
            if (revitAddinInfo.IsValid()) return revitAddinInfo;
            return null;
        }

        public  bool IsValid()
        {
            FileInfo fileDll = new FileInfo(AssemblyPath);
            if (fileDll.Exists == false) return false;
            return true;
        }

        public static RevitAddinInfo CreateFromLine(string line, string addinDllPath)
        {
            IList<string> strDlls = line.ExtSplit(StringExtension.TextSeparatorComma);
            string fullClassName = strDlls[1];
            IList<string> namespaces = fullClassName.ExtSplit(StringExtension.TextSeparatorDot);
            string dllName = strDlls[0];
            string dllFullName = dllName.AddDllExtension();
            string type = strDlls[2];
            string commandText = DDNRevitAddinHandler.AddPrefix(namespaces.Last());
            var revitAddinInfo = new RevitAddinInfo(addinDllPath, dllName, fullClassName, type, commandText, dllFullName);
            return revitAddinInfo;
        }

        public RevitAddinInfo (string addinDllPath, string dllName, string fullClassName, string type,
            string commandText, string dllFullName)
        {
            DllName = dllName;
            FullClassName = fullClassName;
            Type = type;
            CommandText = commandText;
            DllFullName = dllFullName;
            AssemblyPath = addinDllPath + dllFullName;
        }

        public RevitAddInCommand CreateRevitAddinCommand()
        {
            return RevitAddinCommandExtension.Create(AssemblyPath, FullClassName, CommandText);
        }

        public RevitAddInApplication CreateRevitAddinApplication()
        {
            return RevitAddinApplicationExtension.Create(AssemblyPath, FullClassName);
        }

        public RevitAddInManifestWrapper CreateRevitAddInManifestWrapper()
        {
            RevitAddInManifestWrapper result = new RevitAddInManifestWrapper();
            if (Type == DDNRevitAddinLoader.CommandTypeName)
            {
                result.AddCommand(CreateRevitAddinCommand());
            }
            else
            {
                result.AddApplication(CreateRevitAddinApplication());
            }
            return result;
        }
    }
}