using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverterTests.IO
{
    [TestClass]
    public class FileWriterTests
    {
        private static IFileWriter writer;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            writer = new FileWriter();
        }

        [TestMethod]
        public void TestWritingDataToFileCreatesFile()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"..\..\IO\WriteTarget\Output.csv";
            writer.SaveDataToFile(content, filePath);
            var exists = File.Exists(filePath);
            File.Delete(filePath);
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void TestWritingDataFromStreamForCsv()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestWritingDataFromStreamForXml()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestReturnStatusForSuccessfulWriteIsCorrect()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"..\..\IO\WriteTarget\Output.csv";
            var stringResult = writer.SaveDataToFile(content, filePath);
            var exists = File.Exists(filePath);
            File.Delete(filePath);
            Assert.IsTrue(exists);
            Assert.AreEqual(string.Format(Resources.FileCreated, filePath), stringResult);
        }

        [TestMethod]
        public void TestReturnStatusForWritingToDestingationWithNoPermissions()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"C:\Program Files\atAll.txt";
            var stringResult = writer.SaveDataToFile(content, filePath);
            var expectedString = string.Format(Resources.UnableToSaveFilePermissionDenied, @"C:\Program Files");
            Assert.AreEqual(expectedString, stringResult);
        }

        [TestMethod]
        public void TestReturnStatusForWritingToNonExistantDestination()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"C:\Nowhere\Real\atAll.txt";
            var stringResult = writer.SaveDataToFile(content, filePath);
            var expectedString = string.Format(Resources.UnableToSaveToNoExistantDirectory, @"C:\Nowhere\Real");
            Assert.AreEqual(expectedString, stringResult);
        }

        private MemoryStream CreateStreamForDataString(string content)
        {
            var contentStream = new MemoryStream();
            var writer = new StreamWriter(contentStream);
            writer.Write(content);
            writer.Flush();
            contentStream.Position = 0;
            return contentStream;
        }
    }
}
