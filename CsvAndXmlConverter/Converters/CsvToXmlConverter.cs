using CsvAndXmlConverter.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.Converters
{
    public class CsvToXmlConverter : IConverter
    {
        private readonly IStandardFileReader _fileReader;

        public void ConvertFile(string path)
        {
            throw new NotImplementedException();
        }
    }
}
