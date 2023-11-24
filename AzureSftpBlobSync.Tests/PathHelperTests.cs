namespace AzureSftpBlobSync.Tests
{
    public class PathHelperTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase("/origin/input", "destiny/output", "/origin/input/test.txt", "/destiny/output/test.txt")]
        [TestCase("/origin/input", "destiny/output", "/origin/input/subfolder/test.txt", "/destiny/output/subfolder/test.txt")]
        [TestCase("origin/input", "/destiny/output/", "/origin/input/subfolder/test.txt", "/destiny/output/subfolder/test.txt")]
        public void Test1(string originSearchPath, string destinyPath, string fullFileName, string expectedResult )
        {
            var result = PathHelper.CalculateDestiny(originSearchPath, destinyPath, fullFileName);
            Assert.That( result, Is.EqualTo( expectedResult ) );
        }
    }
}