using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverter.Validator
{
    public interface IXmlValidator
    {
        /// <summary>
        /// Method to validate an Xml Document to ensure that it is valid for conversion before attempting conversion.
        /// The success of the validation is idicated by the boolean part of the tuple and the string collection in the 
        /// tuple contians details of validation failures (empty otherwise)
        /// </summary>
        /// <param name="document">xml document to validate</param>
        /// <returns>tuple of boolean success of validation and array of string validation failures</returns>
        Tuple<bool, string[]> ValidateXml(XDocument document);
    }
}
