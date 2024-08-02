using Autodesk.RevitAddIns;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public static class RevitAddInManifestUtilityExtension
    {
        public static RevitAddInManifest ExtGetRevitAddInManifest(string addinFileFullName)
        {
            return AddInManifestUtility.GetRevitAddInManifest(addinFileFullName);
        }
    }
}
