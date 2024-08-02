using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.RevitAddIns;
using DDNGeneralLibrary;
using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;
using DDNRevitAddins.General;

namespace DDNRevitAddinInstaller
{
    public class DDNRevitAddinHandler
    {
        public const string StrPrefix = "DDN_";
        public  static string AddinDllPath =>ApplicationExtension.ExtGetStartUpPath();
        private IDictionary<string, FileInfo> _classesInstalled = null;
        private IList<FileInfo> _fileAddins;
        private IList<FileInfo> _fileDlls;
        private string _strAddinPath;
        private DDNRevitAddinLoader _ddnRevitAddinLoader = null;
        private IList<string> _dllUsed =null;
        private  Dictionary<string, RevitAddinInfo> _classesNew = new Dictionary<string, RevitAddinInfo>();
        private const string LoaderFileFullName = @".\DDNRevitAddinLoader.txt";

        public IList<string> DllUsed => _dllUsed ?? (_dllUsed = RevitAddinLoader.DllUsed);

        public string RevitAddinsPath => _strAddinPath ?? (_strAddinPath = GetAddinPath());

        public IList<FileInfo> FileAddins => _fileAddins ?? (_fileAddins = RevitProductExtension.GetAddinFiles( AddPrefix( FileInfoExtension.AddinSearchExtension)));
        public IList<FileInfo> FileDlls => _fileDlls ?? (_fileDlls = GetDllFiles());

        public IDictionary<string, FileInfo> ClassesInstalled => _classesInstalled ??
                                                                 (_classesInstalled = GetInstalled());

        public DDNRevitAddinLoader RevitAddinLoader => _ddnRevitAddinLoader ??
                                                       (_ddnRevitAddinLoader = new DDNRevitAddinLoader(LoaderFileFullName, AddinDllPath));

        public Dictionary<string, RevitAddinInfo> ClassesNew => _classesNew ?? (_classesNew = RevitAddinLoader.NewRevitAddinInfos);

        public void InstallAddins()
        {
            RevitAddinLoader.InstallAddins(ClassesInstalled, RevitAddinsPath);
            //create a new addin manifest
            //create an external command

            ShowResultMessage();
            ShowErrorMessage();
            //HistoryCode();
        }

        private void ThrowNewExceptionOfCancel(string message)
        {
            throw new ExceptionToCancel(message);
        }

        private string GetAddinPath()
        {
            var currentProductAllUsersAddInFolder = RevitProductExtension.GetCurrentProductAllUsersAddInFolder();
            if (currentProductAllUsersAddInFolder == null) ThrowNewExceptionOfCancel("Can't Find Addin Folders");
            var strAddinPath = currentProductAllUsersAddInFolder;
            return strAddinPath;
        }

        private IList<FileInfo> GetDllFiles()
        {
            IList<FileInfo> fileInfos = DirectoryInfoExtension.GetFiles(AddinDllPath, FileInfoExtension.DllSearchExtension);
            if (!_fileDlls.Any()) ThrowNewExceptionOfCancel("Can't find Dll Files");
            var i = 1;
            foreach (var file in fileInfos)
            {
                if (DllUsed.Contains(file.Name)) continue;
                StrErrorDll.AppendLine(i + ": " + file.Name);
                i = i + 1;
            }
            return fileInfos;
        }


        private StringBuilder StrErrorDll { get; } = new StringBuilder();
        private StringBuilder StrErrorMessage { get; } = new StringBuilder();
        private StringBuilder StrResultMessage { get; } = new StringBuilder();

        
        public void AddMessage(StringBuilder messageBuilder,string  title, StringBuilder message)
        {
            if (message.Length == 0) return;
            messageBuilder.AppendLine(title);
            messageBuilder.Append(message.ToString());
            messageBuilder.ExtAppendDoubleLines();
        }
        private void ShowResultMessage()
        {
            AddMessage(StrResultMessage, "CLASSES INSTALLED:", RevitAddinLoader.StrInstalled);
            AddMessage(StrResultMessage, "CLASSES UNINSTALLED", RevitAddinLoader.StrUninstalled);
            ShowMessage("RESULT",StrResultMessage);
        }

        private void ShowMessage(string caption,StringBuilder stringBuilder)
        {
            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString(), caption);
            }
        }


        private void ShowErrorMessage()
        {
            AddMessage(StrErrorMessage,"COULD NOT FIND DLLS FOR:",RevitAddinLoader.StrErrorLine);
            AddMessage(StrErrorMessage,"DUPLICATED CLASS NAME AT:",RevitAddinLoader.StrDuplicated);
            AddMessage(StrErrorMessage,"USELESS DLLS AT:",StrErrorDll);
            ShowMessage("ERROR", StrErrorMessage);
        }

        private IDictionary<string, FileInfo> GetInstalled()
        {
            var addins = new RevitAddInManifestCollection(FileAddins);
            return addins.InstalledClasses;
        }

        public static string AddPrefix(string text)
        {
            return StrPrefix + text;
        }
    }
}