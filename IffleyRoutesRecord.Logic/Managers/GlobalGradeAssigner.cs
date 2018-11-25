using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class GlobalGradeAssigner : IGlobalGradeAssigner
    {
        private const string GlobalGradesConfigSection = "GlobalGrades";

        private readonly IGradeManager gradeManager;
        private readonly IConfiguration configuration;

        public GlobalGradeAssigner(IGradeManager gradeManager, IConfiguration configuration)
        {
            this.gradeManager = gradeManager;
            this.configuration = configuration;
        }

        public void AssignGlobalGrade(ProblemResponse problem)
        {
            if (problem is null)
            {
                throw new ArgumentNullException(nameof(problem));
            }

            if (problem.TechGrade != null)
            {
                problem.GlobalGrade = problem.TechGrade.Rank;
                return;
            }

            var techGrades = gradeManager.GetTechGrades();

            string bGrade = problem.BGrade?.Name;
            if (bGrade != null)
            {
                problem.GlobalGrade = techGrades
                    .Single(grade => grade.Name == configuration[$"{GlobalGradesConfigSection}:BGrades:{bGrade}"]).Rank;
                return;
            }

            string poveyGrade = problem.PoveyGrade?.Name;
            if (poveyGrade != null)
            {
                problem.GlobalGrade = techGrades
                    .Single(grade => grade.Name == configuration[$"{GlobalGradesConfigSection}:PoveyGrades:{poveyGrade}"]).Rank;
                return;
            }

            string furlongGrade = problem.FurlongGrade?.Name;
            if (furlongGrade != null)
            {
                problem.GlobalGrade = techGrades
                    .Single(grade => grade.Name == configuration[$"{GlobalGradesConfigSection}:FurlongGrades:{furlongGrade}"]).Rank;
                return;
            }

            throw new CannotAssignGlobalGradeException();
        }

        //Use ICollection not IEnumerable so changes are not discarded
        public void AssignGlobalGrades(IEnumerable<ProblemResponse> problems)
        {
            if (problems is null)
            {
                throw new ArgumentNullException(nameof(problems));
            }

            foreach (var problem in problems.ToList())
            {
                AssignGlobalGrade(problem);
            }
        }
    }
}
