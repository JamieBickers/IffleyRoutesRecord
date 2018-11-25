using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Models.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IffleyRoutesRecord.Logic.Managers
{
    public class GlobalGradeAssigner : IGlobalGradeAssigner
    {
        private static readonly Dictionary<string, string> bGradeToTechGrade = new Dictionary<string, string>()
        {
            {"B0", "4b+" },
            {"B1", "5a+" },
            {"B2", "5b+" },
            {"B3", "5c" },
            {"B4", "6a" },
            {"B5", "6b-" },
            {"B6", "6b+" },
            {"B7", "6c-" },
            {"B8", "6c+" }
        };

        private static readonly Dictionary<string, string> poveyGradeToTechGrade = new Dictionary<string, string>()
        {
            {"Easy", "4b" },
            {"4b", "5b" },
            {"Hard", "6b-" }
        };

        private static readonly Dictionary<string, string> furlongGradeToTechGrade = new Dictionary<string, string>()
        {
            {"A", "4b" },
            {"AA", "4c" },
            {"AAA", "5b-" },
            {"XXX", "5c" },
            {"WTF", "6b" },
        };

        private readonly IGradeManager gradeManager;

        public GlobalGradeAssigner(IGradeManager gradeManager)
        {
            this.gradeManager = gradeManager;
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
                problem.GlobalGrade = techGrades.Single(grade => grade.Name == bGradeToTechGrade[bGrade]).Rank;
                return;
            }

            string poveyGrade = problem.PoveyGrade?.Name;
            if (poveyGrade != null)
            {
                problem.GlobalGrade = techGrades.Single(grade => grade.Name == poveyGradeToTechGrade[poveyGrade]).Rank;
                return;
            }

            string furlongGrade = problem.FurlongGrade?.Name;
            if (furlongGrade != null)
            {
                problem.GlobalGrade = techGrades.Single(grade => grade.Name == furlongGradeToTechGrade[furlongGrade]).Rank;
                return;
            }

            throw new CannotAssignGlobalGradeException();
        }

        public void AssignGlobalGrades(IEnumerable<ProblemResponse> problems)
        {
            if (problems is null)
            {
                throw new ArgumentNullException(nameof(problems));
            }

            foreach (var problem in problems)
            {
                AssignGlobalGrade(problem);
            }
        }
    }
}
