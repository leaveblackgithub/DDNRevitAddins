using Autodesk.RevitAddIns;
using DDNGeneralLibrary;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public static class RevitAddinApplicationExtension
    {
        public static RevitAddInApplication Create(string assembly,string fullClassName)
        {
            return new RevitAddInApplication(fullClassName, assembly, GuidExtension.NewGUID(),
                fullClassName, RevitAddInManifestWrapper.VendorId);
        }
    }
}
