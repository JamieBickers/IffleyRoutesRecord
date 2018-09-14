using System;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    /// <summary>
    /// The exception thrown when the name being used already exists in the database
    /// </summary>
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
