using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverterTests.IO
{
    [TestFixture]
    public class FileWriterTests
    {
        private static IFileWriter writer;

        [TestFixtureSetUp]
        public void Setup()
        {
            writer = new FileWriter();
        }

        [Test]
        public void TestWritingDataToFileCreatesFile()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"..\..\IO\WriteTarget\Output.csv";
            writer.SaveDataToFile(content, filePath);
            var exists = File.Exists(filePath);
            File.Delete(filePath);
            Assert.IsTrue(exists);
        }

        [Test]
        public void TestWritingDataFromStreamForCsv()
        {
            var csvData = "col1,col2,col3" + Environment.NewLine + "red,yellow,blue";
            var filePath = @"..\..\IO\WriteTarget\CsvTest.csv";
            writer.SaveDataToFile(CreateStreamForDataString(csvData), filePath);
            var dataStrngFromFile = File.ReadAllText(filePath);
            File.Delete(filePath);
            Assert.AreEqual(csvData, dataStrngFromFile);
        }

        [Test]
        public void TestWritingDataFromStreamForXml()
        {
            var xmlData = "<Items>" + Environment.NewLine +
                            "   <Name>George</Name>" + Environment.NewLine + 
                            "</Items>";
            var filePath = @"..\..\IO\WriteTarget\XmlTest.xml";
            writer.SaveDataToFile(CreateStreamForDataString(xmlData), filePath);
            var dataStrngFromFile = File.ReadAllText(filePath);
            File.Delete(filePath);
            Assert.AreEqual(xmlData, dataStrngFromFile);
        }

        [Test]
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

        [Test]
        public void TestReturnStatusForWritingToDestingationWithNoPermissions()
        {
            var content = CreateStreamForDataString("col1,col2,col3");
            var filePath = @"C:\Program Files\atAll.txt";
            var stringResult = writer.SaveDataToFile(content, filePath);
            var expectedString = string.Format(Resources.UnableToSaveFilePermissionDenied, @"C:\Program Files");
            Assert.AreEqual(expectedString, stringResult);
        }

        [Test]
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
