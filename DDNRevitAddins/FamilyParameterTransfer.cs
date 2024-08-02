using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using DDNGeneralLibrary;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Environments;
using DDNRevitAddins.General;

namespace DDNRevitAddins
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class FamilyParameterTransfer: FamilyParameterBase
    {
        protected override bool RunBeforeTransaction()
        {
            DefinitionFile defFile = RevitDbApplication.OpenSharedParameterFile();
            if(defFile==null)throw new ExceptionToCancel("Can't find share parameter file.");
            DefinitionGroups defGroups = defFile.Groups;
            ExternalDefinitions = new Dictionary<string, ExternalDefinition>();
            foreach (DefinitionGroup group in defGroups)
            {
                foreach (Definition def in group.Definitions)
                {
                    ExternalDefinition edef = def as ExternalDefinition;
                    ExternalDefinitions.Add(edef.Name, edef);
                }
            }
            AddSingleTransactionHandler((SingleTransactionHandler)TransferParameters);
            return true;
        }

    }
    /*  
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class ParameterInstanceToType : IExternalCommand
  {
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
          try
          {
              UIApplication uiapp = commandData.Application;
              UIDocument uidoc = uiapp.ActiveUIDocument;
              Document doc = uidoc.Document;

              FamilyManager fM = doc.FamilyManager;

              FrmParameters frmParameters = new FrmParameters(uiapp);
              if (frmParameters.ShowDialog() != DialogResult.OK) return Result.Cancelled;

              Document sourceDoc = frmParameters.SelectedDoc.Value;
              FamilyManager sfM = sourceDoc.FamilyManager;

              if (fM.Types.Size == 0)
              {
                  throw new Exception("Current Family must have type");
              }
              if (sfM.Types.Size == 0)
              {
                  throw new Exception("Source Family must have type");
              }

              List<FamilyParameter> sourceParameters = frmParameters.SelectedParameters.Values.ToList<FamilyParameter>();

              Transaction transaction = new Transaction(uidoc.Document);
              transaction.Start("Instance Parameter Become Type Parameter");
              List<FamilyParameter> added = new List<FamilyParameter>();
              FamilyType type = fM.CurrentType;
              foreach (FamilyParameter p in sourceParameters)
              {
                  FamilyParameter pn = fM.get_Parameter(p.Definition.Name);
                  if (pn == null) continue;
                  if (pn.IsInstance == false ) continue;
                  fM.MakeType(pn);
              }
              transaction.Commit();
              return Result.Succeeded;
          }
          //命令异常退出
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return Result.Failed;
          }
      }
  }
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class ParameterTypeToInstance : IExternalCommand
  {
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
          try
          {
              UIApplication uiapp = commandData.Application;
              UIDocument uidoc = uiapp.ActiveUIDocument;
              Document doc = uidoc.Document;

              FamilyManager fM = doc.FamilyManager;

              FrmParameters frmParameters = new FrmParameters(uiapp);
              if (frmParameters.ShowDialog() != DialogResult.OK) return Result.Cancelled;

              Document sourceDoc = frmParameters.SelectedDoc.Value;
              FamilyManager sfM = sourceDoc.FamilyManager;

              if (fM.Types.Size == 0)
              {
                  throw new Exception("Current Family must have type");
              }
              if (sfM.Types.Size == 0)
              {
                  throw new Exception("Source Family must have type");
              }

              List<FamilyParameter> sourceParameters = frmParameters.SelectedParameters.Values.ToList<FamilyParameter>();

              Transaction transaction = new Transaction(uidoc.Document);
              transaction.Start("Instance Parameter Become Type Parameter");
              List<FamilyParameter> added = new List<FamilyParameter>();
              FamilyType type = fM.CurrentType;
              foreach (FamilyParameter p in sourceParameters)
              {
                  FamilyParameter pn = fM.get_Parameter(p.Definition.Name);
                  if (pn == null) continue;
                  if (pn.IsInstance == true) continue;
                  fM.MakeInstance(pn);
              }
              transaction.Commit();
              return Result.Succeeded;
          }
          //命令异常退出
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return Result.Failed;
          }
      }
  }
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class ParameterToData : IExternalCommand
  {
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
          try
          {
              UIApplication uiapp = commandData.Application;
              UIDocument uidoc = uiapp.ActiveUIDocument;
              Document doc = uidoc.Document;

              FamilyManager fM = doc.FamilyManager;

              FrmParameters frmParameters = new FrmParameters(uiapp);
              if (frmParameters.ShowDialog() != DialogResult.OK) return Result.Cancelled;

              Document sourceDoc = frmParameters.SelectedDoc.Value;
              FamilyManager sfM = sourceDoc.FamilyManager;

              if (fM.Types.Size == 0)
              {
                  throw new Exception("Current Family must have type");
              }
              if (sfM.Types.Size == 0)
              {
                  throw new Exception("Source Family must have type");
              }

              List<FamilyParameter> sourceParameters = frmParameters.SelectedParameters.Values.ToList<FamilyParameter>();

              Transaction transaction = new Transaction(uidoc.Document);
              transaction.Start("Instance Parameter Become Type Parameter");
              List<FamilyParameter> added = new List<FamilyParameter>();
              FamilyType type = fM.CurrentType;
              foreach (FamilyParameter p in sourceParameters)
              {
                  FamilyParameter pn = fM.get_Parameter(p.Definition.Name);
                  if (pn == null) continue;
                  if (pn.IsInstance == true) continue;
                  fM.ReplaceParameter(pn, pn.Definition as ExternalDefinition, BuiltInParameterGroup.PG_DATA , true);
              }
              transaction.Commit();
              return Result.Succeeded;
          }
          //命令异常退出
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return Result.Failed;
          }
      }
  }

  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class ParameterToConstraint : IExternalCommand
  {
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
          try
          {
              UIApplication uiapp = commandData.Application;
              UIDocument uidoc = uiapp.ActiveUIDocument;
              Document doc = uidoc.Document;

              FamilyManager fM = doc.FamilyManager;

              FrmParameters frmParameters = new FrmParameters(uiapp);
              if (frmParameters.ShowDialog() != DialogResult.OK) return Result.Cancelled;

              Document sourceDoc = frmParameters.SelectedDoc.Value;
              FamilyManager sfM = sourceDoc.FamilyManager;

              if (fM.Types.Size == 0)
              {
                  throw new Exception("Current Family must have type");
              }
              if (sfM.Types.Size == 0)
              {
                  throw new Exception("Source Family must have type");
              }

              List<FamilyParameter> sourceParameters = frmParameters.SelectedParameters.Values.ToList<FamilyParameter>();

              Transaction transaction = new Transaction(uidoc.Document);
              transaction.Start("Instance Parameter Become Type Parameter");
              List<FamilyParameter> added = new List<FamilyParameter>();
              FamilyType type = fM.CurrentType;
              foreach (FamilyParameter p in sourceParameters)
              {
                  FamilyParameter pn = fM.get_Parameter(p.Definition.Name);
                  if (pn == null) continue;
                  if (pn.IsInstance == true) continue;
                  fM.ReplaceParameter(pn, pn.Definition as ExternalDefinition, BuiltInParameterGroup.PG_CONSTRAINTS, true);
              }
              transaction.Commit();
              return Result.Succeeded;
          }
          //命令异常退出
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return Result.Failed;
          }
      }
  }
  [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
  [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
  public class ParameterRemove : IExternalCommand
  {
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
          try
          {
              UIApplication uiapp = commandData.Application;
              UIDocument uidoc = uiapp.ActiveUIDocument;
              Document doc = uidoc.Document;

              FamilyManager fM = doc.FamilyManager;

              FrmParameters frmParameters = new FrmParameters(uiapp);
              if (frmParameters.ShowDialog() != DialogResult.OK) return Result.Cancelled;

              Document sourceDoc = frmParameters.SelectedDoc.Value;
              FamilyManager sfM = sourceDoc.FamilyManager;

              if (fM.Types.Size == 0)
              {
                  throw new Exception("Current Family must have type");
              }
              if (sfM.Types.Size == 0)
              {
                  throw new Exception("Source Family must have type");
              }

              List<FamilyParameter> sourceParameters = frmParameters.SelectedParameters.Values.ToList<FamilyParameter>();

              Transaction transaction = new Transaction(uidoc.Document);
              transaction.Start("Instance Parameter Become Type Parameter");
              List<FamilyParameter> added = new List<FamilyParameter>();
              FamilyType type = fM.CurrentType;
              foreach (FamilyParameter p in sourceParameters)
              {
                  FamilyParameter pn = fM.get_Parameter(p.Definition.Name);
                  if (pn == null) continue;
                  fM.RemoveParameter(pn);
              }
              transaction.Commit();
              return Result.Succeeded;
          }
          //命令异常退出
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return Result.Failed;
          }
      }
  }*/
}