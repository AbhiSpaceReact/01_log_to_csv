using NUnit.Framework;
using System.Collections.Generic;
using System.IO;

namespace NUnitTestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetFilesList_UnitTest()
        {
            string SourceDirecoryPath= @"D:\Office\OfficeAssignment\01_log_to_csv\LogToCSVConverter\NUnitTestProject1\Data\";
            List<string> fileNameList=    LogToCSVConverter.FileUtility.GetLogFileList(SourceDirecoryPath);

            Assert.AreEqual(1, fileNameList.Count);
            Assert.AreEqual("app.log", Path.GetFileName(fileNameList[0]));
        }
    }
}