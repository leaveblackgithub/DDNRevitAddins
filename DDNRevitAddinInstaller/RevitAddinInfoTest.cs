using DDNRevitAddinInstaller.RevitAddinUtilityExtensions;
using NUnit.Framework;

namespace DDNRevitAddinInstaller
{
    [TestFixture]
    public class RevitAddinInfoTest : RevitAddinTestBase
    {
        [Test]
        public void CreateValidFromLineTest()
        {
            Assert.AreEqual(FullClassName, ValidTestRevitInfo.FullClassName);
            Assert.AreEqual(AssemblyPath, ValidTestRevitInfo.AssemblyPath);
            Assert.AreEqual(CommandText, ValidTestRevitInfo.CommandText);
        }

        [Test]
        public void CreateFromLineTest()
        {
            Assert.AreEqual(FullClassName, TestRevitInfo.FullClassName);
            Assert.AreEqual(AssemblyPath, TestRevitInfo.AssemblyPath);
            Assert.AreEqual(CommandText, TestRevitInfo.CommandText);
        }

        [Test]
        public void IsValidTest()
        {
            Assert.True(TestRevitInfo.IsValid());
        }
    }
}
