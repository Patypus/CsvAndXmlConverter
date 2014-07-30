using CsvAndXmlConverter.Data;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using CsvAndXmlConverter.Validator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CsvAndXmlConverter.Converters
{
    public class XmlToCsvConverter : IConverter
    {
        private readonly IFileWriter _writer;
        private readonly IXMLFileReader _reader;
        private readonly IXmlValidator _validator;

        public XmlToCsvConverter(IFileWriter writer, IXMLFileReader reader, IXmlValidator validator)
        {
            _writer = writer;
            _reader = reader;
            _validator = validator;
        }

        public IConversionResult ConvertFile(string path)
        {
            XDocument documentToConvert;
            try
            {
                documentToConvert = _reader.ReadDataFromFile(path);
            }
            catch(Exception exception)
            {
                return HandleExceptionFromReadingFile(exception, path);
            }

            var validationResult = _validator.ValidateXml(documentToConvert);

            return validationResult.Item1 ?  PerfromConversionForDataDocument(documentToConvert, path) :
                                             ReportValidationFailure(validationResult);
        }

        private IConversionResult ReportValidationFailure(Tuple<bool, string[]> validationResult)
        {
            return new ConversionResult 
                       { 
                           Success = validationResult.Item1, 
                           ResultMessage = CreateSingleStringFromArrayOfValidationFailures(validationResult.Item2) 
                       };
        }

        private string CreateSingleStringFromArrayOfValidationFailures(string[] failures)
        {
            var introMessage = Resources.ValidationErrorsEncountered + Environment.NewLine;
            return introMessage + string.Join(Environment.NewLine, failures);
        }

        private IConversionResult PerfromConversionForDataDocument(XDocument documentToConvert, string path)
        {
            var convertedContent = ConvertXmlContentToCSV(documentToConvert);
            var writeResult = WriteDataToFile(convertedContent, CreatePathForConvertedFile(path));
            return new ConversionResult { Success = true, ResultMessage = writeResult };
        }

        private string ConvertXmlContentToCSV(XDocument content)
        {
            var resultBuilder = new StringBuilder();
            resultBuilder.Append(CreateColumnTitlesRow(content));
            var childElements = content.Root.Elements();
            foreach (var element in childElements)
            {
                resultBuilder.Append(Environment.NewLine + MakeRowFromValuesInElementCollection(element.Elements()));
            }
            return resultBuilder.ToString();
        }

        private string MakeRowFromValuesInElementCollection(IEnumerable<XElement> elements)
        {
            var elementValuesCollection = elements.Select(element => element.Value.ToString());
            return string.Join(",", elementValuesCollection);
        }

        private string CreateColumnTitlesRow(XDocument document)
        {
            var resultBuilder = new StringBuilder();
            var singleItemElement = document.Root.Elements().First();
            var propertyElementTitles = singleItemElement.Elements().Select(element => element.Name.ToString());
            return string.Join(",", propertyElementTitles);
        }

        private string WriteDataToFile(string data, string path)
        {
            return _writer.SaveDataToFile(TranscribeDataStringToStream(data), path);
        }

        private MemoryStream TranscribeDataStringToStream(string data)
        {
            var dataStream = new MemoryStream();
            var dataWriter = new StreamWriter(dataStream);
            dataWriter.Write(data);
            dataWriter.Flush();
            dataStream.Position = 0;
            return dataStream;
        }

        private string CreatePathForConvertedFile(string originalPath)
        {
            var filePath = Path.GetDirectoryName(originalPath);
            var newFileName = "converted_" + Path.GetFileNameWithoutExtension(originalPath) + ".csv";
            return Path.Combine(filePath, newFileName);
        }

        private IConversionResult HandleExceptionFromReadingFile(Exception exception, string path)
        {
            if (exception.GetType() == typeof(FileNotFoundException))
            {
                var message = string.Format(Resources.FileNotFoundMessage, path);
                return new ConversionResult { Success = false, ResultMessage = message };
            }
            else if (exception.GetType() == typeof(DirectoryNotFoundException))
            {
                var message = string.Format(Resources.DirectoryNotFoundMessage, Path.GetPathRoot(path));
                return new ConversionResult { Success = false, ResultMessage = message };
            }
            else
            {
                var message = string.Format(Resources.GenericUnableToOpenFile, exception.Message);
                return new ConversionResult { Success = false, ResultMessage = message };
            }
        }
    }
}
