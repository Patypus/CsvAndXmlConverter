using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverter.Validator
{
    public class XmlValidator : IXmlValidator
    {
        private List<string> validationMessages = new List<string>();
        private List<string> expectedPropertyElements;
        

        public Tuple<bool, string[]> ValidateXml(XDocument document)
        {
            expectedPropertyElements = PopulateExpectedPropertyElementsFromFirstChildElement(document.Root);
            return new Tuple<bool, string[]>(validationMessages.Count == 0, validationMessages.ToArray()); 
        }

        private List<string> PopulateExpectedPropertyElementsFromFirstChildElement(XElement rootElement)
        {
            var firstChild = rootElement.Elements().First();
            return firstChild.Elements().Select(element => element.Name.ToString()).ToList();
        }
    }
}
