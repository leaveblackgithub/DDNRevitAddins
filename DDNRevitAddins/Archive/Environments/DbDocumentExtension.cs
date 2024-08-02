using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.Archive.Elements;
using DDNRevitAddins.Archive.Geometries;
using View = Autodesk.Revit.DB.View;

namespace DDNRevitAddins.Archive.Environments
{
    public static class DbDocumentExtension
    {
        public static ElementId ExtGetDefaultElementTypeId(this Document dbDocument, ElementTypeGroup defaultTypeId
        )
        {
            return dbDocument.GetDefaultElementTypeId(defaultTypeId);
        }
        public static ElementId ExtGetDefaultTextNoteType(this Document dbDocument)
        {
            return dbDocument.ExtGetDefaultElementTypeId(ElementTypeGroup.TextNoteType);
        }
        public static ElementId ExtGetDefaultFamilyTypeId(this Document dbDocument, ElementId familyCategoryId
        )
        {
            return dbDocument.GetDefaultFamilyTypeId(familyCategoryId);
        }

        public static DimensionType ExtGetDefaultDimensionType(this Document dbDocument)
        {
            return dbDocument.ExtGetElement(
                dbDocument.ExtGetDefaultFamilyTypeId(ElementIdExtension.ExtNewElementIdFrBuiltinCategory( BuiltInCategory.OST_Dimensions))) as DimensionType;
        }
        public static bool ExtIsFamilyDocument(this Document dbDocument)
        {
            return dbDocument.IsFamilyDocument;
        }

        public static FamilyManager ExtGetFamilyManager(this Document dbDocument)
        {
            return dbDocument.FamilyManager;
        }

        public static FamilySymbol ExtGetFamilySymbol(this Document dbDocument, Family family, string symbolName)
        {
            foreach (FamilySymbol symbol in dbDocument.ExtGetFamilySymbols(family))
            {
                if (symbol.Name == symbolName) return symbol;
            }

            return null;
        }

        public static IList<FamilySymbol> ExtGetFamilySymbols(this Document dbDocument, Family family)
        {
            return dbDocument.ExtGetElements(family.ExtGetFamilySymbolIds()).Select(e => (FamilySymbol) e).ToList();
        }

        public static Autodesk.Revit.Creation.Document ExtCreate(this Document dbDocument)
        {
            return dbDocument.Create;
        }

        public static Element ExtGetElement(this Document dbDocument, Reference reference)
        {
            return dbDocument.GetElement(reference);
        }

        public static IList<Element> ExtGetElements(this Document dbDocument, ICollection<Reference> references)
        {
            return references.Select(r => dbDocument.GetElement((Reference) r)).ToList();
        }

        public static IList<Element> ExtGetElements(this Document dbDocument, ICollection<ElementId> ids)
        {
            return ids.Select(id => dbDocument.ExtGetElement(id)).ToList();
        }

        public static Element ExtGetElement(this Document dbDocument, ElementId id)
        {
            return dbDocument.GetElement(id);
        }

        public static Transaction ExtNewTransaction(this Document dbDocument, string name)
        {
            Transaction transaction = new Transaction(dbDocument);
            if (transaction.Start(name) == TransactionStatus.Started) return transaction;
            return null;
        }

        public static Element ExtImportImage(this Document dbDocument, string path, XYZ insertPoint, View insertView)
        {
            ImagePlacementOptions options = new ImagePlacementOptions(insertPoint, BoxPlacement.Center);
            ImageTypeOptions typeOptions = new ImageTypeOptions(path, false, ImageTypeSource.Import);
            ImageType imageType = ImageType.Create(dbDocument, typeOptions);
            Element element = ImageInstance.Create(dbDocument, insertView, imageType.Id, options);
            return element;
        }

        public static void ExtDelete(this Document dbDocument, ElementId id)
        {
            dbDocument.Delete(id);
        }

        public static SketchPlane ExtCreateSketchPlane(this Document dbDocument, Plane plane)
        {
            return SketchPlane.Create(dbDocument, plane);
        }

        public static SketchPlane ExtCreateSketchPlaneByOriginAndXyPlane(this Document dbDocument, XYZ origin)
        {
            return dbDocument.ExtCreateSketchPlane(PlaneExtension.ExtCreateXyPlaneByOrigin(origin));
        }

        public static SketchPlane ExtCreateSketchPlaneByOriginAndBasis(this Document dbDocument, XYZ origin, XYZ basisX,
            XYZ basisY)
        {
            return dbDocument.ExtCreateSketchPlane(PlaneExtension.ExtCreateByOriginAndBasis(origin, basisX, basisY));
        }

        public static SketchPlane ExtCreateSketchPlaneByView(this Document dbDocument, View view)
        {
            XYZ origin = view.ExtGetOrigin();
            XYZ basisX = view.ExtGetRightDirection();
            XYZ basisY = view.ExtGetUpDirection();
            return dbDocument.ExtCreateSketchPlaneByOriginAndBasis(origin, basisX, basisY);
        }

