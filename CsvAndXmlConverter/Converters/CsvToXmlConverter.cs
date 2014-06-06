using CsvAndXmlConverter.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.Converters
{
    public class CsvToXmlConverter : IConverter
    {
        private readonly IStandardFileReader _fileReader;
        private readonly IFileWriter _fileWriter;

        public CsvToXmlConverter(IStandardFileReader reader, IFileWriter writer)
        {
            _fileReader = reader;
            _fileWriter = writer;
        }

        public void ConvertFile(string path)
        {
            var fileData = RetrieveDataFromFile(path);
            var convertedFilePath = CreatePathForConvertedFile(path);
            WriteDataToFile(fileData.ToString(), convertedFilePath);
        }

        private IEnumerable<string> RetrieveDataFromFile(string path)
        {
            return _fileReader.ReadDataFromFile(path);
        }

        private string CreatePathForConvertedFile(string originalPath)
        {
            var filePath = Path.GetDirectoryName(originalPath);
            var newFileName = "converted_" + Path.GetFileNameWithoutExtension(originalPath) + ".xml";
            return Path.Combine(filePath, newFileName);
        }

        private string WriteDataToFile(string data, string path)
        {
            var dataStream = TranscribeDataStringToStream(data);
            return _fileWriter.SaveDataToFile(dataStream, path);
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
    }
}
