using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvAndXmlConverter.Data
{
    public class ConversionResult : IConversionResult
    {
        public bool Success { get; set; }

        public string ResultMessage { get; set; }
    }
}
