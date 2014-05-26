using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.IO
{
    public interface IStandardFileReader
    {
        IEnumerable<string> ReadDataFromFile(string path);
    }
}
