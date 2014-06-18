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
    public class XMLFileReaderTests
    {
        [TestMethod]
        public void TestReaderReturnsContentFromValidFile()
        {
            Assert.IsFalse(true);
        }

        /*
         * These exceptions are specifically tested for as they are used to provide
         * specific error messages to the user. Any changes to StandardFileReader must
         * still satisfy these tests.
         */
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestIncorrectFileNameThrowsExpectedException()
        {
            Assert.IsFalse(true);
        }

         [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestInvalidPathThrowsExpectedException()
        {
            Assert.IsFalse(true);
        }
    }
}