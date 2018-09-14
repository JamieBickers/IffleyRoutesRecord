using System;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class NotEqualException : Exception
    {
        public NotEqualException()
        {

        }

        public NotEqualException(string message) : base(message)
        {
        }

        public NotEqualException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
