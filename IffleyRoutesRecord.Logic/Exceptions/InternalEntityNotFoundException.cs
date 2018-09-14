using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    public class InternalEntityNotFoundException : Exception
    {
        public InternalEntityNotFoundException()
        {

        }

        public InternalEntityNotFoundException(string message) : base(message)
        {
        }

        public InternalEntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
