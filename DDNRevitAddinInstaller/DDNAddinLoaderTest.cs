using System.Collections.Generic;
using System.IO;
using System.Linq;
using DDNGeneralLibrary;
using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;
using Moq;
using NUnit.Framework;

namespace DDNRevitAddinInstaller
{
    [TestFixture]
    public class DDNAddinLoaderTest : RevitAddinTestBase
    {
        private Dictionary<string, RevitAddinInfo> _newRevitAddinInfos;
        private RevitAddinInfo _newRevitAddinInfo1;
        private string _addinFilePath;

        [SetUp]
        public void Setup()
        {
            _newRevitAddinInfos = Loader.NewRevitAddinInfos;
            _newRevitAddinInfo1 = _newRevitAddinInfos.ElementAt(0).Value;
        }

        private void LoaderGetInstalled(string addinFullName,FileInfo addinFileInfo)
        {
            Dictionary<string, FileInfo> _installedClasses = new Dictionary<string, FileInfo>
            {
                {addinFullName, addinFileInfo},
            };
            _addinFilePath = TestClassBase.TestFolder;
            Loader.GetInstalled(_installedClasses, _addinFilePath);
        }

        [Test]
        public void UninstallTest()
        {
            string commandText = ExtraRevitAddinInfo.CommandText;
            var extraAddinFile = GetExtraAddinFile(commandText);
            LoaderGetInstalled(ExtraRevitAddinInfo.FullClassName, extraAddinFile);
            Assert.True(extraAddinFile.Exists);
            Loader.UnInstallAddins();
            Assert.AreEqual("1: DDNRevitAddins.MultipleUnCut2\r\n", Loader.StrUninstalled.ToString());
            Assert.False(GetExtraAddinFile(commandText).Exists);
            
        }

        private FileInfo GetExtraAddinFile(string commandText)
        {
            return FileInfoExtension.GetAddinFileInfo(TestAddinFilePath, commandText);
        }

        [Test]
        public void InstallAddinTest()
        {
            Loader.InstallAddin(_newRevitAddinInfo1, 0);
            string commandText = _newRevitAddinInfo1.CommandText;
            FileInfo addinFileInfo = FileInfoExtension.GetAddinFileInfo(_addinFilePath, commandText);
            Assert.AreEqual(commandText.AddAddinExtension(),addinFileInfo.ExtName());
            addinFileInfo.ExtDelete();
            Assert.AreEqual("0: DDNRevitAddins.ReloadImage\r\n", Loader.StrInstalled.ToString());
        }
        [Test]
        public void IsAddinInstalledTest()
        {
            LoaderGetInstalled(_newRevitAddinInfo1.FullClassName, null);
            Assert.True(Loader.AddinIsInstalled(_newRevitAddinInfo1.FullClassName));
        }
        [Test]
        public void CreateValidRevitAddinFromLine()
        {
            var revitAddinInfoFromLine = Loader.CreateValidRevitAddinInfoFromLine(LoaderLine0, 0);
            Assert.NotNull(revitAddinInfoFromLine);
            Assert.True(Loader.CheckErrorOrDuplicate(LoaderLine0, 0, revitAddinInfoFromLine));
        }

        [Test]
        public void GetNewRevitInfosTest()
        {
            Assert.AreEqual(3, _newRevitAddinInfos.Count);
            Assert.AreEqual(FullClassName, _newRevitAddinInfo1.FullClassName);
            Assert.AreEqual("Line 5: DDNRevitAddins2,DDNRevitAddins.MultipleUnCut,com\r\n",
                Loader.StrErrorLine.ToString());
            Assert.AreEqual("Line 2: DDNRevitAddins,DDNRevitAddins.ReloadImage,com\r\n",
                Loader.StrDuplicated.ToString());
        }

        [Test]
        public void ReadLoaderLineTest()
        {
            Assert.AreEqual(5, Lines.Count);
        }
    }
}