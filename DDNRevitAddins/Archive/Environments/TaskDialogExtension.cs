using Autodesk.Revit.UI;

namespace DDNRevitAddins.Archive.Environments
{
    public static class TaskDialogExtension
    {
        public static void ExtShow(string title, string message)
        {
            TaskDialog.Show(title, message);
        }
    }
}
