using System.Collections.Generic;
using IffleyRoutesRecord.Models.DTOs.Responses;

namespace IffleyRoutesRecord.Logic.Interfaces
{
    public interface IGlobalGradeAssigner
    {
        /// <summary>
        /// Computes and assigns the global grade from the problem. This is based on tech grades.
        /// </summary>
        /// <param name="problem">The problem to compute the global grade for</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CannotAssignGlobalGradeException"></exception>
        void AssignGlobalGrade(ProblemResponse problem);

        /// <summary>
        /// Computes and assigns the global grades for a list of problems.
        /// </summary>
        /// <param name="problems"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="CannotAssignGlobalGradeException"></exception>
        void AssignGlobalGrades(IEnumerable<ProblemResponse> problems);
    }
}