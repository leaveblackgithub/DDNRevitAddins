using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;

namespace DDNRevitAddins
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class IsolatedRadialDimension : IsolatedDimensionsBase
    {
        protected override bool RunBeforeTransaction()
        {
            base.RunBeforeTransaction();

            //AddSingleTransactionHandler((SingleTransactionHandler)CreateDimAndGroup);
            return true;
        }
    }
}
