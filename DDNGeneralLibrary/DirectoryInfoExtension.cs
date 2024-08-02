using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNGeneralLibrary
{
    public static class DirectoryInfoExtension
    {
        public static DirectoryInfo GetDirectoryInfo(string path)
        {
            return new DirectoryInfo(path);
        }

        public static IList<FileInfo> GetFiles(string path, string searchPattern)
        {
            return GetDirectoryInfo(path).ExtGetFiles(searchPattern);
        }

        public static IList<FileInfo> ExtGetFiles(this DirectoryInfo directoryInfo, string searchPattern)
        {
            return directoryInfo.GetFiles(searchPattern).ToList();
        }
        public static bool DirectoryExists(string directoryName)
        {
            // Check the FileName argument.
            if (string.IsNullOrEmpty(directoryName))
            {
                return false;
            }

            // Check to see if the file exists.
            DirectoryInfo fInfo = GetDirectoryInfo(directoryName);

            // You can throw a personalized exception if 
            // the file does not exist.
            if (!fInfo.Exists)
            {
                return false;
            }

            return true;
        }
    }
}
