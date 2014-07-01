using CsvAndXmlConverter.Data;
using CsvAndXmlConverter.IO;
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

        public XmlToCsvConverter(IFileWriter writer, IXMLFileReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public IConversionResult ConvertFile(string path)
        {
            var documentToConvert = _reader.ReadDataFromFile(path);
            var convertedContent = ConvertXmlContentToCSV(documentToConvert);
            var writeResult = WriteDataToFile(convertedContent, CreatePathForConvertedFile(path));
            return new ConversionResult { Success = true, ResultMessage = "success" };
        }

        private string ConvertXmlContentToCSV(XDocument content)
        {
            //TODO - add the conversion logic in here.
            return content.ToString();
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
    }
}
