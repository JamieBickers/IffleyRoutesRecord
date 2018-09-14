using System;

namespace IffleyRoutesRecord.IntegrationTests
{
    public class ConditionNotSatisfiedException : Exception
    {
        public ConditionNotSatisfiedException()
        {

        }

        public ConditionNotSatisfiedException(string message) : base(message)
        {
        }

        public ConditionNotSatisfiedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
