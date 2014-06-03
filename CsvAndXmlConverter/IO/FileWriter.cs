using CsvAndXmlConverter.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.IO
{
    public class FileWriter : IFileWriter
    {
        public string SaveDataToFile(MemoryStream data, string filePath)
        {
            try
            {
                return PerformSave(data, filePath);
            }
            catch (DirectoryNotFoundException)
            {
                return string.Format(Resources.UnableToSaveToNoExistantDirectory, Path.GetDirectoryName(filePath));
            }
            catch (UnauthorizedAccessException)
            {
                return string.Format(Resources.UnableToSaveFilePermissionDenied, Path.GetDirectoryName(filePath));
            }
        }

        private string PerformSave(MemoryStream data, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                data.WriteTo(fileStream);
            }
            return string.Format(Resources.FileCreated, filePath);
        }
    }
}
