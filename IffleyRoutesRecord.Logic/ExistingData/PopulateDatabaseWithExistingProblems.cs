using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData.Models;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Logic.StaticHelpers;
using IffleyRoutesRecord.Models.DTOs.Requests;
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

            foreach (var problem in existingProblems)
            {
                StoreProblem(problem, validate);
            }

            repository.SaveChanges();
        }

        private void StoreProblem(ExistingProblem problem, bool validate)
        {
            var newProblem = new CreateProblemRequest()
            {
                Name = problem.Name,
                NewRules = new List<CreateProblemRuleRequest>(),
                ExistingRuleIds = new List<int>(),
                Holds = new List<CreateHoldOnProblemRequest>()
            };

            AddRules(problem, newProblem);
            AddHolds(problem, newProblem);
            AddGrades(problem, newProblem);

            if (validate)
            {
                validator.Validate(newProblem);
            }

            var problemDbo = Mapper.Map(newProblem);
            repository.Problem.Add(problemDbo);
        }

        private void AddHolds(ExistingProblem problem, CreateProblemRequest newProblem)
        {
            foreach (var hold in problem.Holds)
            {
                var newHold = new CreateHoldOnProblemRequest()
                {
                    HoldId = repository.Hold.Single(h => h.Name == hold.Hold).Id,
                    IsStandingStartHold = hold.IsStandingStartHold,
                    ExistingHoldRuleIds = new List<int>(),
                    NewHoldRules = new List<CreateHoldRuleRequest>()
                };

                AddHoldRules(hold, newHold);

                newProblem.Holds.Add(newHold);
            }
        }

        private void AddHoldRules(ExistingProblemHold hold, CreateHoldOnProblemRequest newHold)
        {
            foreach (string rule in hold.Rules ?? new List<string>())
            {
                var existingRule = repository.HoldRule.SingleOrDefault(holdRule => holdRule.Name == rule);

                if (existingRule is null)
                {
                    existingRule = repository.HoldRule.Local.SingleOrDefault(holdRule => holdRule.Name == rule);
                }

                if (existingRule is null)
                {
                    newHold.NewHoldRules = newHold.NewHoldRules.Concat(new List<CreateHoldRuleRequest>()
                        {
                            new CreateHoldRuleRequest()
                            {
                                Name = rule
                            }
                        });
                }
                else
                {
                    newHold.ExistingHoldRuleIds = newHold.ExistingHoldRuleIds.Concat(new List<int>() { existingRule.Id });
                }
            }
        }

        private void AddRules(ExistingProblem problem, CreateProblemRequest newProblem)
        {
            foreach (string rule in problem.Rules ?? new List<string>())
            {
                var existingRule = repository.GeneralRule.SingleOrDefault(generalRule => generalRule.Name == rule);

                if (existingRule is null)
                {
                    existingRule = repository.GeneralRule.Local.SingleOrDefault(generalRule => generalRule.Name == rule);
                }

                if (existingRule is null)
                {
                    newProblem.NewRules = newProblem.NewRules.Concat(new List<CreateProblemRuleRequest>()
                    {
                        new CreateProblemRuleRequest()
                        {
                            Name = rule
                        }
                    });
                }
                else
                {
                    newProblem.ExistingRuleIds = newProblem.ExistingRuleIds.Concat(new List<int>() { existingRule.Id });
                }
            }
        }

        private void AddGrades(ExistingProblem problem, CreateProblemRequest newProblem)
        {
            foreach (string grade in problem.Grades)
            {
                if (Regex.IsMatch(grade, "^[4-6][a-c][+-]?$"))
                {
                    if (newProblem.TechGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.TechGradeId = repository.TechGrade.Single(techGrade => techGrade.Name == grade).Id;
                }
                else if (Regex.IsMatch(grade, "^B[0-8]$"))
                {
                    if (newProblem.BGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.BGradeId = repository.BGrade.Single(bGrade => bGrade.Name == grade).Id;
                }
                else if (Regex.IsMatch(grade, "^(AAA)|(XXX)$"))
                {
                    if (newProblem.FurlongGradeId != null)
                    {
                        throw new Exception();
                    }

                    newProblem.FurlongGradeId = repository.FurlongGrade.Single(furlongGrade => furlongGrade.Name == grade).Id;
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
