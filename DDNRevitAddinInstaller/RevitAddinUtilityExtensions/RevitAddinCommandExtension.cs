using Autodesk.RevitAddIns;
using DDNGeneralLibrary;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public static class RevitAddinCommandExtension
    {
        public static RevitAddInCommand Create(string assembly, string fullClassName, string commandText)
        {
            RevitAddInCommand command1 = new RevitAddInCommand(assembly, GuidExtension.NewGUID(), fullClassName,
                RevitAddInManifestWrapper.VendorId)
            {
                Description = StringExtension.ExtEmpty(),
                Text = commandText,
                VisibilityMode = VisibilityMode.AlwaysVisible
            };
            // this command only visible in Revit MEP, Structure, and only visible
            // in Project document or when no document at all
            return command1;
        }
    }
}
