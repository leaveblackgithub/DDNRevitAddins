using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    public class ReplaceRoomSeparationWithWall : IExternalCommand
    {

        private const double UnconnectedWallHeight = 6.0; // 墙体的未连接高度
        #region External Command Main - V10.3
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
                // 获取墙体对象
                Wall selectedWall = GetOrPromptWall(uiDoc);
                if (selectedWall == null)
                {
                    return Result.Cancelled;
                }

                // 获取房间分隔线
                IList<Element> roomSeparationLines = PromptUserToSelectRoomSeparationLines(uiDoc);
                if (roomSeparationLines == null || roomSeparationLines.Count == 0)
                {
                    return Result.Cancelled;
                }

                // 创建墙体并调整位置
                IList<Element> walls = CreateWallsFromRoomSeparationLines(doc, roomSeparationLines, selectedWall.WallType);

                // 删除房间分隔线
                DeleteRoomSeparationLines(doc, roomSeparationLines);

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
        #region Delete Room Separation Lines - V1.0
        private void DeleteRoomSeparationLines(Document doc, IList<Element> roomSeparationLines)
        {
            using (Transaction trans = new Transaction(doc, "Delete Room Separation Lines"))
            {
                trans.Start();

                foreach (Element line in roomSeparationLines)
                {
                    doc.Delete(line.Id);
                }

                trans.Commit();
            }
        }
        #endregion



        #region Create Walls From Room Separation Lines - V1.4
        private IList<Element> CreateWallsFromRoomSeparationLines(Document doc, IList<Element> roomSeparationLines, WallType wallType)
        {
            List<Element> createdWalls = new List<Element>();

            using (Transaction trans = new Transaction(doc, "Create Walls from Room Separation Lines"))
            {
                // 设置失败处理选项
                FailureHandlingOptions failureHandlingOptions = trans.GetFailureHandlingOptions();
                failureHandlingOptions.SetFailuresPreprocessor(new WarningSwallower());
                trans.SetFailureHandlingOptions(failureHandlingOptions);

                trans.Start();

                foreach (Element separationLine in roomSeparationLines)
                {
                    LocationCurve locCurve = separationLine.Location as LocationCurve;
                    if (locCurve != null)
                    {
                        Curve curve = locCurve.Curve;
                        Wall newWall = Wall.Create(doc, curve, wallType.Id, doc.ActiveView.GenLevel.Id, UnconnectedWallHeight, 0.0, false, false);
                        //确保roombounding是选中的。
                        Parameter roomBoundingParam = newWall.get_Parameter(BuiltInParameter.WALL_ATTR_ROOM_BOUNDING);

                        if (roomBoundingParam != null && !roomBoundingParam.IsReadOnly)
                        {
                            // 设置Room Bounding为true
                            roomBoundingParam.Set(1);
                        }
                        createdWalls.Add(newWall);
                    }
                }

                trans.Commit();
            }

            return createdWalls;
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




        #region Get Or Prompt Wall - V2.0
        private Wall GetOrPromptWall(UIDocument uiDoc)
        {
            // 尝试从当前选择集中获取墙体对象
            Wall wall = GetWallFromSelection(uiDoc);
            if (wall != null)
            {
                return wall;
            }

            // 提示用户选择墙体对象
            return PromptUserToSelectWall(uiDoc);
        }
        #endregion


        #region Get Wall From Selection - V2.0
        private Wall GetWallFromSelection(UIDocument uiDoc)
        {
            ICollection<ElementId> selectedIds = uiDoc.Selection.GetElementIds();

            // 如果选择集包含多个元素，清空选择集并提示用户重新选择
            if (selectedIds.Count != 1)
            {
                uiDoc.Selection.SetElementIds(new List<ElementId>());
                return null;
            }

            // 检查选择集中的单个元素是否为墙体
            Element element = uiDoc.Document.GetElement(selectedIds.First());
            if (element is Wall wall)
            {
                return wall;
            }

            return null;
        }
        #endregion



        #region Prompt User To Select Wall - V2.0
        private Wall PromptUserToSelectWall(UIDocument uiDoc)
        {
            try
            {
                // 使用选择提示进行墙体对象选择
                Reference refWall = uiDoc.Selection.PickObject(ObjectType.Element, new WallElementSelectionFilter(), "请选择一个墙体对象");
                Element element = uiDoc.Document.GetElement(refWall);

                if (element is Wall selectedWall)
                {
                    return selectedWall;
                }
            }
            catch (OperationCanceledException)
            {
                // 用户按下ESC取消选择
            }

            return null;
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

        #region Log Error - V2.0
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

        #region Prompt User To Select Room Separation Lines - V1.0
        private IList<Element> PromptUserToSelectRoomSeparationLines(UIDocument uiDoc)
        {
            IList<Element> roomSeparationLines = new List<Element>();

            try
            {
                // 提示用户选择房间分隔线
                IList<Reference> roomSeparationRefs = uiDoc.Selection.PickObjects(ObjectType.Element, new RoomSeparationLineSelectionFilter(), "请选择房间分隔线");
                foreach (Reference refLine in roomSeparationRefs)
                {
                    Element element = uiDoc.Document.GetElement(refLine);
                    if (element != null)
                    {
                        roomSeparationLines.Add(element);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // 用户按下ESC取消选择
                return null;
            }
            return roomSeparationLines;
        }
        #endregion


        #region Room Separation Line Selection Filter - V1.0
        public class RoomSeparationLineSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                return elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_RoomSeparationLines; // 检查是否为房间分隔线
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false;
            }
        }
        #endregion

        

    }
}