using CsvAndXmlConverter.IO;
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
    public class StandardFileReaderTests
    {
        private static IStandardFileReader reader;

        [TestFixtureSetUp]
        public void Setup()
        {
            reader = new StandardFileReader();
        }

        [Test]
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
        [Test]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestIncorrectFileNameThrowsExpectedException()
        {
            var path = @"../../IO/Data/NotReallyAfile.txt";
            reader.ReadDataFromFile(path);
        }

        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestInvalidPathThrowsExpectedException()
        {
            var path = @"../../NotADirectory/something.txt";
            reader.ReadDataFromFile(path);
        }
    }
}
