using CsvAndXmlConverter.Properties;
using CsvAndXmlConverter.Validator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverterTests.Validator
{
    [TestClass]
    public class XmlValidatorTests
    {
        [TestMethod]
        public void TestValidXmlPassesValidation()
        {
            var validator = new XmlValidator();
            var document = LoadXmlDocument(@"../../Validator/XmlTestData/ValidXmlFile.xml");
            var result = validator.ValidateXml(document);
            Assert.IsTrue(result.Item1);
            Assert.AreEqual(0, result.Item2.Count());
        }

        [TestMethod]
        public void TestXDocumentWithNoDataElementIsReportedAsAfailure()
        {
            var validator = new XmlValidator();
            var document = LoadXmlDocument(@"../../Validator/XmlTestData/NoData.xml");
            var result = validator.ValidateXml(document);
            Assert.IsFalse(result.Item1);
            Assert.AreEqual(Resources.NoDataElementsInDocument, result.Item2.First());
        }

        [TestMethod]
        public void TestDataElementWithInconsistentChildElementsIsReportedAsAFailure()
        {
            var validator = new XmlValidator();
            var document = LoadXmlDocument(@"../../Validator/XmlTestData/InconsistentChildElements.xml");
            var expectedElement = string.Format(Resources.InconsistentElementsMessage,
                                                "2",
                                                "name",
                                                "Wigan");
            var result = validator.ValidateXml(document);
            Assert.IsFalse(result.Item1);
            Assert.AreEqual(expectedElement, result.Item2[0]);
        }

        private XDocument LoadXmlDocument(string path)
        {
            return XDocument.Load(path);
        }
    }
}
