using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Interfaces;
using IffleyRoutesRecord.Models.DTOs.Requests;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;
        private readonly IProblemCreator problemCreator;

        public PopulateDatabaseWithExistingProblems(IConfiguration configuration, IProblemCreator problemCreator, IffleyRoutesRecordContext repository)
        {
            this.problemCreator = problemCreator;
            this.configuration = configuration;
            this.repository = repository;
        }

        public void Populate()
        {
            string json = File.ReadAllText(@"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\ParsedExistingProblems.json");
            var existingProblems = JsonConvert.DeserializeObject<IEnumerable<ExistingProblem>>(json);

            foreach (var problem in existingProblems)
            {
                ValidateProblem(problem);
            }

            foreach (var problem in existingProblems)
            {
                StoreProblem(problem);
            }
        }

        private void StoreProblem(ExistingProblem problem)
        {
            var newProblem = new CreateProblemRequest()
            {
                Name = problem.Name,
                NewRules = new List<CreateProblemRuleRequest>(),
                ExistingRuleIds = new List<int>(),
                Holds = new List<CreateHoldOnProblemRequest>()
            };

            foreach (string rule in problem.Rules ?? new List<string>())
            {
                var existingRule = repository.GeneralRule.SingleOrDefault(generalRule => generalRule.Name == rule);

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

            foreach (var hold in problem.Holds)
            {
                var newHold = new CreateHoldOnProblemRequest()
                {
                    HoldId = repository.Hold.Single(h => h.Name == hold.Hold).Id,
                    IsStandingStartHold = hold.IsStandingStartHold,
                    ExistingHoldRuleIds = new List<int>(),
                    NewHoldRules = new List<CreateHoldRuleRequest>()
                };

                foreach (string rule in hold.Rules ?? new List<string>())
                {
                    var existingRule = repository.HoldRule.SingleOrDefault(holdRule => holdRule.Name == rule);

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

                newProblem.Holds.Add(newHold);
            }

            AddRulesToProblem(problem, newProblem);

            problemCreator.CreateProblem(newProblem);
        }

        private void AddRulesToProblem(ExistingProblem problem, CreateProblemRequest newProblem)
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

        private void ValidateProblem(ExistingProblem problem)
        {
            ValidateName(problem);
            ValidateGrades(problem);
            ValiadteHolds(problem);
        }

        private void ValiadteHolds(ExistingProblem problem)
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

        private void ValidateGrades(ExistingProblem problem)
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

        private void ValidateName(ExistingProblem problem)
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
