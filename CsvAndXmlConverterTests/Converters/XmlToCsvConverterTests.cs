using CsvAndXmlConverter.Converters;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using CsvAndXmlConverter.Validator;
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
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderReturingDummyXmlConent(CreateBasicXMLStructure());
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            converter.ConvertFile(path);
            mockReader.Verify(mock => mock.ReadDataFromFile(path), Times.Exactly(1));
        }

        [TestMethod]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = CreateFileReaderReturingDummyXmlConent(CreateBasicXMLStructure());
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            converter.ConvertFile(@"C:\Location\file.xml");
            mockWriter.Verify(mock =>
                mock.SaveDataToFile(It.IsAny<MemoryStream>(), @"C:\Location\converted_file.csv"), Times.Exactly(1));
        }

        [TestMethod]
        public void TestARowIsAddedForEachChildElementOfTheRootElement()
        {
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockReader = CreateFileReaderReturingDummyXmlConent(LoadXmlDocument(@"../../Converters/XmlTestData/MultipleChildren.xml"));
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestOutputCSVContainsRowForEachFirstLevelChild(memoryStream));
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            converter.ConvertFile(@"C:\Location\file.xml");
        }

        private void TestOutputCSVContainsRowForEachFirstLevelChild(MemoryStream convertedContent)
        {
            var reader = new StreamReader(convertedContent);
            var content = reader.ReadToEnd().Split(new [] { Environment.NewLine }, StringSplitOptions.None);
            var countWithoutTitleRow = content.Skip(1).Count();
            Assert.AreEqual(5, countWithoutTitleRow);
        }

        [TestMethod]
        public void TestCommonElementTagsBecomeColumnTitles()
        {
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockReader = CreateFileReaderReturingDummyXmlConent(LoadXmlDocument(@"../../Converters/XmlTestData/MulitpleProperty.xml"));
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestColumnTitlesMatchElementTagsFromXmlFile(memoryStream));
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            converter.ConvertFile(@"C:\Location\file.xml");
        }

        private void TestColumnTitlesMatchElementTagsFromXmlFile(MemoryStream convertedContent)
        {
            var reader = new StreamReader(convertedContent);
            var content = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var titles = content.First();
            var contains = titles.Contains("name") && titles.Contains("id") && titles.Contains("date");
            Assert.IsTrue(contains);
            Assert.AreEqual(3, titles.Split(',').Length);
        }

        [TestMethod]
        public void TestElementValuesAreInCorrectColumnsAfterConversion()
        {
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockReader = CreateFileReaderReturingDummyXmlConent(LoadXmlDocument(@"../../Converters/XmlTestData/MulitpleProperty.xml"));
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns("Success")
                        .Callback((MemoryStream memoryStream, string s) => TestColumnValuesAreInCorrectColumnInCsvStream(memoryStream));
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            converter.ConvertFile(@"C:\Location\file.xml");
        }

        private void TestColumnValuesAreInCorrectColumnInCsvStream(MemoryStream csvStream)
        {
            var reader = new StreamReader(csvStream);
            var content = reader.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            var firstItem = content[1].Split(',');
            var secondItem = content[2].Split(',');
            Assert.IsTrue(firstItem[0] == "Name value" && secondItem[0] == "Name 2 value");
            Assert.IsTrue(firstItem[1] == "1" && secondItem[1] == "2");
            Assert.IsTrue(firstItem[2] == "26/07/14" && secondItem[2] == "25/07/14");
        }

        [TestMethod]
        public void TestSuccessIsReturnedFromConverterWhenConversionSucceds()
        {
            var successOfWriteMessage = "This idicates if the file write was successful";
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockReader = CreateFileReaderReturingDummyXmlConent(LoadXmlDocument(@"../../Converters/XmlTestData/MulitpleProperty.xml"));
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>()))
                        .Returns(successOfWriteMessage);
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            var result = converter.ConvertFile(@"C:\Location\file.xml");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(successOfWriteMessage, result.ResultMessage);
        }

        [TestMethod]
        public void TestFullContentOfConvertedFileIsCorrect()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestConversionIsNotAttemptedAfterAValidationFailure()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestValidationFailureIsReportedByTheConverter()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestMissingFileErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\strange.csv";
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IXMLFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new FileNotFoundException("not found"));
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            var result = converter.ConvertFile(testFileName);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(string.Format(Resources.FileNotFoundMessage, testFileName), result.ResultMessage);
        }

        [TestMethod]
        public void TestMissingDirectoryErrorIsReportedByConverter()
        {
            var testFileName = @"C:\Somewhere\strange.csv";
            var mockValidator = CreateDummyValidatorReturningSuccess();
            var mockWriter = CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage();
            var mockReader = new Mock<IXMLFileReader>();
            mockReader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Throws(new DirectoryNotFoundException("not found"));
            var converter = new XmlToCsvConverter(mockWriter.Object, mockReader.Object, mockValidator.Object);
            var result = converter.ConvertFile(testFileName);
            Assert.AreEqual(false, result.Success);
            Assert.AreEqual(string.Format(Resources.DirectoryNotFoundMessage, Path.GetPathRoot(testFileName)), result.ResultMessage);
        }

        private Mock<IFileWriter> CreateBasicFileWriterAcceptingAnyParametersAndReturningDummySuccessMessage()
        {
            var mockWriter = new Mock<IFileWriter>();
            mockWriter.Setup(mock => mock.SaveDataToFile(It.IsAny<MemoryStream>(), It.IsAny<string>())).Returns("Success");
            return mockWriter;
        }

        private Mock<IXMLFileReader> CreateFileReaderReturingDummyXmlConent(XDocument content)
        {
            var reader = new Mock<IXMLFileReader>();
            reader.Setup(mock => mock.ReadDataFromFile(It.IsAny<string>())).Returns(content);
            return reader;
        }

        private Mock<IXmlValidator> CreateDummyValidatorReturningSuccess()
        {
            var mockValidator = new Mock<IXmlValidator>();
            mockValidator.Setup(mock => mock.ValidateXml(It.IsAny<XDocument>()))
                         .Returns(new Tuple<bool, string[]>(true, new string[]{}));
            return mockValidator;
        }

        private XDocument CreateBasicXMLStructure()
        {
            var document = new XDocument();
            var dummyDataRoot = new XElement("items");
            dummyDataRoot.Add(CreateBasicChildElement(1));
            dummyDataRoot.Add(CreateBasicChildElement(2));
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

        private XDocument LoadXmlDocument(string path)
        {
            return XDocument.Load(path);
        }
    }
}