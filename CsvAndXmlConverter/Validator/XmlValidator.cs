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
        public Tuple<bool, string[]> ValidateXml(XDocument document)
        {
            return new Tuple<bool, string[]>( false, new [] { "", "" } ); 
        }
    }
}
