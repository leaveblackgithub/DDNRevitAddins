using System.Collections.Generic;
using System.IO;
using DDNGeneralLibrary;

namespace DDNRevitAddinInstaller.RevitAddinUtilityExtensions
{
    public class RevitAddInManifestCollection
    {
        private readonly IList<RevitAddInManifestWrapper> _addins=new List<RevitAddInManifestWrapper>();
        public IDictionary<string, FileInfo> InstalledClasses { get; set; } = new Dictionary<string, FileInfo>();

        public RevitAddInManifestCollection(IList<FileInfo> fileInfos)
        {
            foreach (var fileInfo in fileInfos)
            {
                RevitAddInManifestWrapper addin=RevitAddInManifestWrapper.CreateFromAddinFileInfo(fileInfo);
                if (addin == null) continue;
                _addins.Add(addin);
                InstalledClasses=InstalledClasses.MergeDict(addin.InstalledClasses);
            }
        }
    }
}
