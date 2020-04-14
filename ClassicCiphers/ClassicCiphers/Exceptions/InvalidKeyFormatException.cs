using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassicCiphers.Exceptions
{
    class InvalidKeyFormatException : Exception
    {
        public InvalidKeyFormatException()
        {
        }

        public InvalidKeyFormatException(string message)
            : base(message)
        {
        }

        public InvalidKeyFormatException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
