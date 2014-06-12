using CsvAndXmlConverter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.Converters
{
    public interface IConverter
    {
        IConversionResult ConvertFile(string path);
    }
}
