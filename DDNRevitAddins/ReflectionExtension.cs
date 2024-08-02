using System.IO;
using System.Reflection;

namespace DDNRevitAddins
{
    public static class ReflectionExtension
    {
        public static string GetDllPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
