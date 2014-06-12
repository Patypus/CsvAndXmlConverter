using CsvAndXmlConverter.Data;
using CsvAndXmlConverter.IO;
using CsvAndXmlConverter.Properties;
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
            
            var convertedFilePath = CreatePathForConvertedFile(path);
            WriteDataToFile(fileData.ToString(), convertedFilePath);
            return null;
        }

        private IConversionResult HandleExceptionFromReadingFile(Exception exception, string path)
        {
            if(exception.GetType() == typeof(FileNotFoundException))
            {
                var message = string.Format(Resources.FileNotFoundMessage, path);
                return new ConversionResult { Result = false, ResultMessage = message };
            }
            else if (exception.GetType() == typeof(DirectoryNotFoundException))
            {
                var message = string.Format(Resources.DirectoryNotFoundMessage, Path.GetPathRoot(path));
                return new ConversionResult { Result = false, ResultMessage = message };
            }
            else
            {
                var message = string.Format(Resources.GenericUnableToOpenFile, exception.Message);
                return new ConversionResult { Result = false, ResultMessage = message };
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
    }
}
