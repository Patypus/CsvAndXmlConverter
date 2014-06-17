using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.Data
{
    public interface IConversionResult
    {
        bool Success { get; set; }

        string ResultMessage { get; set; }
    }
}