        public static SketchPlane ExtCreateSketchPlaneByActiveView(this Document dbDocument)
        {
            return dbDocument.ExtCreateSketchPlaneByView(dbDocument.ExtGetActiveView());
        }

        public static ModelCurve ExtNewModelLineOnXyPlane(this Document dbDocument, XYZ point1, XYZ point2)
        {
            return dbDocument.ExtCreate()
                .ExtNewModelLine(point1, point2, dbDocument.ExtCreateSketchPlaneByOriginAndXyPlane(point1));
        }

        public static void ExtAddCutBetweenSolids(this Document dbDocument, Element solidToBeCut, Element cuttingSolid)
        {
            try
            {
                SolidSolidCutUtils.AddCutBetweenSolids(dbDocument, solidToBeCut, cuttingSolid);
            }
            catch
            {
            }
        }

        public static void ExtRemoveCutBetweenSolids(this Document dbDocument, Element solidToBeCut,
            Element cuttingSolid)
        {
            try
            {
                SolidSolidCutUtils.RemoveCutBetweenSolids(dbDocument, solidToBeCut, cuttingSolid);
            }
            catch
            {
            }
        }

        public static ICollection<ElementId> ExtGetSolidsBeingCut(this Document dbDocument, Element cuttingSolid)
        {
            try
            {
                return SolidSolidCutUtils.GetSolidsBeingCut(cuttingSolid);
            }
            catch
            {
                return null;
            }
        }

        public static IList<Element> ExtGetElementsBeingCut(this Document dbDocument, Element cuttingSolid)
        {
            ICollection<ElementId> ids = dbDocument.ExtGetSolidsBeingCut(cuttingSolid);
            if (ids == null || !ids.Any()) return null;
            return dbDocument.ExtGetElements(ids);

        }

        public static string ExtGetPathName(this Document dbDocument)
        {
            return dbDocument.PathName;
        }

        public static string ExtGetFolderName(this Document dbDocument)
        {
            return Path.GetDirectoryName(dbDocument.PathName);
        }

        public static string ExtGetTitle(this Document dbDocument)
        {
            return dbDocument.Title;
        }

        public static bool ExtIsSameDocument(this Document dbDocument, Document documentToCompare)
        {
            return (dbDocument.ExtGetTitle() == documentToCompare.ExtGetTitle()
                    && dbDocument.ExtGetPathName() == documentToCompare.ExtGetPathName());
        }

        public static Dictionary<string, FamilyParameter> ExtGetParameterDictionary(this Document dbDocument)
        {
            var docFamilyManager = dbDocument.ExtGetFamilyManager();
            return FamilyManagerExtension.ExtGetParameterDictionary(docFamilyManager);
        }

        public static View ExtGetActiveView(this Document dbDocument)
        {
            return dbDocument.ActiveView;
        }

        public static Settings ExtGetSettings(this Document dbDocument)
        {
            return dbDocument.Settings;
        }

        public static Categories ExtGetCategories(this Document dbDocument)
        {
            return dbDocument.ExtGetSettings().ExtGetCategories();
        }

        public static Category ExtGetCategory(this Document dbDocument, BuiltInCategory builtInCategory)
        {
            return dbDocument.ExtGetCategories().ExtGetCategory(builtInCategory);
        }

        public static Category ExtNewSubCategory(this Document dbDocument, Category parentCategory, string name)
        {
            return dbDocument.ExtGetCategories().ExtNewSubCategory(parentCategory, name);
        }

        public static FilteredElementCollector ExtFilteredElementCollector(this Document dbDocument)
        {
            return new FilteredElementCollector(dbDocument);
        }

        public static IList<Element> ExtGetElementsOfCategory(this Document dbDocument,
            BuiltInCategory builtInCategory)
        {
            FilteredElementCollector collector = dbDocument.ExtFilteredElementCollector();
            ElementCategoryFilter filter = new ElementCategoryFilter(builtInCategory);
            return collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();
        }

        public static IEnumerable<Document> ExtGetLinkedDocuments(this Document dbDocument)
        {
            return dbDocument.GetLinkedDocuments();
        }

        private static IEnumerable<ExternalFileReference>
            GetLinkedFileReferences(this Document _document)
        {
            var collector = new FilteredElementCollector(
                _document);

            var linkedElements = collector
                .OfClass(typeof(RevitLinkType))
                .Select(x => x.GetExternalFileReference())
                .ToList();

            return linkedElements;
        }

        private static IEnumerable<Document>
            GetLinkedDocuments(this Document _document)
        {
            var linkedfiles = GetLinkedFileReferences(
                _document);

            var linkedFileNames = linkedfiles
                .Select(x => ModelPathUtils
                    .ConvertModelPathToUserVisiblePath(
                        x.GetAbsolutePath())).ToList();

            return _document.Application.Documents
                .Cast<Document>()
                .Where(doc => linkedFileNames.Any(
                    fileName => doc.PathName.Equals(fileName)));
        }

        public static string ExtConvertToStableRepresentation(this Document _document, Reference reference)
        {
            return reference.ConvertToStableRepresentation(_document);
        }

    }
}