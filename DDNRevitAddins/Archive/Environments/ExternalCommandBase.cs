using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Castle.Core.Internal;
using DDNGeneralLibrary;
using DDNRevitAddins.AddinLibOfFamlyParameterTransfer;
using DDNRevitAddins.Archive.Datas;
using DDNRevitAddins.General;
using OperationCanceledException = Autodesk.Revit.Exceptions.OperationCanceledException;

namespace DDNRevitAddins.Archive.Environments
{
    public abstract class ExternalCommandBase : IExternalCommand
    {
        public delegate void ContinuousTransactionAfterHandler();

        public delegate void ContinuousTransactionHandler();

        public delegate void SingleTransactionHandler();


        protected Application RevitDbApplication;
        protected Document RevitDbDocument;
        protected Selection RevitSelection;
        public double RevitShortCurveTolerance;
        protected UIApplication RevitUiApplication;
        protected UIDocument RevitUiDocument;
        protected Transaction Transaction;
        private IDictionary< string,Delegate> _transactionHandlerDictionary;
        protected View RevitActiveView;
        protected Autodesk.Revit.Creation.Application RevitCreateApplication;


        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                if (!InitiateRevitAccessor(commandData)) return Result.Cancelled;
                if (!RunBeforeTransaction()) return Result.Cancelled;
                var i = 0;
                while (i < _transactionHandlerDictionary.Count)
                {
                    KeyValuePair<string, Delegate> pair = _transactionHandlerDictionary.ElementAt(i);
                    var handler = pair.Value;
                    var name = pair.Key;
                    if (handler is SingleTransactionHandler)
                    {
                        RunCustomTransactions(name, (SingleTransactionHandler)handler);
                        i++;
                        continue;
                    }

                    if (i == _transactionHandlerDictionary.Count - 1) continue;
                    var afterHandler = _transactionHandlerDictionary.ElementAt(i + 1).Value;
                    if (handler is ContinuousTransactionHandler && afterHandler is ContinuousTransactionAfterHandler)
                    {
                        RunContinuousTransactions((ContinuousTransactionHandler)handler,
                            (ContinuousTransactionAfterHandler)afterHandler);
                        i = i + 2;
                        continue;
                    }

                    throw new Exception("New TransactionHandler Type haven't been defined.");
                }

