using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Exceptions
{
    public class DefaultException : Exception
    {
        public DefaultException(string message) : base(message) { }

        public DefaultException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; } = string.Empty;
    }
}
