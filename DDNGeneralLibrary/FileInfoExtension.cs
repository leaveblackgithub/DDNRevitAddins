using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DDNGeneralLibrary
{
    public static class FileInfoExtension
    {
        public static FileInfo GetFileInfo(string fullName)
        {
            return new FileInfo(fullName);
        }
        public static FileInfo GetDllFileInfo(string path,string fileName)
        {
            return GetFileInfo(path+fileName.AddDllExtension());
        }
        public static FileInfo GetAddinFileInfo(string path, string fileName)
        {
            return GetFileInfo(path + fileName.AddAddinExtension());
        }
        public static string AddDllExtension(this string fileName)
        {
            return fileName + DllExtension;
        }
        public static string AddAddinExtension(this string fileName)
        {
            return fileName + AddinExtension;
        }

        public static string ExtName(this FileInfo fileInfo)
        {
            return fileInfo.Name;
        }

        public static void ExtDelete(this FileInfo fileInfo)
        {
            fileInfo.Delete();
        }

        public static bool FileExists(string fileName)
        {
            // Check the FileName argument.
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            // Check to see if the file exists.
            FileInfo fInfo = GetFileInfo(fileName);

            // You can throw a personalized exception if 
            // the file does not exist.
            if (!fInfo.Exists)
            {
                return false;
            }

            return true;
        }


        public const string DllExtension = ".dll";
        public const string AddinSearchExtension = "*.addin";
        public const string DllSearchExtension = "*.dll";
        public const string AddinExtension = ".addin";
        public const string CurrentFolder = @".\";
    }
}