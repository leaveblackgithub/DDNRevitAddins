using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DDNGeneralLibrary
{
    public static class FileOpen
    {
        public static string OpenFile(string fileDescription, string filter = "", string initialDirectory = "")
        {
            OpenFileDialog fileDlg = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                FileName = initialDirectory,
                Filter = filter,
                Multiselect = false,
                Title = fileDescription
            };
            if (fileDlg.ShowDialog() == DialogResult.OK)
                return fileDlg.FileName;
            return "";
        }

        public static string OpenFolder(string fileDescription, string initialDirectory = "")
        {
            FolderBrowserDialog fileDlg = new FolderBrowserDialog
            {
                Description = fileDescription,
                ShowNewFolderButton = false
            };
            if (initialDirectory != "")
            {
                //设置此次默认目录为上一次选中目录
                fileDlg.SelectedPath = initialDirectory;
            }
            //按下确定选择的按钮
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录
                return fileDlg.SelectedPath = initialDirectory;
            }
            return "";
        }

        public const string ImageFileFilter = "All Image Files (*.bmp, *.jpg, *.jpeg, *.png, *.tif)|*.bmp;*.jpg;*.jpeg;*.png;*.tif";
    }
}