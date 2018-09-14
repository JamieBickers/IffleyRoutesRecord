using System;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    public class EntityWithNameAlreadyExistsException : Exception
    {
        public EntityWithNameAlreadyExistsException()
        {

        }

        public EntityWithNameAlreadyExistsException(string message) : base(message)
        {
        }

        public EntityWithNameAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
