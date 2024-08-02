using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Autodesk.RevitAddIns;
using DDNGeneralLibrary;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public static class RevitProductExtension
    {
        private static List<RevitProduct> ExtGetAllInstalledRevitProducts()
        {
            return RevitProductUtility.GetAllInstalledRevitProducts();
        }

        private static bool IsVersion(this RevitProduct product, RevitVersion revitVersion)
        {
            return ExtGetVersion(product) == revitVersion;
        }

        private static RevitVersion ExtGetVersion(this RevitProduct product)
        {
            return product.Version;
        }

        private static string ExtGetAllUsersAddInFolder(this RevitProduct currentProduct)
        {
            return currentProduct.AllUsersAddInFolder.ExtAddBackSplash();
        }

        private const RevitVersion CurrentRevitVersion = RevitVersion.Revit2020;

        public static string GetCurrentProductAllUsersAddInFolder()
        {
            return @"C:\ProgramData\Autodesk\Revit\Addins\2021\";
        }

        internal static RevitProduct GetCurrentProduct()
        {
            List<RevitProduct> revitProducts = ExtGetAllInstalledRevitProducts();
            if (!revitProducts.Any()) return null;
            foreach (RevitProduct product in revitProducts)
            {
                if (product.IsVersion(CurrentRevitVersion))
                {
                    return product;
                }
            }
            return null;
        }
        public static IList<FileInfo> GetAddinFiles(string searchPattern)
        {
            return DirectoryInfoExtension.GetFiles(GetCurrentProductAllUsersAddInFolder(), searchPattern);
        }
    }
}
