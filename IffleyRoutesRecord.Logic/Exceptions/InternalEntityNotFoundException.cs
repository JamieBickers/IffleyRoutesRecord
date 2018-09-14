using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    /// <summary>
    /// The exception thrown when an entity is not found due to unexpected code error
    /// </summary>
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
