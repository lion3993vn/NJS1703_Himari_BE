using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Exceptions
{
    public class IsRequireDataException : Exception
    {
        public IsRequireDataException(string message) : base(message) { }

        public IsRequireDataException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; } = string.Empty;
    }
}
