using CsvAndXmlConverter.IO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverterTests.IO
{
    [TestFixture]
    public class XMLFileReaderTests
    {
        [Test]
        public void TestReaderReturnsContentFromValidFile()
        {
            var expectedContent = CreateExpectedDocument();
            var path = @"../../IO/Data/TestXmlFile.xml";
            var result = (new XMLFileReader()).ReadDataFromFile(path);
            Assert.AreEqual(expectedContent.ToString(), result.ToString());
        }

        private XDocument CreateExpectedDocument()
        {
            var document = new XDocument();
            var rootElement = new XElement("root");
            var childElement = new XElement("child");
            var element1 = new XElement("item1");
            var element2 = new XElement("item2");
            element1.Value = "value1";
            element2.Value = "value2";
            childElement.Add(element1);
            childElement.Add(element2);
            rootElement.Add(childElement);
            document.Add(rootElement);
            return document;
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
            var path = @"../../IO/Data/FakeFile.xml";
            (new XMLFileReader()).ReadDataFromFile(path);
        }

        [Test]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestInvalidPathThrowsExpectedException()
        {
            var path = @"../../../No/File/Here/oops.xml";
            (new XMLFileReader()).ReadDataFromFile(path);
        }
    }
}