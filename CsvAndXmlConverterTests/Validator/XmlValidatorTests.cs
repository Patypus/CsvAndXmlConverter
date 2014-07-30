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
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestXDocumentWithNoDataIsReportedAsAfailure()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestDataItemWithNoChildElementsIsReportedAsAFailure()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestDataElementWithInconsistentChildElementsIsReportedAsAFailure()
        {
            Assert.IsFalse(true);
        }

        [TestMethod]
        public void TestDataItemWithNoChildElementsIsReportedAsAFailure()
        {
            Assert.IsFalse(true);
        }
    }
}
