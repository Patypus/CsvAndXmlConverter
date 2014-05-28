using CsvAndXmlConverter.IO;
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
    public class StandardFileReaderTests
    {
        private static IStandardFileReader reader;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            reader = new StandardFileReader();
        }

        [TestMethod]
        public void TestReaderReturnsContentFromValidFile()
        {
            var expectedContent = new[] { "I am test data.", "So am I.", "I am not. hehehe." };
            var path = @"../../IO/Data/TestTextFile.txt";
            var result = (string[]) reader.ReadDataFromFile(path);
            Assert.AreEqual(expectedContent.Count(), result.Count());
            Assert.AreEqual(expectedContent[0], result[0]);
            Assert.AreEqual(expectedContent[1], result[1]);
            Assert.AreEqual(expectedContent[2], result[2]);
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
            var path = @"../../IO/Data/NotReallyAfile.txt";
            reader.ReadDataFromFile(path);
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestInvalidPathThrowsExpectedException()
        {
            var path = @"../../NotADirectory/something.txt";
            reader.ReadDataFromFile(path);
        }
    }
}
