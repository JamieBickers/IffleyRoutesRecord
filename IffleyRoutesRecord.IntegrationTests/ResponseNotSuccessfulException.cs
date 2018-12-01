using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class ResponseNotSuccessfulException : Exception
    {
        public ResponseNotSuccessfulException(string message) : base(message)
        {
        }

        public ResponseNotSuccessfulException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ResponseNotSuccessfulException()
        {
        }
    }
}
