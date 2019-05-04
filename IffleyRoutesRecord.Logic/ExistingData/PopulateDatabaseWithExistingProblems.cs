using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.ExistingData.Models;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
using IffleyRoutesRecord.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace IffleyRoutesRecord.Logic.ExistingData
{
    public class PopulateDatabaseWithExistingProblems
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly string existingDataPath;
        private readonly IProblemRequestValidator validator;

        public PopulateDatabaseWithExistingProblems(IffleyRoutesRecordContext repository, string existingDataPath, IProblemRequestValidator validator)
        {
            this.repository = repository;
            this.existingDataPath = existingDataPath;
            this.validator = validator;
        }

        public void Populate(bool validate)
        {
            string json = File.ReadAllText(Path.ChangeExtension(Path.Combine(existingDataPath, "ExistingProblems"), "json"));
            var existingProblems = JsonConvert.DeserializeObject<IEnumerable<ExistingProblem>>(json);

            if (validate)
            {
                foreach (var problem in existingProblems)
                {
                    ValidateProblem(problem);
                }
            }

            AddRulesToDatabase(existingProblems);

            StoreProblems(existingProblems, validate);
            repository.SaveChanges();
        }

        private void AddRulesToDatabase(IEnumerable<ExistingProblem> existingProblems)
        {
            if (repository.GeneralRule is null || repository.HoldRule is null)
            {
                throw new DatabaseException();
            }

            var rules = existingProblems
                .Where(problem => problem.Rules != null)
                .SelectMany(problem => problem.Rules)
                .Distinct()
                .Select(rule => new GeneralRule()
                {
                    Name = rule
                });

            repository.GeneralRule.AddRange(rules);

            var holdRules = existingProblems
                .SelectMany(problem => problem.Holds.Where(hold => hold.Rules != null).SelectMany(hold => hold.Rules))
                .Distinct()
                .Select(holdRule => new HoldRule()
                {
                    Name = holdRule
                });

            repository.HoldRule.AddRange(holdRules);
        }

        private void StoreProblems(IEnumerable<ExistingProblem> problems, bool validate)
        {
            if (repository.Problem is null || repository.HoldRule is null)
            {
                throw new DatabaseException();
            }

            var holds = repository.Hold.ToList();
            var holdRules = repository.HoldRule.Local.ToList();
            var techGrades = repository.TechGrade.ToList();
            var bGrades = repository.BGrade.ToList();
            var furlongGrades = repository.FurlongGrade.ToList();

            var newProblems = new List<CreateProblemRequest>();
            foreach (var problem in problems)
            {
                var newProblem = new CreateProblemRequest()
                {
                    Name = problem.Name,
                    NewRules = new List<CreateProblemRuleRequest>(),
                    ExistingRuleIds = new List<int>(),
                    Holds = new List<CreateHoldOnProblemRequest>()
                };

                AddRules(problem, newProblem);
                AddHolds(problem, newProblem, holds, holdRules);
                AddGrades(problem, newProblem, techGrades, bGrades, furlongGrades);

                if (validate)
                {
                    validator.Validate(newProblem);
                }

                newProblems.Add(newProblem);
            }

            var problemDbos = newProblems.Select(Mapper.Map);
            repository.Problem.AddRange(problemDbos);
        }

        private void AddHolds(ExistingProblem problem, CreateProblemRequest newProblem,
            IEnumerable<Hold> existingHolds, IEnumerable<HoldRule> existingHoldRules)
        {
            if (problem.Holds is null)
            {
                throw new ArgumentException("The problem must have holds.", nameof(problem));
            }

            if (newProblem.Holds is null)
            {
                throw new ArgumentException("The problem must have holds.", nameof(newProblem));
            }

            foreach (var hold in problem.Holds)
            {
                var newHold = new CreateHoldOnProblemRequest()
                {
                    HoldId = existingHolds.First(existingHold => existingHold.Name == hold.Hold).Id,
                    IsStandingStartHold = hold.IsStandingStartHold,
                    ExistingHoldRuleIds = new List<int>(),
                    NewHoldRules = new List<CreateHoldRuleRequest>()
                };

                AddHoldRules(hold, newHold, existingHoldRules);
                newProblem.Holds.Add(newHold);
            }
        }

        private void AddHoldRules(ExistingProblemHold hold, CreateHoldOnProblemRequest newHold, IEnumerable<HoldRule> existingHoldRules)
        {
            foreach (string rule in hold.Rules ?? new List<string>())
            {
                var existingRule = existingHoldRules.First(holdRule => holdRule.Name == rule);
                newHold.ExistingHoldRuleIds = newHold.ExistingHoldRuleIds.Concat(new List<int>() { existingRule.Id });
            }
        }

        private void AddRules(ExistingProblem problem, CreateProblemRequest newProblem)
        {
            if (repository.GeneralRule is null)
            {
                throw new DatabaseException();
            }

            foreach (string rule in problem.Rules ?? new List<string>())
            {
                var existingRule = repository.GeneralRule.Local.First(generalRule => generalRule.Name == rule);
                newProblem.ExistingRuleIds = newProblem.ExistingRuleIds.Concat(new List<int>() { existingRule.Id });
            }
        }

        private static void AddGrades(ExistingProblem problem, CreateProblemRequest newProblem, IEnumerable<TechGrade> techGrades,
            IEnumerable<BGrade> bGrades, IEnumerable<FurlongGrade> furlongGrades)
        {
            if (problem.Grades is null)
            {
                throw new ArgumentException("The problem must have grades.", nameof(problem));
            }

            foreach (string grade in problem.Grades)
            {
                if (Regex.IsMatch(grade, "^[4-6][a-c][+-]?$"))
                {
                    if (newProblem.TechGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.TechGradeId = techGrades.First(techGrade => techGrade.Name == grade).Id;
                }
                else if (Regex.IsMatch(grade, "^B[0-8]$"))
                {
                    if (newProblem.BGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.BGradeId = bGrades.First(bGrade => bGrade.Name == grade).Id;
                }
                else if (Regex.IsMatch(grade, "^(AAA)|(XXX)$"))
                {
                    if (newProblem.FurlongGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.FurlongGradeId = furlongGrades.First(furlongGrade => furlongGrade.Name == grade).Id;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private static void ValidateProblem(ExistingProblem problem)
        {
            ValidateName(problem);
            ValidateGrades(problem);
            ValiadteHolds(problem);
        }

        private static void ValiadteHolds(ExistingProblem problem)
        {
            if (problem.Holds is null)
            {
                throw new ArgumentException("The problem must have holds.", nameof(problem));
            }

            var validHoldRegexes = new List<string>()
            {
                "[0-9]{1,3}[A-E]?",
                "any",
                "floor",
                "Arete",
                "Girder",
                "balcony"
            };

            var validHoldRuleRegex = new List<string>()
            {
                "optional",
                "feet only",
                "no undercut",
                "hands only",
                "slope only",
                "top right",
                "top",
                "match",
                "undercut only"
            };

            foreach (var hold in problem.Holds)
            {
                if (hold.Rules is null)
                {
                    continue;
                }

                foreach (string rule in hold.Rules)
                {
                    if (!validHoldRuleRegex.Any(regex => Regex.IsMatch(rule, regex)))
                    {
                        throw new Exception();
                    }
                }

                if (!validHoldRegexes.Any(regex => Regex.IsMatch(hold.Hold, regex)))
                {
                    throw new Exception();
                }
            }
        }

        private static void ValidateGrades(ExistingProblem problem)
        {
            if (problem.Grades is null)
            {
                throw new ArgumentException("The problem must have grades.", nameof(problem));
            }

            var validGradeRegexes = new List<string>()
            {
                "AAA",
                "[4-6][a-c][+-]?",
                "B[0-9]",
                "XXX"
            };

            foreach (string grade in problem.Grades)
            {
                if (!validGradeRegexes.Any(regex => Regex.IsMatch(grade, regex)))
                {
                    throw new Exception();
                }
            }
        }

        private static void ValidateName(ExistingProblem problem)
        {
            if (problem.Name is null)
            {
                throw new ArgumentException("The problem must have a name.", nameof(problem));
            }

            if (problem.Name == "“4b or not …”" || problem.Name == "While Inventing a Nice 4b")
            {
                return;
            }

            string[] words = problem.Name.Split(' ');

            foreach (string word in words)
            {
                if (word == "^AAA$" || Regex.IsMatch(word, "^[4-6][a-c][+-]?$") || Regex.IsMatch(word, "^B[0-9]$"))
                {
                    throw new Exception();
                }
            }
        }
    }
}
