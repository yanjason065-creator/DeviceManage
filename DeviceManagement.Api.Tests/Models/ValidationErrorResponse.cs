using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManagement.Api.Tests.Models
{
    public class ValidationErrorResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public Dictionary<string, string[]> Data { get; set; }
    }
}
