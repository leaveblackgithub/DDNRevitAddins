using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    public class ReplaceWallWithRoomSeparation : IExternalCommand
    {
        #region External Command Main - V1.1
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            try
            {
                // 获取要替换的墙体列表
                IList<Wall> selectedWalls = PromptUserToSelectWalls(uiDoc);
                if (selectedWalls == null || selectedWalls.Count == 0)
                {
                    return Result.Cancelled;
                }

                // 创建房间分隔线并删除墙体
                foreach (Wall wall in selectedWalls)
                {
                    CreateRoomSeparationLinesFromWall(doc, wall);
                    DeleteWall(doc, wall);
                }

                return Result.Succeeded;
            }
            catch (OperationCanceledException)
            {
                // 用户按下ESC，退出命令
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                LogError(ex);
                return Result.Failed;
            }
        }
        #endregion

        #region Prompt User To Select Walls - V1.0
        private IList<Wall> PromptUserToSelectWalls(UIDocument uiDoc)
        {
            IList<Wall> selectedWalls = new List<Wall>();

            try
            {
                // 提示用户选择多个墙体
                IList<Reference> wallRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new WallElementSelectionFilter(), "请选择一个或多个墙体对象");
                foreach (Reference refWall in wallRefs)
                {
                    Element element = uiDoc.Document.GetElement(refWall);
                    if (element is Wall selectedWall)
                    {
                        selectedWalls.Add(selectedWall);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 用户按下ESC取消选择
                return null;
            }

            return selectedWalls;
        }
        #endregion

        #region Create Room Separation Lines From Wall - V1.1
        private void CreateRoomSeparationLinesFromWall(Document doc, Wall wall)
        {
            using (Transaction trans = new Transaction(doc, "Create Room Separation Lines from Wall"))
            {
                // 设置失败处理选项
                FailureHandlingOptions failureHandlingOptions = trans.GetFailureHandlingOptions();
                failureHandlingOptions.SetFailuresPreprocessor(new WarningSwallower());
                trans.SetFailureHandlingOptions(failureHandlingOptions);

                trans.Start();

                // 获取墙体的位置曲线
                LocationCurve locCurve = wall.Location as LocationCurve;
                if (locCurve != null)
                {
                    Curve curve = locCurve.Curve;

                    // 将曲线添加到 CurveArray
                    CurveArray curveArray = new CurveArray();
                    curveArray.Append(curve);

                    // 创建房间分隔线
                    doc.Create.NewRoomBoundaryLines(doc.ActiveView.SketchPlane, curveArray, doc.ActiveView);
                }

                trans.Commit();
            }
        }
        #endregion

        #region Delete Wall - V1.1
        private void DeleteWall(Document doc, Wall wall)
        {
            using (Transaction trans = new Transaction(doc, "Delete Wall"))
            {
                // 设置失败处理选项
                FailureHandlingOptions failureHandlingOptions = trans.GetFailureHandlingOptions();
                failureHandlingOptions.SetFailuresPreprocessor(new WarningSwallower());
                trans.SetFailureHandlingOptions(failureHandlingOptions);

                trans.Start();
                doc.Delete(wall.Id);
                trans.Commit();
            }
        }
        #endregion

        #region Log Error - V1.0
        private void LogError(Exception ex)
        {
            string userFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string logFolderPath = Path.Combine(userFolderPath, "Documents", "temp");
            string logFilePath = Path.Combine(logFolderPath, "RevitPluginLog.txt");

            try
            {
                // 检查并创建日志文件夹
                if (!Directory.Exists(logFolderPath))
                {
                    Directory.CreateDirectory(logFolderPath);
                }

                // 检查并创建日志文件
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath).Close(); // 使用Close()释放文件句柄
                }

                // 写入错误日志
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {ex.Message}\n{ex.StackTrace}\n");
            }
            catch (Exception logEx)
            {
                TaskDialog.Show("日志错误", $"无法写入日志文件: {logEx.Message}");
            }
        }
        #endregion

        #region Wall Element Selection Filter - V1.0
        public class WallElementSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem is Wall; // 检查是否为墙
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
        #endregion

        #region Warning Swallower - V1.0
        public class WarningSwallower : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                IList<FailureMessageAccessor> failureMessages = failuresAccessor.GetFailureMessages();

                foreach (FailureMessageAccessor failureMessage in failureMessages)
                {
                    // 忽略所有警告
                    if (failureMessage.GetSeverity() == FailureSeverity.Warning)
                    {
                        failuresAccessor.DeleteWarning(failureMessage);
                    }
                }

                return FailureProcessingResult.Continue;
            }
        }
        #endregion
    }
}
