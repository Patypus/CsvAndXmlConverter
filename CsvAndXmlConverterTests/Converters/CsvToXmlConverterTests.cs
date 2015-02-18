using CsvAndXmlConverter.Converters;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverterTests.Converters
{
    [TestFixture]
    public class CsvToXmlConverterTests
    {
        [Test]
        public void TestCorrectFilePathIsRequestedToBeReadFromFileReaderByConverter()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockReader.Verify(mock => mock.ReadDataFromFile(@"C:\Somewhere\change.csv"), Times.Exactly(1));
        }

        [Test]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockWriter.Verify(mock => 
                mock.SaveDataToFile(It.IsAny<MemoryStream>(), @"C:\Somewhere\converted_change.xml"), Times.Exactly(1));
        }

        [Test]
        public void TestRootElementIsBasedOnFileName()
        {
            var expectedName = "change";
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestStreamContentForStartingAndClosingTags(memoryStream, expectedName));
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\" + expectedName + ".csv");
        }

        private void TestStreamContentForStartingAndClosingTags(MemoryStream stream, string expectedName)
        {
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd().Split('\r');
            Assert.IsTrue(content.First().Contains("<" + expectedName + "s>"));
            Assert.IsTrue(content.Last().Contains("</" + expectedName + "s>"));
        }

        [Test]
        public void TestEachRecordsRootElementIsBasedOnFileName()
        {
            var expectedName = "element";
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestElementStartAndEndTagsAreBasedOnFileName(memoryStream, expectedName));
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\else\" + expectedName + ".csv");
        }

        private void TestElementStartAndEndTagsAreBasedOnFileName(MemoryStream stream, string expectedName)
        {
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd().Split('\r');
            Assert.IsTrue(content[1].Contains("<" + expectedName + ">"));
            Assert.IsTrue(content[content.Length - 2].Contains("</" + expectedName + ">"));
        }

        [Test]
        public void TestEachRowIsRepresentedAsASeparateElement()
        {
            var testFileName = "testFile";
            //setup mock writer
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestSteamContentForRowsAsElements(memoryStream, testFileName));
            //setup mock reader
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>()))
                      .Returns(new[] { "col1,col2", "item1.1,item1.2", "item1.1,item1.2" });
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\else\" + testFileName + ".csv");
        }

        private void TestSteamContentForRowsAsElements(MemoryStream stream, string fileName)
        {
            var documentFromStream = XDocument.Load(stream);
            var count = documentFromStream.Element(fileName + "s").Elements(fileName).Count();
            Assert.AreEqual(2, count);
        }

        [Test]
        public void TestColumnTitlesAreConvertedToElements()
        {
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestColumnNamesAreConvertedToElementsInOutputStream(memoryStream));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Any\Old\file.txt");
        }

        private void TestColumnNamesAreConvertedToElementsInOutputStream(MemoryStream stream)
        {
            var documentFromStream = XDocument.Load(stream);
            var rowElements = documentFromStream.Element("files").Descendants("file");
            Assert.IsTrue(rowElements.Elements("col1").Count() == 1);
            Assert.IsTrue(rowElements.Elements("col2").Count() == 1);
            Assert.AreEqual(2, rowElements.Descendants().Count());
        }

        [Test]
        public void TestColumnValuesAreInCorrectElementTagsAfterConversion()
        {
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestValuesOfColumnElementsInStream(memoryStream));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Any\Old\file.txt");
        }

        private void TestValuesOfColumnElementsInStream(MemoryStream stream)
        {
            var documentFromStream = XDocument.Load(stream);
            var rowElements = documentFromStream.Element("files").Descendants("file");
            var col1Element = rowElements.Elements("col1");
            var col2Element = rowElements.Elements("col2");
            Assert.AreEqual(1, col1Element.Count(), col2Element.Count());
            Assert.AreEqual("item1", col1Element.First().Value);
            Assert.AreEqual("item2", col2Element.First().Value);
        }

        [Test]
        public void TestContentOfConvertedFileIsCorrect()
        {
            var fileName = "file";
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestFullFileContentIsAsExpected(memoryStream, fileName));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Any\Old\" + fileName + ".txt");
        }

        private void TestFullFileContentIsAsExpected(MemoryStream stream, string filename)
        {
            var expected = "<" + filename + "s>" + Environment.NewLine +
                           "  <" + filename + ">" + Environment.NewLine +
                           "    <col1>item1</col1>" + Environment.NewLine +
                           "    <col2>item2</col2>" + Environment.NewLine +
                           "  </" + filename + ">" + Environment.NewLine +
                           "</" + filename + "s>";
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            Assert.AreEqual(expected, content);
        }

        [Test]
        public void TestCsvFileWithNoDataIsHandledProperly()
        {
            var path = @"C:\Pathto\anempty\file.csv";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Returns(new string[0]);
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(path);
            var expectedMessage = string.Format(Resources.NotDataInFileToConvert, path);
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedMessage, result.ResultMessage);
        }

        [Test]
        public void TestOutputMessageFromFileWriterIsPassedUpThoughConverterToCallerInResult()
        {
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("I am an error message");
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(@"C:\Somehere\aFile\mightbe.csv");
            Assert.AreEqual("I am an error message", result.ResultMessage);
        }

        [Test]
        public void TestMissingFileErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\strange.csv";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new FileNotFoundException("not found"));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(testFileName);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(string.Format(Resources.FileNotFoundMessage, testFileName), result.ResultMessage);
        }

        [Test]
        public void TestMissingDirectoryErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\strange.csv";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new DirectoryNotFoundException("not found"));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(testFileName);
            var expectedResult = string.Format(Resources.DirectoryNotFoundMessage, Path.GetPathRoot(testFileName));
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedResult, result.ResultMessage);
        }

        private Mock<IFileWriter> CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage()
        {
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("Success");
            return mockWriter;
        }

        private Mock<IStandardFileReader> CreateFileReaderAcceptingAllPathAndReturningDummyContent()
        {
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Returns(new[] { "col1,col2", "item1,item2" });
            return mockReader;
        }
    }
}