                return Result.Succeeded;
            }
            catch (ExceptionToCancel e)
            {
                TaskDialogShowing(e.Message);
                return Result.Cancelled;
            }
        }

        protected abstract bool RunBeforeTransaction();


        internal virtual bool InitiateRevitAccessor(ExternalCommandData commandData)
        {
            RevitUiApplication = commandData.Application;
            RevitDbApplication = RevitUiApplication.Application;
            RevitShortCurveTolerance = RevitDbApplication.ExtShortCurveTolerance();
            RevitUiDocument = RevitUiApplication.ActiveUIDocument;
            RevitDbDocument = RevitUiDocument.Document;
            RevitSelection = RevitUiDocument.Selection;
            RevitActiveView = RevitDbDocument.ExtGetActiveView();
            RevitCreateApplication = RevitDbApplication.ExtGetCreateApplication();
            _transactionHandlerDictionary = new Dictionary<string, Delegate>();
            return true;
        }


        protected Element PickElementOfCategory(string categoryName, string statusPrompt)
        {
            var reference = RevitSelection.ExtPickElementOfCategory(categoryName, statusPrompt);
            if (reference == null) return null;
            return RevitDbDocument.ExtGetElement(reference);
        }

        protected Element PickElement(string statusPrompt)
        {
            var reference = RevitSelection.ExtPickElement(statusPrompt);
            if (reference == null) return null;
            return RevitDbDocument.ExtGetElement(reference);
        }

        protected IList<Element> PickElementsOfCategory(string categoryName, string statusPrompt)
        {
            var references = RevitSelection.ExtPickElementsOfCategory(categoryName, statusPrompt);
            if (references == null || !references.Any()) return null;
            return RevitDbDocument.ExtGetElements(references);
        }

        protected IList<Element> PickElements(string statusPrompt)
        {
            var references = RevitSelection.ExtPickElements(statusPrompt);
            if (references == null || !references.Any()) return null;
            return RevitDbDocument.ExtGetElements(references);
        }

        protected string AddinName()
        {
            return GetType().Name;
        }


        protected bool NewTransaction(string name)
        {
            Transaction = RevitDbDocument.ExtNewTransaction(name);
            if (Transaction == null) return false;
            return true;
        }
        protected bool CommitTransaction()
        {
            return Transaction.ExtCommit();
            ;
        }

        private string GetSubTransactionName()
        {
            StringBuilder result = new StringBuilder();
            result.Append(AddinName());
            int count = _transactionHandlerDictionary.Count;
            if (count == 0) return result.ToString();
            String separator = StringExtension.TextSeparatorUnderscore.ToString();
            result.Append(separator); ;
            result.Append(count.ToString());
            return result.ToString();
        }

        public void AddSingleTransactionHandler(SingleTransactionHandler handler)
        {
            _transactionHandlerDictionary.Add(
                GetSubTransactionName(),
                handler);
        }
        protected void AddContinuousTransactionHandler(ContinuousTransactionHandler handler, ContinuousTransactionAfterHandler afterhandler)
        {
            _transactionHandlerDictionary.Add(GetSubTransactionName(), handler);
            _transactionHandlerDictionary.Add(GetSubTransactionName(), afterhandler);
        }

        protected bool RunCustomTransactions(string name,SingleTransactionHandler transactionHandler)
        {
            if (transactionHandler == null) return false;
            try
            {
                if (!NewTransaction(name)) return false;
                transactionHandler();
                CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                TaskDialogShowing(e.Message);
            }

            Transaction.ExtRollBack();
            return false;
        }
        protected bool RunContinuousTransactions(ContinuousTransactionHandler transactionHandler,
            ContinuousTransactionAfterHandler afterHandler)
        {
            if (transactionHandler == null || afterHandler == null) return false;
            try
            {
                while (NewTransaction(AddinName()))
                {
                    transactionHandler();
                    CommitTransaction();
                }
            }
            catch (ExceptionToCancelContinuousTransaction)
            {
                afterHandler();
                CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                TaskDialogShowing(e.Message);
            }

            Transaction.ExtRollBack();
            return false;
        }

        private void TaskDialogShowing(string message)
        {
            TaskDialogExtension.ExtShow(AddinName(), message);
        }


        protected IList<Element> PickElementsOfFamilyInstance(string statusPrompt)
        {
            var references =
                RevitSelection.ExtPickElements(new FamilyInstanceSelectionFilter(RevitDbDocument), statusPrompt);
            if (references == null || !references.Any()) return null;
            return RevitDbDocument.ExtGetElements(references);
        }

        protected void ThrowExceptionIfNoFamilyDocument()
        {
            if (!RevitDbDocument.ExtIsFamilyDocument())
                throw new ExceptionToCancel("This should be run on Family documents.");
        }

        protected void ThrowExceptionIfNoProjectDocument()
        {
            if (RevitDbDocument.ExtIsFamilyDocument())
                throw new ExceptionToCancel("This should be run on Project documents.");
        }

        protected GraphicsStyle GetProjectLineStyle(string name)
        {
            var categoryOfLines = RevitDbDocument.ExtGetCategory( CategoriesExtension.BuiltInCategoryOfLines);
            var categoryOfLine = categoryOfLines.ExtGetSubCategory(name);
            if (categoryOfLine != null)
            {
                return categoryOfLine.ExtGetGraphicStyleOfProjection();
            }
            var newCat =RevitDbDocument.ExtNewSubCategory( categoryOfLines, name);
            return newCat.ExtGetGraphicStyleOfProjection();
        }
    }
}