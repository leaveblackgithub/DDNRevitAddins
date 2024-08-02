using System.Collections.Generic;
using System.IO;
using DDNGeneralLibrary;
using NUnit.Framework;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public class RevitAddinTestBase
    {
        internal  RevitAddinInfo ValidTestRevitInfo;
        internal DDNRevitAddinHandler Handler;

        [SetUp]
        public void SetUp()
        {
            Handler = new DDNRevitAddinHandler();
            HandlerFileAddins = Handler.FileAddins;

            string loaderFileFullName = TestClassBase.GetTestFileFullName("DDNRevitAddinLoaderTest.txt");
            TestAddinDllPath = FileInfoExtension.CurrentFolder;
            Loader = new DDNRevitAddinLoader(loaderFileFullName, TestAddinDllPath);

            Lines = Loader.ReadLoaderLines();
            LoaderLine0 = Lines[0];
            TestRevitInfo = RevitAddinInfo.CreateFromLine(LoaderLine0, TestAddinDllPath);
            ValidTestRevitInfo = RevitAddinInfo.CreateValidFromLine(LoaderLine0,
                TestAddinDllPath);
            AddinManagerFile = RevitProductExtension.GetAddinFiles("*AddinManager.addin");

            ExtraRevitAddinInfo = RevitAddinInfo.CreateFromLine(extraAddinLine,TestAddinDllPath);
            TestAddinFilePath = TestClassBase.TestFolder;
            RevitAddInManifestWrapper.CreateRevitAddInManifestWrapper(ExtraRevitAddinInfo,TestAddinFilePath);
        }

        internal IList<FileInfo> AddinManagerFile;
        internal DDNRevitAddinLoader Loader;
        internal IList<string> Lines;
        internal RevitAddinInfo TestRevitInfo;
        internal string LoaderLine0;
        internal const string FullClassName = "DDNRevitAddins.ReloadImage";
        internal const string CommandText = "DDN_ReloadImage";
        internal const string AssemblyPath = @".\DDNRevitAddins.dll";
        internal string TestAddinDllPath;
        internal string TestAddinFilePath;
        internal RevitAddinInfo ExtraRevitAddinInfo;
        internal IList<FileInfo> HandlerFileAddins;
        internal const string extraAddinLine = "DDNRevitAddins,DDNRevitAddins.MultipleUnCut2,com";
    }
}