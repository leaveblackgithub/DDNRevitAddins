using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Datas;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins.Archive.Environments
{
    public static class SelectionExtension
    {
        public static Reference ExtPickElementOfCategory(this Selection selection, string categoryName, string statusPrompt)
        {
            try
            {
                return selection.PickObject(ObjectType.Element, new CategoryNameSelectionFilter(categoryName), statusPrompt);
            }
            catch (OperationCanceledException )
            {
                return null;
            }
        }
        public static Reference ExtPickElement(this Selection selection, string statusPrompt)
        {
            try
            {
                return selection.PickObject(ObjectType.Element, statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
        public static IList<Reference> ExtPickElementsOfCategory(this Selection selection, string categoryName, string statusPrompt)
        {
            try
            {
                return selection.PickObjects(ObjectType.Element, new CategoryNameSelectionFilter(categoryName), statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
        public static IList<Reference> ExtPickElements(this Selection selection,
            ISelectionFilter selectionFilter, string statusPrompt)
        {
            try
            {
                return selection.PickObjects(ObjectType.Element,  selectionFilter,statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
        public static XYZ ExtPickPoint(this Selection selection, ObjectSnapTypes objectSnapTypes,string  statusPrompt)
        {
            try
            {
                return selection.PickPoint(objectSnapTypes, statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public static IList<Reference> ExtPickElements(this Selection selection, string statusPrompt)
        {
            try
            {
                return selection.PickObjects(ObjectType.Element,  statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }

        public static Reference ExtPickObject(this Selection selection, ObjectType objtype,string statusPrompt)
        {
            try
            {
                return selection.PickObject(objtype, statusPrompt);
            }
            catch (OperationCanceledException)
            {
                return null;
            }

        }

    }
}
