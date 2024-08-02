using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace DDNGeneralLibrary
{
    [TestFixture]
    public class ExcelExtensionTest
    {
        [Test]
        public void HelloWorldTest()
        {
            ExcelExtension xls = new ExcelExtension();
            xls.SetCellValue(1,1,"Hello World");
            List<object> rowValues = new List<object>
            {
                "a","b","c",1,2,3
            };
            xls.SetRowValues(3, rowValues,3);
            xls.SaveAs(@"F:\OneDrive\DDNRevitAddins\DDNGeneralLibrary\Test\Test.xlsx");
            xls.Close();
            Assert.True(true);
        }
    }
}