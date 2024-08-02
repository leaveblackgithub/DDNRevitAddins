using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNGeneralLibrary
{
    public static class StreamReaderExtension
    {
        public static IList<string> ReadLines(string path)
        {
            StreamReader reader = new StreamReader(path);
            IList<string> result = new List<string>();
            string line;
            while ((line = reader.ExtReadLine()) != null)
            {
                line = line.ExtTrim();
                if (line.ExtIsNullOrEmpty()) continue;
                result.Add(line);
            }
            return result;
        }

        private static string ExtReadLine(this StreamReader reader)
        {
            return reader.ReadLine();
        }
    }
}
