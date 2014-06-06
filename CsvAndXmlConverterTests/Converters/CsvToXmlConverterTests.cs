using CsvAndXmlConverter.Converters;
using CsvAndXmlConverter.IO;
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
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningBummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockReader.Verify(mock => mock.ReadDataFromFile(@"C:\Somewhere\change.csv"), Times.Exactly(1));
        }

        [TestMethod]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderAcceptingAllPathAndReturningBummyContent();
            var converter = new CsvToXmlConverter(mockReader.Object, mockWriter.Object);
            converter.ConvertFile(@"C:\Somewhere\change.csv");
            mockWriter.Verify(mock => 
                mock.SaveDataToFile(It.IsAny<MemoryStream>(), @"C:\Somewhere\converted_change.xml"), Times.Exactly(1));
        }

        private Mock<IFileWriter> CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage()
        {
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("Success");
            return mockWriter;
        }

        private Mock<IStandardFileReader> CreateFileReaderAcceptingAllPathAndReturningBummyContent()
        {
            var mockReader = new Mock<IStandardFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Returns(new[] { "col1,col2", "item1,item2" });
            return mockReader;
        }

        [TestMethod]
        public void TestRootElementIsBasedOnFileName()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestEachRecordsRootElementIsBasedOnFileName()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void TestContentOfConvertedFileIsCorrect()
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
        public void TestEachRowIsRepresentedAsASeparateElement()
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
            Assert.IsTrue(false);
        }
    }
}
