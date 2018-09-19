using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    public class EntityCreationException : Exception
    {
        public EntityCreationException()
        {

        }

        public EntityCreationException(string message) : base(message)
        {
        }

        public EntityCreationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
