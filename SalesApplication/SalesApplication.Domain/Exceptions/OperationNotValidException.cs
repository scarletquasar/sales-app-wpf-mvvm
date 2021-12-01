using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Domain.Exceptions
{
    public class OperationNotValidException : Exception
    {
        public OperationNotValidException()
        {
        }

        public OperationNotValidException(string message) : base(message)
        {
        }

        public OperationNotValidException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
