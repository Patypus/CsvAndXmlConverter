using CsvAndXmlConverter.Properties;
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
            var rootElement = document.Root;
            if (rootElement.Elements().Count() != 0)
            {
                expectedPropertyElements = PopulateExpectedPropertyElementsFromFirstChildElement(rootElement);
                CheckChildElementNameConsistency(rootElement.Elements().ToList());
            }
            else
            {
                validationMessages.Add(Resources.NoDataElementsInDocument);
            }
            
            return new Tuple<bool, string[]>(validationMessages.Count == 0, validationMessages.ToArray()); 
        }

        private List<string> PopulateExpectedPropertyElementsFromFirstChildElement(XElement rootElement)
        {
            var firstChild = rootElement.Elements().First();
            return firstChild.Elements().Select(element => element.Name.ToString()).ToList();
        }

        private void CheckChildElementNameConsistency(IList<XElement> dataElements)
        {
            foreach (var element in dataElements)
            {
                var childElementNames = element.Elements().Select(x => x.Name.ToString()).ToList();
                if (!childElementNames.SequenceEqual(expectedPropertyElements))
                {
                    var firstElement =  element.Elements().First();
                    validationMessages.Add(string.Format(Resources.InconsistentElementsMessage,
                                                         dataElements.IndexOf(element) + 1,
                                                         firstElement.Name,
                                                         firstElement.Value));
                }
            }
        }
    }
}
