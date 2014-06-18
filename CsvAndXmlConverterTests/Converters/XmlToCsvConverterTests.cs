using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverterTests.Converters
{
    [TestClass]
    public class XmlToCsvConverterTests
    {
        [TestMethod]
        public void TestCorrectFilePathIsRequestedToBeReadFromFileReaderByConverter()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestConvertedFileIsPassedToFileWriterWithCorrectName()
        {
            Assert.IsFalse(true);
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
    }
}