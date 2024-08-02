namespace DDNGeneralLibrary
{
    public class TestClassBase
    {
        public const string TestFolder = @"./Test/";

        public static string GetTestFileFullName(string testFileName)
        {
            return TestFolder + testFileName;
        }
    }
}