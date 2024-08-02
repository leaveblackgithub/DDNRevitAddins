using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.Archive.Elements;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.Archive.Geometries;
using DDNRevitAddins.General;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class ReloadImage: ExternalCommandProjectBase
    {
        private const string StatusPromptToPickImage = "Pick the image to reload";
        private const string ParameterNameOfLoadedFromFile = "Loaded from file";
        private const string ParameterNameOfWidth = "Width";
        private Element _imageToReload;
        private Element _newImage;
        private XYZ _minOld;
        private XYZ _minNew;
        private XYZ _centerPointOfImage;
        private XYZ _lowerLeftPoint;

        protected override bool RunBeforeTransaction()
        {
            if (!GetImageToReload()) return false;
            if (!GetLowerLeftPoint()) return false;
            AddSingleTransactionHandler((SingleTransactionHandler)ReloadImageImpl);
            return true;
        }

        protected void ReloadImageImpl()
        {
            if (!ImportImage()) throw new ExceptionToCancel("Can't Load new image");
            AdjustWidth();
            CheckDirection();
            DeleteOldImage();
        }

        private void CheckDirection()
        {
            BoundingBoxXYZ boundingBoxXyz = _newImage.ExtBoundingBox();
            _minNew = boundingBoxXyz.ExtMinTransform();
            if (!RevitDbApplication.ExtAlmostEqualTo(_minOld, _minNew)) AdjustDirection();
            DebugExtension.PrintFieldValue(nameof(CheckDirection),nameof(_minNew),_minNew.ToString());
        }

        private void AdjustDirection()
        {
            XYZ directionOld = _minOld.ExtSubtract(GetCenterPointOfImage());
            DebugExtension.PrintFieldValue(nameof(AdjustDirection), nameof(directionOld), directionOld.ToString());
            XYZ directionNew = _minNew.ExtSubtract(GetCenterPointOfImage());
            DebugExtension.PrintFieldValue(nameof(AdjustDirection), nameof(directionNew), directionNew.ToString());
            double angle = directionNew.ExtAngleOnXyPlaneTo(directionOld);
            DebugExtension.PrintFieldValue(nameof(AdjustDirection), nameof(angle), angle);
            //_newImage.ExtRotateOnXyPlane(GetCenterPointOfImage(), angle);
        }
        

        private void DeleteOldImage()
        {
            _imageToReload.ExtDelete();
        }

        private void AdjustWidth()
        {
            _newImage.ExtSetParameterValueDouble(ParameterNameOfWidth, GetOldWidth());
        }
        private double  GetOldWidth()
        {
            double oldWidth = _imageToReload.ExtGetParameterValueDouble(ParameterNameOfWidth);
            DebugExtension.PrintFieldValue(nameof(GetOldWidth), nameof(oldWidth), oldWidth);
            return  oldWidth;
        }
        private bool ImportImage()
        {
            string newPath = GetNewPath();
            if (newPath == "") return false;
            _newImage=RevitDbDocument.ExtImportImage(newPath, GetCenterPointOfImage(), GetOwnerView());
            DebugExtension.PrintFieldValue(nameof(ImportImage), nameof(_newImage), _newImage.ExtIdValue());
            return true;
        }

        private View GetOwnerView()
        {
            View ownerView = _imageToReload.ExtOwnerView();
            //RevitCreateDocument.ExtShowViewBoundingbox(ownerView);
            DebugExtension.PrintFieldValue(nameof(GetOwnerView), nameof(ViewExtension.ExtCropBox), ownerView.ExtCropBox().ExtAngle());
            DebugExtension.PrintFieldValue(nameof(GetOwnerView), nameof(ownerView), ownerView.ExtIdValue());
            return ownerView;
        }

        private string GetNewPath()
        {
            string newPath = FileOpen.OpenFile("Choose image file to reload from", FileOpen.ImageFileFilter, GetOldPath());
            DebugExtension.PrintFieldValue(nameof(GetNewPath), nameof(newPath), newPath);
            return newPath;
        }

        private string GetOldPath()
        {
            string oldPath = _imageToReload.ExtGetTypeParameterValueString(ParameterNameOfLoadedFromFile);
            DebugExtension.PrintFieldValue(nameof(GetOldPath), nameof(oldPath), oldPath);
            return oldPath;
        }
        private XYZ GetCenterPointOfImage()
        {
            if (_centerPointOfImage != null) return _centerPointOfImage;
            BoundingBoxXYZ boundingBoxXyz = _imageToReload.ExtBoundingBox();
            DebugExtension.PrintFieldValue(nameof(GetCenterPointOfImage), nameof(BoundingBoxXyzExtension.ExtAngle), boundingBoxXyz.ExtAngle());
            _minOld = boundingBoxXyz.ExtMinTransform();
            DebugExtension.PrintFieldValue(nameof(GetCenterPointOfImage), nameof(_minOld), _minOld.ToString());
            _centerPointOfImage = boundingBoxXyz?.ExtCenter() ?? throw new ArgumentNullException();
            DebugExtension.PrintFieldValue(nameof(GetCenterPointOfImage), nameof(_centerPointOfImage), _centerPointOfImage.ToString());
            return _centerPointOfImage;
        }

        private bool GetLowerLeftPoint()
        {
            _lowerLeftPoint = RevitSelection.ExtPickPoint(ObjectSnapTypeExtension.ExtEndPoints(), "Select the lowerleft point");
            if (_lowerLeftPoint == null) return false;
            return true;
        }

        private bool GetImageToReload()
        {
            _imageToReload = PickElementOfCategory(ElementExtension.CategoryNameOfRasterImages,
                StatusPromptToPickImage);
            if (_imageToReload == null) return false;
            DebugExtension.PrintFieldValue(nameof(GetImageToReload), nameof(_imageToReload), _imageToReload.ExtIdValue());
            return true;
        }
    }
}
