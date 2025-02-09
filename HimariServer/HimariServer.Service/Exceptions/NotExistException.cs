using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Exceptions
{
    public class NotExistException : Exception
    {
        public NotExistException(string message) : base(message) { }

        public NotExistException(string message, string errorCode) : base(message)
        {
            ErrorCode = errorCode;
        }

        public string ErrorCode { get; set; } = string.Empty;
    }
}
