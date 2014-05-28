using CsvAndXmlConverter.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestReturnStatusForWritingToDestingationWithNoPermissions()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestReturnStatusForWritingToNonExistantDestination()
        {
            Assert.IsFalse(true);
        }
    }
}
