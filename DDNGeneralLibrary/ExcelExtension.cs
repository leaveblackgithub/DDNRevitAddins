using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using Excel = Microsoft.Office.Interop.Excel;

namespace DDNGeneralLibrary
{
    public class ExcelExtension
    {
        private Application _xlsApp;
        private Workbook _workBook;
        private Worksheet _sheet;

        public ExcelExtension()
        {
            _xlsApp = new Excel.Application();
            _workBook = _xlsApp.Workbooks.Add();
            _sheet = _workBook.ActiveSheet;
        }

        public void SetCellValue(int row, int col, object value)
        {
            _sheet.Cells[row, col] = value;
        }

        public void SetRowValues(int row, IList<object> values, int colStart=0)
        {
            foreach (var value in values)
            {
                SetCellValue(row,colStart,value);
                colStart++;
            }
        }

        public void SaveAs(string fullName)
        {
            _workBook.SaveAs(fullName);
            _workBook.Close();
        }

        public void Close()
        {
            try
            {
                _xlsApp.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_xlsApp);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(_workBook);

                Process[] excelProcesses = Process.GetProcessesByName("excel");
                foreach (Process p in excelProcesses)
                {
                    if (string.IsNullOrEmpty(p.MainWindowTitle)) // use MainWindowTitle to distinguish this excel process with other excel processes 
                    {
                        p.Kill();
                    }
                }
            }
            catch (Exception ex2)
            { }
        }
    }
}
