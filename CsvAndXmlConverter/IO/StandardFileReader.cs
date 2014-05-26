using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.IO
{
    public class StandardFileReader : IStandardFileReader
    {
        public IEnumerable<string> ReadDataFromFile(string path)
        {
            return File.ReadAllLines(path);
        }
    }
}
