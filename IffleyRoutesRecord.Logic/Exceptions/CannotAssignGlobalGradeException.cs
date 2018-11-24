using System;
using System.Collections.Generic;
using System.Text;

namespace IffleyRoutesRecord.Logic.Exceptions
{
    public class CannotAssignGlobalGradeException : Exception
    {
        public CannotAssignGlobalGradeException(string message) : base(message)
        {
        }

        public CannotAssignGlobalGradeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CannotAssignGlobalGradeException()
        {
        }
    }
}
