using System;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    /// <summary>
    /// The exception thrown when an entity is not found in the database due to user error
    /// </summary>
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {

        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
