using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Castle.Core.Internal;

namespace DDNGeneralLibrary
{
    public static class StringExtension
    {
        private const string BackSplash = "\\";

        public static IList<string> ExtSplit(this string line, char[] separators)
        {
            return line.Split(separators).ToList();
        }

        public static IList<string> ExtSplit(this string line, char separator)
        {
            return line.ExtSplit(new char[] {separator});
        }
        public static string ExtAddBackSplash(this string path)
        {
            return path + BackSplash;
        }

        public const string LineChanger = "\n";

        public static void ExtAppendDoubleLines(this StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine();
            stringBuilder.AppendLine();
        }

        public static string ExtEmpty()
        {
            return String.Empty;
        }

        public static string ExtTrim(this string text)
        {
            return text.Trim();
        }
        public static int ExtLength(this string text)
        {
            return text.Length;
        }

        public static bool ExtIsNullOrEmpty(this string text)
        {
            return text.IsNullOrEmpty();
        }

        

        public const char TextSeparatorUnderscore = '_';
        public const char TextSeparatorComma = ',';
        public const char TextSeparatorDot = '.';
    }
}
