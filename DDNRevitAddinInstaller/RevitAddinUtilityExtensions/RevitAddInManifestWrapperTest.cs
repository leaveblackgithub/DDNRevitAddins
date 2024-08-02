using NUnit.Framework;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    [TestFixture]
    public class RevitAddInManifestWrapperTest:RevitAddinTestBase
    {
        [Test]
        public void CreateRevitAddInManifestWrapperTest()
        {
            RevitAddInManifestWrapper wrapper = ValidTestRevitInfo.CreateRevitAddInManifestWrapper();
            Assert.AreEqual(1,wrapper.AddinCommands.Count);
            Assert.AreEqual(FullClassName,wrapper.AddinCommands[0].FullClassName);
        }

        [Test]
        public void CreateFromAddinFileInfoTest()
        {
            RevitAddInManifestWrapper wrapper= RevitAddInManifestWrapper.CreateFromAddinFileInfo(AddinManagerFile[0]);
            Assert.AreEqual(3,wrapper.InstalledClasses.Count);
            wrapper = RevitAddInManifestWrapper.CreateFromAddinFileInfo(HandlerFileAddins[0]);
            Assert.AreEqual(1, wrapper.InstalledClasses.Count);
        }
    }
}
