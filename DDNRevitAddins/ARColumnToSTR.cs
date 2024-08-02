using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    public class ARColumnToSTR : IExternalCommand
    {
        // 常量定义
        private const string OutputDirectory = @"\Temp";
        private const string ColumnDataFileName = "column_data.csv";
        private const string LogFileName = "log.txt";
        private const string StructureColumnFamilyName = "Square Bars-Column";
        private const string ParameterWidth = "Width";
        private const string ParameterHeight = "Height";
        private const string ParameterBaseLevel = "Base Level";
        private const string ParameterTopLevel = "Top Level";
        private const string ParameterBaseOffset = "Base Offset";
        private const string ParameterTopOffset = "Top Offset";

        // Execute() // 14:22
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            // 获取当前文档
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // 创建输出目录
            string outputFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + OutputDirectory;
            EnsureDirectoryExists(outputFolderPath);

            try
            {
                // 开始事务
                using (Transaction trans = new Transaction(doc, "Convert Columns"))
                {
                    trans.Start();

                    // 获取所有COLUMN对象
                    List<Element> columns = GetAllColumns(doc);

                    // 获取所有不重复的COLUMN类型
                    List<FamilySymbol> columnTypes = GetUniqueColumnTypes(doc);

                    // 获取或创建相应的STRUCTURE COLUMN类型
                    List<FamilySymbol> structureColumnTypes = GetOrCreateStructureColumnTypes(doc, columnTypes);

                    // 插入结构柱
                    List<ColumnInfo> columnInfos = InsertStructureColumns(doc, columns, structureColumnTypes);

                    // 导出信息到CSV
                    ExportColumnDataToCSV(columnInfos, outputFolderPath);

                    trans.Commit(); // 提交事务
                    TaskDialog.Show("Export Structure Column Types", "Structure columns processed and exported to CSV file successfully.");
                }

                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = "Error in execution: " + ex.Message;
                LogError(ex.Message, outputFolderPath);
                return Result.Failed;
            }
        }

        // GetAllColumns() // 14:22
        private List<Element> GetAllColumns(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Columns)
                .WhereElementIsNotElementType();

            return collector.ToList();
        }

        // GetUniqueColumnTypes() // 13:31
        private List<FamilySymbol> GetUniqueColumnTypes(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Columns)
                .OfClass(typeof(FamilySymbol));

            return collector.Cast<FamilySymbol>().Distinct().ToList();
        }

        // GetOrCreateStructureColumnTypes() // 13:31
        private List<FamilySymbol> GetOrCreateStructureColumnTypes(Document doc, List<FamilySymbol> columnTypes)
        {
            List<FamilySymbol> structureTypes = new List<FamilySymbol>();
            Family family = GetStructureColumnFamily(doc, StructureColumnFamilyName);

            foreach (FamilySymbol columnType in columnTypes)
            {
                // 查找现有的STRUCTURE COLUMN Type
                FamilySymbol structureType = new FilteredElementCollector(doc)
                    .OfCategory(BuiltInCategory.OST_StructuralColumns)
                    .OfClass(typeof(FamilySymbol))
                    .Cast<FamilySymbol>()
                    .FirstOrDefault(t => t.Name == columnType.Name);

                if (structureType == null && family != null)
                {
                    // 如果不存在，创建新的STRUCTURE COLUMN Type
                    structureType = family.GetFamilySymbolIds()
                        .Select(id => doc.GetElement(id) as FamilySymbol)
                        .FirstOrDefault();

                    if (structureType != null)
                    {
                        // 复制参数
                        structureType = structureType.Duplicate(columnType.Name) as FamilySymbol;
                        structureType.LookupParameter(ParameterWidth).Set(columnType.LookupParameter(ParameterWidth).AsDouble());
                        structureType.LookupParameter(ParameterHeight).Set(columnType.LookupParameter("Depth").AsDouble());
                        structureType.Activate();
                        doc.Regenerate();
                    }
                }
                else if (structureType != null)
                {
                    // 确保参数一致
                    structureType.LookupParameter(ParameterWidth).Set(columnType.LookupParameter(ParameterWidth).AsDouble());
                    structureType.LookupParameter(ParameterHeight).Set(columnType.LookupParameter("Depth").AsDouble());
                }

                if (structureType != null)
                {
                    structureTypes.Add(structureType);
                }
            }

            return structureTypes;
        }

        // GetStructureColumnFamily() // 13:36
        private Family GetStructureColumnFamily(Document doc, string familyName)
        {
            Family family = new FilteredElementCollector(doc)
                .OfClass(typeof(Family))
                .Cast<Family>()
                .FirstOrDefault(f => f.Name.Equals(familyName));

            if (family == null)
            {
                TaskDialog.Show("Missing Family", $"The structure column family '{familyName}' is not loaded. Please load it to continue.");
                throw new FileNotFoundException($"Family '{familyName}' not found in the document.");
            }
            return family;
        }

        // InsertStructureColumns() // 13:45
        private List<ColumnInfo> InsertStructureColumns(Document doc, List<Element> columns, List<FamilySymbol> structureColumnTypes)
        {
            List<ColumnInfo> columnInfos = new List<ColumnInfo>();

            foreach (Element column in columns)
            {
                LocationPoint loc = column.Location as LocationPoint;
                if (loc == null) continue;

                XYZ point = loc.Point;
                FamilySymbol colType = column.Document.GetElement(column.GetTypeId()) as FamilySymbol;
                FamilySymbol strType = structureColumnTypes.FirstOrDefault(t => t.Name == colType.Name);

                if (strType == null) continue;

                Level baseLevel = doc.GetElement(column.LookupParameter(ParameterBaseLevel).AsElementId()) as Level;
                Level topLevel = doc.GetElement(column.LookupParameter(ParameterTopLevel).AsElementId()) as Level;
                double baseOffset = column.LookupParameter(ParameterBaseOffset).AsDouble();
                double topOffset = column.LookupParameter(ParameterTopOffset).AsDouble();

                double angle = loc.Rotation;
                // 插入结构柱
                FamilyInstance newColumn = doc.Create.NewFamilyInstance(point, strType, baseLevel, StructuralType.Column);
                newColumn.LookupParameter(ParameterBaseOffset).Set(baseOffset);
                newColumn.LookupParameter(ParameterTopOffset).Set(topOffset);
                newColumn.LookupParameter(ParameterTopLevel).Set(topLevel.Id);

                // 使用ElementTransformUtils旋转元素
                Line axis = Line.CreateBound(point, point + XYZ.BasisZ);
                ElementTransformUtils.RotateElement(doc, newColumn.Id, axis, angle);

                ColumnInfo info = new ColumnInfo
                {
                    ColumnPoint = point,
                    ColumnTypeName = colType.Name,
                    ColumnAngle = angle,
                    ColumnBaseLevel = baseLevel.Name,
                    ColumnBaseOffset = baseOffset,
                    ColumnTopLevel = topLevel.Name,
                    ColumnTopOffset = topOffset,
                    StructTypeName = strType.Name,
                    StructAngle = angle,
                    StructBaseLevel = baseLevel.Name,
                    StructBaseOffset = baseOffset,
                    StructTopLevel = topLevel.Name,
                    StructTopOffset = topOffset
                };

                columnInfos.Add(info);
            }

            return columnInfos;
        }

        // ExportColumnDataToCSV() // 14:22
        private void ExportColumnDataToCSV(List<ColumnInfo> columnInfos, string outputFolderPath)
        {
            string csvPath = Path.Combine(outputFolderPath, ColumnDataFileName);
            List<string> lines = new List<string>
            {
                "X,Y,Z,COLUMN TYPE,COLUMN Angle,COLUMN Base Level,COLUMN Base Offset,COLUMN Top Level,COLUMN Top Offset,STRUCT COLUMN TYPE,STRUCT Angle,STRUCT Base Level,STRUCT Base Offset,STRUCT Top Level,STRUCT Top Offset"
            };

            foreach (var info in columnInfos)
            {
                lines.Add($"{info.ColumnPoint.X},{info.ColumnPoint.Y},{info.ColumnPoint.Z},{info.ColumnTypeName},{info.ColumnAngle},{info.ColumnBaseLevel},{info.ColumnBaseOffset},{info.ColumnTopLevel},{info.ColumnTopOffset},{info.StructTypeName},{info.StructAngle},{info.StructBaseLevel},{info.StructBaseOffset},{info.StructTopLevel},{info.StructTopOffset}");
            }

            File.WriteAllLines(csvPath, lines);
        }

        // LogError() // 14:22
        private void LogError(string error, string outputFolderPath)
        {
            string logPath = Path.Combine(outputFolderPath, LogFileName);
            File.AppendAllText(logPath, error + System.Environment.NewLine);
        }

        // EnsureDirectoryExists() // 14:22
        private void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }

    public class ColumnInfo
    {
        public XYZ ColumnPoint { get; set; }
        public string ColumnTypeName { get; set; }
        public double ColumnAngle { get; set; }
        public string ColumnBaseLevel { get; set; }
        public double ColumnBaseOffset { get; set; }
        public string ColumnTopLevel { get; set; }
        public double ColumnTopOffset { get; set; }
        public string StructTypeName { get; set; }
        public double StructAngle { get; set; }
        public string StructBaseLevel { get; set; }
        public double StructBaseOffset { get; set; }
        public string StructTopLevel { get; set; }
        public double StructTopOffset { get; set; }
    }
}
