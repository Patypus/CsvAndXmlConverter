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
using System.Xml.Linq;

namespace CsvAndXmlConverterTests.Converters
{
    [TestClass]
    public class XmlToCsvConverterTests
    {
        [TestMethod]
        public void TestCorrectFilePathIsRequestedToBeReadFromFileReaderByConverter()
        {
            var path = @"C:\Test\This\path\isPassed\toTheReader.xml";
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderReturingBasicDummyXmlConent();
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object);
            converter.ConvertFile(path);
            mockReader.Verify(mock => mock.ReadDataFromFile(path), Times.Exactly(1));
        }

        [TestMethod]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderReturingBasicDummyXmlConent();
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object);
            converter.ConvertFile(@"C:\Location\file.xml");
            mockWriter.Verify(mock =>
                mock.SaveDataToFile(It.IsAny<MemoryStream>(), @"C:\Location\converted_file.csv"), Times.Exactly(1));
        }

        [TestMethod]
        public void TestARowIsAddedForEachChildElementOfTheRootElement()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestCommonElementTagsBecomeColumnTitles()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestElementValuesAreInCorrectColumnsAfterConversion()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestFullContentOfConvertedFileIsCorrect()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestUnMatchingElemetIsHandledCorrectly()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestFileWithNoDataIsHandledCorrectly()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestFileWithNoChildTagsIsHandledCorrectly()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestMissingFileErrorIsReportedByConverter()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestMissingDirectoryErrorIsReportedByConverter()
        {
            Assert.IsFalse(true);
        }

        private Mock<IFileWriter> CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage()
        {
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("Success");
            return mockWriter;
        }

        private Mock<IXMLFileReader> CreateFileReaderReturingBasicDummyXmlConent()
        {
            var content = CreateBasicXMLStructure();
            var reader = new Mock<IXMLFileReader>();
            reader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Returns(content);
            return reader;
        }

        private XDocument CreateBasicXMLStructure()
        {
            var document = new XDocument();
            var dummyDataRoot = new XElement("items");
            var dummyItemOne = CreateBasicChildElement(1);
            var dummyItemTwo = CreateBasicChildElement(2);
            document.Add(dummyDataRoot);
            return document;
        }

        private XElement CreateBasicChildElement(int index)
        {
            var element = new XElement("item");
            var valueOne = new XElement("value1");
            var valueTwo = new XElement("value2");
            valueOne.Value = "item" + index + "content1";
            valueTwo.Value = "item" + index + "content2";
            element.Add(valueOne);
            element.Add(valueTwo);
            return element;
        }
    }
}