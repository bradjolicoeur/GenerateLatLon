using System;
using System.Collections.Generic;
using System.Text;

namespace GenerateLatLonFunction.Models
{
    public class FunctionResponse
    {
        public bool Success { get; set; }
        public int? Rows { get; set; }
        public string Message { get; set; }
    }
}
