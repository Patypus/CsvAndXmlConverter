using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.IO
{
    public interface IFileWriter
    {
        /// <summary>
        /// Method to write a steam of data to a file specified
        /// </summary>
        /// <param name="data">Stream of data to save</param>
        /// <param name="filePath">String path to save the file to</param>
        /// <returns>String status to indicate the result of the save.</returns>
        string SaveDataToFile(MemoryStream data, string filePath);
    }
}
