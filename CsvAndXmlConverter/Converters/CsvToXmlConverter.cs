using CsvAndXmlConverter.Data;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public IConversionResult ConvertFile(string path)
        {
            IEnumerable<string> fileData;
            try 
            {
                fileData = RetrieveDataFromFile(path);
            }
            catch (Exception exception)
            {
                return HandleExceptionFromReadingFile(exception, path);
            }

            return fileData.Count() > 0 ? PerformConversionOfFileData(fileData, path) :
                                          CreateResultForEmptyFileContent(fileData, path);
        }

        private IConversionResult PerformConversionOfFileData(IEnumerable<string> fileData, string path)
        {
            var convertedFilePath = CreatePathForConvertedFile(path);
            var convertedData = convertDataContent(fileData, Path.GetFileNameWithoutExtension(path));
            var result = WriteDataToFile(convertedData, convertedFilePath);
            return new ConversionResult { Success = true, ResultMessage = result };
        }

        private IConversionResult CreateResultForEmptyFileContent(IEnumerable<string> fileData, string path)
        {
            var message = string.Format(Resources.NotDataInFileToConvert, path);
            return new ConversionResult { Success = false, ResultMessage = message };
        }

        private IConversionResult HandleExceptionFromReadingFile(Exception exception, string path)
        {
            if(exception.GetType() == typeof(FileNotFoundException))
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

        private string convertDataContent(IEnumerable<string> fileData, string baseName)
        {
            var document = new XDocument();
            var root = new XElement(baseName + "s");
            var fieldNames = CreateIndexAndFeildDictionaryFromColumnLineOfCsvContent(fileData.First());
            var content = fileData.Skip(1).ToList();
            foreach (var item in content)
            {
                var itemElement = new XElement(baseName);
                var contentArray = item.Split(',');
                foreach (var field in fieldNames)
                {
                    itemElement.Add(new XElement(field.Value, contentArray[field.Key]));
                }
                root.Add(new XElement(itemElement));
            }
            document.Add(root);
            return document.ToString();
        }

        private IDictionary<int, string> CreateIndexAndFeildDictionaryFromColumnLineOfCsvContent(string columns)
        {
            var columnArray = columns.Split(',');
            return columnArray.ToDictionary(item => Array.IndexOf(columnArray, item), item => item.ToString());
        }
    }
}
