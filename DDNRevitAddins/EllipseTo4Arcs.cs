using System;
using System.Collections.Generic;
using System.IO;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    public class EllipseTo4Arc : IExternalCommand
    {
        private const string Version = "1.0.0"; // 版本编号

        // 常量定义
        private static readonly string LogFilePath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "temp",
                "EllipseTo4ArcLog.txt"); // 日志文件路径

        #region 主函数

        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements)
        {
            var uiApp = commandData.Application;
            var uiDoc = uiApp.ActiveUIDocument;
            var doc = uiDoc.Document;

            try
            {
                // 选择椭圆弧
                var ellipseCurve = SelectEllipseCurve(uiDoc);
                if (ellipseCurve == null) return Result.Cancelled;

                // 处理椭圆弧
                HandleEllipseCurve(doc, ellipseCurve);

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                LogError(ex);
                message = "执行过程中发生错误，请检查日志。";
                return Result.Failed;
            }
        }

        #endregion

        #region 选择椭圆弧函数 v1.0.2

        private CurveElement SelectEllipseCurve(UIDocument uiDoc)
        {
            try
            {
                // 使用自定义选择过滤器
                var filter = new EllipseCurveSelectionFilter();
                var pickedRef = uiDoc.Selection.PickObject(ObjectType.Element, filter, "请选择一个椭圆弧形墙或线。");

                if (pickedRef != null)
                {
                    var element = uiDoc.Document.GetElement(pickedRef);
                    var curveElement = element as CurveElement;
                    if (curveElement != null && curveElement.GeometryCurve is Ellipse)
                        return curveElement;
                    TaskDialog.Show("错误", "所选元素不是椭圆弧。");
                }
            }
            catch (OperationCanceledException)
            {
                // 用户取消选择
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

        #endregion


        #region 处理椭圆弧函数 v1.0.7

        private void HandleEllipseCurve(Document doc, CurveElement ellipseCurve)
        {
            try
            {
                var ellipse = ellipseCurve.GeometryCurve as Ellipse;
                if (ellipse == null)
                {
                    TaskDialog.Show("错误", "所选元素不是有效的椭圆弧。");
                    return;
                }

                // 分解椭圆弧为四段弧和辅助线
                var curves = DecomposeEllipseToArcs(ellipseCurve, doc);

                // 创建模型线以测试输出
                using (var trans = new Transaction(doc, "Create Model Lines"))
                {
                    trans.Start();

                    var plane = Plane.CreateByNormalAndOrigin(ellipse.Normal, ellipse.Center);
                    var sketchPlane = SketchPlane.Create(doc, plane);

                    foreach (var curve in curves) doc.Create.NewModelCurve(curve, sketchPlane);

                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion

        #region 创建椭圆弧函数 v2.0.2

        private Curve CreateEllipseCurve(EllipseInfo ellipseInfo)
        {
            // 获取椭圆信息
            var center = ellipseInfo.Center;
            var radiusLong = ellipseInfo.RadiusLong;
            var radiusShort = ellipseInfo.RadiusShort;
            var xDirection = ellipseInfo.AxisLongDirection1.Normalize();
            var yDirection = ellipseInfo.AxisShortDirection1.Normalize();
            var startParameter = ellipseInfo.StartParameter;
            var endParameter = ellipseInfo.EndParameter;

            // 创建椭圆弧
            var ellipseArc = Ellipse.CreateCurve(center, radiusLong, radiusShort, xDirection, yDirection,
                startParameter, endParameter);

            return ellipseArc;
        }

        #endregion


        #region 获取椭圆信息函数 v2.0.0

        private EllipseInfo GetEllipseInfo(CurveElement ellipseCurve)
        {
            var ellipse = ellipseCurve.GeometryCurve as Ellipse;
            if (ellipse == null) throw new ArgumentException("所提供的曲线不是椭圆。");

            var ellipseInfo = new EllipseInfo
            {
                Center = ellipse.Center,
                RadiusShort = ellipse.RadiusY,
                RadiusLong = ellipse.RadiusX,
                AxisShortDirection1 = ellipse.YDirection,
                AxisLongDirection1 = ellipse.XDirection,
                StartParameter = ellipse.GetEndParameter(0),
                EndParameter = ellipse.GetEndParameter(1),
                AxisShort = Line.CreateBound(ellipse.Center - ellipse.YDirection * ellipse.RadiusY,
                    ellipse.Center + ellipse.YDirection * ellipse.RadiusY),
                AxisLong = Line.CreateBound(ellipse.Center - ellipse.XDirection * ellipse.RadiusX,
                    ellipse.Center + ellipse.XDirection * ellipse.RadiusX),
                OriginalEllipse = ellipse,
                FullEllipse = (Ellipse)Ellipse.CreateCurve(ellipse.Center, ellipse.RadiusX, ellipse.RadiusY,
                    ellipse.XDirection, ellipse.YDirection, 0, 2 * Math.PI)
            };
            //比较AxisShort和AxisLong的长度，如果AxisShort的长度大于AxisLong的长度，交换两者的值以及两者的方向
            if (ellipseInfo.AxisShort.Length > ellipseInfo.AxisLong.Length)
            {
                var tempDirection = ellipseInfo.AxisShortDirection1;
                ellipseInfo.AxisShortDirection1 = ellipseInfo.AxisLongDirection1;
                ellipseInfo.AxisLongDirection1 = tempDirection;
                var tempCurve = ellipseInfo.AxisShort;
                ellipseInfo.AxisShort = ellipseInfo.AxisLong;
                ellipseInfo.AxisLong = tempCurve;
                var tempRadius = ellipseInfo.RadiusShort;
                ellipseInfo.RadiusShort = ellipseInfo.RadiusLong;
                ellipseInfo.RadiusLong = tempRadius;
            }

            //get the end points of axises
            ellipseInfo.EndPointShort1 = ellipseInfo.AxisShort.GetEndPoint(0);
            ellipseInfo.EndPointShort2 = ellipseInfo.AxisShort.GetEndPoint(1);
            ellipseInfo.EndPointLong1 = ellipseInfo.AxisLong.GetEndPoint(0);
            ellipseInfo.EndPointLong2 = ellipseInfo.AxisLong.GetEndPoint(1);

            //将CENTER往短轴方向和反向移动两个长轴的距离，得到两个点，连成直线AxisShortExtend

            ellipseInfo.AxisShortExtend = Line.CreateBound(ellipse.Center
                                                           - ellipseInfo.AxisShortDirection1
                                                           * ellipseInfo.RadiusLong * 2,
                ellipse.Center
                + ellipseInfo.AxisShortDirection1
                * ellipseInfo.RadiusLong * 2);
            return ellipseInfo;
        }

        #endregion

        #region 椭圆弧选择过滤器 v1.0.1

        public class EllipseCurveSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element elem)
            {
                // 检查元素是否为 CurveElement 且其几何曲线是椭圆
                var curveElement = elem as CurveElement;
                return curveElement != null && curveElement.GeometryCurve is Ellipse;
            }

            public bool AllowReference(Reference reference, XYZ position)
            {
                return false; // 不允许参考选择
            }
        }

        #endregion


        #region 分解椭圆弧为辅助线函数 v2.0.1

        private List<Curve> DecomposeEllipseToArcs(CurveElement ellipseCurve, Document doc)
        {
            var curves = new List<Curve>();
            var pts = new List<XYZ>();
            var ellipseInfo = GetEllipseInfo(ellipseCurve);
            // curves.Add(ellipseInfo.AxisLong);
            //
            // curves.Add(ellipseInfo.AxisShortExtend);
            // curves.Add(ellipseInfo.FullEllipse);
            // curves.Add(CreateEllipseCurve(ellipseInfo));
            var pointsCurvesF = GetPointsF(ellipseInfo);
            // curves.AddRange(pointsCurvesF.Curves);
            var pointsCurveO1O2 = GetPointsO1O2(ellipseInfo, pointsCurvesF);
            // curves.AddRange(pointsCurveO1O2.Curves);
            var pointsCurvesO3O4 = GetPointsO3O4(ellipseInfo, pointsCurveO1O2);
            // curves.AddRange(pointsCurvesO3O4.Curves);
            var pointsCurvesArcEndPoints = GetPointsArcEndPoints(ellipseInfo, pointsCurveO1O2, pointsCurvesO3O4);
            curves.AddRange(pointsCurvesArcEndPoints.Curves);
            return curves;
        }

        private PointsCurves GetPointsArcEndPoints(EllipseInfo ellipseInfo, PointsCurves pointsCurveO1O2,
            PointsCurves pointsCurvesO3O4)
        {
            var pointsCurves = new PointsCurves();
            //提取点O1,O2,O3,O4
            var pointO1 = pointsCurveO1O2.Points[0];
            var pointO2 = pointsCurveO1O2.Points[1];
            var pointO3 = pointsCurvesO3O4.Points[0];
            var pointO4 = pointsCurvesO3O4.Points[1];

            //提取LINE O1O2B,O3O4B
            var lineO1O2b = pointsCurveO1O2.Curves[2] as Line;
            var lineO3O4b = pointsCurvesO3O4.Curves[0] as Line;
            //计算O4到O1的方向
            var directionO4ToO1 = pointO1 - pointO4;
            var pointO4c = pointO1 + directionO4ToO1;
            var lineO1O4c = Line.CreateBound(pointO1, pointO4c);
            //计算O2到O3的方向
            var directionO2ToO3 = pointO3 - pointO2;
            var pointO2c = pointO3 + directionO2ToO3;
            var lineO3O2c = Line.CreateBound(pointO3, pointO2c);


            //计算这四条线和FullEllipse的交点
            var shortArc1End2 = GetIntersectionPoint(ellipseInfo.FullEllipse, lineO1O2b);
            var shortArc1End1 = GetIntersectionPoint(ellipseInfo.FullEllipse, lineO1O4c);
            var shortArc2End1 = GetIntersectionPoint(ellipseInfo.FullEllipse, lineO3O2c);
            var shortArc2End2 = GetIntersectionPoint(ellipseInfo.FullEllipse, lineO3O4b);
            //求这4个交点在Originalellipse上的参数,如果不存在则返回NULL


            //用stringbuilder分行显示这4个参数和ellipse的start和end参数，用taskdialog显示
            // var message = new System.Text.StringBuilder();
            // message.AppendLine("ShortArc1Para1: " + shortArc1Para1);
            // message.AppendLine("ShortArc1Para2: " + shortArc1Para2);
            // message.AppendLine("ShortArc2Para1: " + shortArc2Para1);
            // message.AppendLine("ShortArc2Para2: " + shortArc2Para2);
            // message.AppendLine("EllipseStartPara: " + ellipseInfo.StartParameter);
            // message.AppendLine("EllipseEndPara: " + ellipseInfo.EndParameter);
            // TaskDialog.Show("Parameters", message.ToString());
            //短圆弧半径等于shortArc1End1到pointO1的距离
            var radiusShortArc = shortArc1End1.DistanceTo(pointO1);
            //长圆弧半径等于shortArc1End2到pointO2的距离
            var radiusLongArc = shortArc1End2.DistanceTo(pointO2);
            var shortArcs1 = CreateArcsForEllipse(pointO1, radiusShortArc, shortArc1End1, shortArc1End2, ellipseInfo);
            var shortArcs2 = CreateArcsForEllipse(pointO3, radiusShortArc, shortArc2End1, shortArc2End2, ellipseInfo);
            var longArcs1 = CreateArcsForEllipse(pointO2, radiusLongArc, shortArc1End2, shortArc2End1, ellipseInfo);
            var longArcs2 = CreateArcsForEllipse(pointO4, radiusLongArc, shortArc2End2, shortArc1End1, ellipseInfo);
            //将4个圆弧加入返回值
            pointsCurves.Curves.AddRange(shortArcs1);
            pointsCurves.Curves.AddRange(shortArcs2);
            pointsCurves.Curves.AddRange(longArcs1);
            pointsCurves.Curves.AddRange(longArcs2);
            return pointsCurves;
        }

        private Arc CreateArcsFrCenterRadiusStartEnd(XYZ center, double radius, XYZ startPoint, XYZ endPoint)
        {
        var xDirection = (startPoint - center).Normalize();

        // 用 xDirection 逆时针转 90 度求 yDirection
        var yDirection = new XYZ(-xDirection.Y, xDirection.X, 0);
        var endAngle = CalculateAngleRelativeToAxis(endPoint, center, xDirection);
        var arc = Arc.Create(center, radius, 0, endAngle, xDirection, yDirection);
        return arc;
        }

    private List<Curve> CreateArcsForEllipse(XYZ center, double radius, XYZ startPoint, XYZ endPoint,
            EllipseInfo ellipseInfo)
        {

            // 定义一个最大距离为圆弧半径的 1/10
            var maxDistance = radius / 10;

            //获取startpoint和endpoint在originalellipse上的参数
            var pOnE1 = GetEllipseParameter(ellipseInfo.OriginalEllipse, startPoint);
            var pOnE2 = GetEllipseParameter(ellipseInfo.OriginalEllipse, endPoint);
            //获取originalellipse的起点参数和终点参数
            var eP1 = ellipseInfo.OriginalEllipse.GetEndParameter(0);
            var eP2 = ellipseInfo.OriginalEllipse.GetEndParameter(1);

            //如果两个参数都为空，或者pOnE1>=eP2，或者eP1>=pOnE2则返回空列表
            if (pOnE1 == null && pOnE2 == null)
                return new List<Curve>();
            if (pOnE1 > pOnE2)
            {
                var result=new List<Curve>();
                if(eP2!=null&&eP2 > pOnE1)
                    result.Add(CreateArcsFrCenterRadiusStartEnd(center, radius, startPoint, ellipseInfo.OriginalEllipse.GetEndPoint(1)));
                if(eP1!=null&&eP1 < pOnE2)
                    result.Add(CreateArcsFrCenterRadiusStartEnd(center, radius, ellipseInfo.OriginalEllipse.GetEndPoint(0), endPoint));
                return result;
            }
            //如果eP1在pOnE1和pOnE2之间，eP2为NULL或者大于pOnE2,以originalellipse的起点为起点，endPoint为终点，创建arc
            if (eP1 >= pOnE1 && eP1<=pOnE2 && (eP2 == null || eP2 > pOnE2))
            {
                var arc = CreateArcsFrCenterRadiusStartEnd(center, radius, ellipseInfo.OriginalEllipse.GetEndPoint(0), endPoint);
                return new List<Curve> { arc };
            }
            //如果eP2在pOnE1和pOnE2之间，eP1为NULL或者小于pOnE1,以originalellipse的终点为终点，startPoint为起点，创建arc
            if (eP2 >= pOnE1 && eP2<=pOnE2 && (eP1 == null || eP1 < pOnE1))
            {
                var arc = CreateArcsFrCenterRadiusStartEnd(center, radius, startPoint, ellipseInfo.OriginalEllipse.GetEndPoint(1));
                return new List<Curve> { arc };
            }

            //如果起点参数大于等于originalellipse的起点参数，且终点参数小于等于originalellipse的终点参数，则返回含arc的列表
            if (eP1 != null && eP2 != null)
            {
                if (pOnE1 >= eP1 && pOnE2 <= eP2)
                    return new List<Curve> { CreateArcsFrCenterRadiusStartEnd(center, radius, startPoint, endPoint) };
                //如果eP1>pOnE1且eP2<pOnE2，originalellipse的起点终点为起点终点，创建arc
                if (eP1 > pOnE1 && eP2 < pOnE2)
                {
                    var arc = CreateArcsFrCenterRadiusStartEnd(center, radius,
                        ellipseInfo.OriginalEllipse.GetEndPoint(0), ellipseInfo.OriginalEllipse.GetEndPoint(1));
                    return new List<Curve> { arc };
                }
            }
            //
            return new List<Curve>();

        }


    public double CalculateAngleRelativeToAxis(XYZ point, XYZ center, XYZ axis)
        {
            // Normalize the axis direction
            var normalizedAxis = axis.Normalize();

            // Calculate the vector from the center to the point
            var vector = point - center;

            // Project the vector onto the axis
            var projectionLength = vector.DotProduct(normalizedAxis);
            var projection = normalizedAxis * projectionLength;

            // Calculate the perpendicular component
            var perpendicularComponent = vector - projection;

            // Use Math.Atan2 to calculate the angle
            var angle = Math.Atan2(perpendicularComponent.GetLength(), projectionLength);

            return angle;
        }

        private double? GetEllipseParameter(Ellipse ellipse, XYZ point)
        {
            try
            {
                var result = ellipse.Project(point);
                return result?.Parameter;
            }
            catch
            {
                return null;
            }
        }

        private PointsCurves GetPointsO3O4(EllipseInfo ellipseInfo, PointsCurves pointsCurveO1O2)
        {
            var pointsCurves = new PointsCurves();
            //提取点O1,O2
            var pointO1 = pointsCurveO1O2.Points[0];
            var pointO2 = pointsCurveO1O2.Points[1];
            //计算O1到O的方向和距离
            var directionO1ToCenter = ellipseInfo.Center - pointO1;
            //计算O3
            var pointO3 = ellipseInfo.Center + directionO1ToCenter;
            //计算O4
            var directionO4ToCenter = ellipseInfo.Center - pointO2;
            var pointO4 = ellipseInfo.Center + directionO4ToCenter;
            //将O3,O4加入返回值
            pointsCurves.Points.Add(pointO3);
            pointsCurves.Points.Add(pointO4);
            //将O3O4连线加入返回值
            //求O4到O3的方向
            var directionO4ToO3 = pointO3 - pointO4;
            var pointO4b = pointO3 + directionO4ToO3;
            pointsCurves.Curves.Add(Line.CreateBound(pointO3, pointO4b));
            return pointsCurves;
        }

        private PointsCurves GetPointsO1O2(EllipseInfo ellipseInfo, PointsCurves pointsCurvesF)
        {
            //以pointsCurvesF的Curve[0]为line1,做line1的中垂线，延长与长短轴分别相交，把中垂线,短轴交点O1，长轴交点O2，O1O2连线都加入返回值
            var pointsCurves = new PointsCurves();
            var line1 = pointsCurvesF.Curves[0] as Line;
            //求LINE1的中点
            var midPointLine1 = (line1.GetEndPoint(0) + line1.GetEndPoint(1)) / 2;
            //求LINE1在XY平面的法线方向
            var normalLine1 = new XYZ(-line1.Direction.Y, line1.Direction.X, 0);
            //过中点做法线方向的线，线的两端到中点距离为长轴长度。
            var lineNormal = Line.CreateBound(midPointLine1 - normalLine1 * ellipseInfo.RadiusLong * 2,
                midPointLine1 + normalLine1 * ellipseInfo.RadiusLong * 2);
            //将法线加入返回值
            pointsCurves.Curves.Add(lineNormal);
            // 求长轴和法线交点 O1
            var intersectionO1 = GetIntersectionPoint(ellipseInfo.AxisLong as Line, lineNormal);
            if (intersectionO1 != null) pointsCurves.Points.Add(intersectionO1);

            // 求短轴延长线和法线交点 O2
            var intersectionO2 = GetIntersectionPoint(ellipseInfo.AxisShortExtend as Line, lineNormal);
            if (intersectionO2 != null) pointsCurves.Points.Add(intersectionO2);

            // 确保有两个交点 O1 和 O2
            if (pointsCurves.Points.Count >= 2)
            {
                pointsCurves.Curves.Add(Line.CreateBound(pointsCurves.Points[0], pointsCurves.Points[1]));
                //求O2到O1的方向
                var directionO2ToO1 = pointsCurves.Points[0] - pointsCurves.Points[1];
                var pointO2b = pointsCurves.Points[0] + directionO2ToO1;
                pointsCurves.Points.Add(pointO2b);
                pointsCurves.Curves.Add(Line.CreateBound(pointsCurves.Points[0], pointsCurves.Points[2]));
            }
            else
            {
                throw new InvalidOperationException("无法找到足够的交点来创建 O1O2 线。");
            }

            return pointsCurves;
        }

        #region 求两曲线交点函数 v1.1.0

        private XYZ GetIntersectionPoint(Curve curve1, Curve curve2)
        {
            IntersectionResultArray results;
            var result = curve1.Intersect(curve2, out results);

            if (result == SetComparisonResult.Overlap && results != null && results.Size > 0)
                return results.get_Item(0).XYZPoint;
            return null;
        }

        #endregion


        private PointsCurves GetPointsF(EllipseInfo ellipseInfo)
        {
            var pointsCurves = new PointsCurves();
            var directionShort1ToLong1 = ellipseInfo.EndPointLong1 - ellipseInfo.EndPointShort1;
            //F点为短轴端点1网长轴端点1移动长短轴半径差的距离
            var pointF = ellipseInfo.EndPointShort1 +
                         directionShort1ToLong1.Normalize() * (ellipseInfo.RadiusLong - ellipseInfo.RadiusShort);
            //连线长轴端点和F点
            var lineLong1ToF = Line.CreateBound(ellipseInfo.EndPointLong1, pointF);
            //把点和线加入返回值
            pointsCurves.Points.Add(pointF);
            pointsCurves.Curves.Add(lineLong1ToF);
            return pointsCurves;
        }

        #endregion


        #region 日志记录函数

        private void LogError(Exception ex)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(LogFilePath));
                using (var writer = new StreamWriter(LogFilePath, true))
                {
                    writer.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {ex.Message}");
                    writer.WriteLine(ex.StackTrace);
                }
            }
            catch
            {
                // 如果日志记录失败，可以选择忽略或提示用户
            }
        }

        private bool IsPointOnPlane(XYZ point, Plane plane)
        {
            // Calculate the distance from the point to the plane
            var distance = plane.Normal.DotProduct(point - plane.Origin);
            // Allow a small tolerance for floating-point inaccuracies
            return Math.Abs(distance) < 1e-6;
        }

        #endregion
    }

    public class EllipseInfo
    {
        public XYZ Center { get; set; }
        public double RadiusShort { get; set; }
        public double RadiusLong { get; set; }
        public XYZ AxisShortDirection1 { get; set; }
        public XYZ AxisLongDirection1 { get; set; }
        public Curve AxisShort { get; set; }

        public Curve AxisShortExtend { get; set; }
        public Curve AxisLong { get; set; }
        public double StartParameter { get; set; }
        public double EndParameter { get; set; }

        public XYZ EndPointShort1 { get; set; }

        public XYZ EndPointShort2 { get; set; }
        public XYZ EndPointLong1 { get; set; }
        public XYZ EndPointLong2 { get; set; }

        public Ellipse FullEllipse { get; set; }

        public Ellipse OriginalEllipse { get; set; }
    }


    public class PointsCurves
    {
        public PointsCurves()
        {
            Points = new List<XYZ>();
            Curves = new List<Curve>();
        }

        public List<XYZ> Points { get; set; }
        public List<Curve> Curves { get; set; }
    }
}