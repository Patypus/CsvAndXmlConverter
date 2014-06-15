using CsvAndXmlConverter.Converters;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverterTests.Converters
{
    [TestClass]
    public class CsvToXmlConverterTests
    {
        [TestMethod]
        public void TestCorrectFilePathIsRequestedToBeReadFromFileReaderByConverter()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockReader.Verify(mock => mock.ReadDataFromFile(@"C:\Somewhere\change.csv"), Times.Exactly(1));
        }

        [TestMethod]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockWriter.Verify(mock => 
                mock.SaveDataToFile(It.IsAny<MemoryStream>(), @"C:\Somewhere\converted_change.xml"), Times.Exactly(1));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void TestEachRowIsRepresentedAsASeparateElement()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestColumnTitlesAreConvertedToElements()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestColumnValuesAreInCorrectElementTagsAfterConversion()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestContentOfConvertedFileIsCorrect()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestCsvFileWithNoDataIsHandledProperly()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestOutputMessageFromFileWriterIsPassedUpThoughConverterToCallerInResult()
        {
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningDummyContent();
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("I am an error message");
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(@"C:\Somehere\aFile\mightbe.csv");
            Assert.AreEqual("I am an error message", result.ResultMessage);
        }

        [TestMethod]
        public void TestMissingFileErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\change.csv";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new FileNotFoundException("not found"));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(testFileName);
            Assert.AreEqual(false, result.Completed);
            Assert.AreEqual(string.Format(Resources.FileNotFoundMessage, testFileName), result.ResultMessage);
        }

        [TestMethod]
        public void TestMissingDirectoryErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\change.csv";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new DirectoryNotFoundException("not found"));
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            var result = converter.ConvertFile(testFileName);
            var expectedResult = string.Format(Resources.DirectoryNotFoundMessage, Path.GetPathRoot(testFileName));
            Assert.AreEqual(false, result.Completed);
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
