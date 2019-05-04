using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.Exceptions;
using IffleyRoutesRecord.Logic.ExistingData.Models;
using IffleyRoutesRecord.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace IffleyRoutesRecord.Logic.ExistingData
{
    public class PopulateDatabaseWithStaticData
    {
        private readonly IffleyRoutesRecordContext repository;
        private readonly string existingDataPath;

        public PopulateDatabaseWithStaticData(IffleyRoutesRecordContext repository, string existingDataPath)
        {
            this.repository = repository;
            this.existingDataPath = existingDataPath;
        }

        public void Populate()
        {
            PopulateGrades();
            PopulateStyleSymbols();
            PopulateHolds();
        }

        private void PopulateHolds()
        {
            if (repository.Hold == null)
            {
                throw new DatabaseException();
            }

            var holds = GetListOfEntities<ExistingHold>("Holds");

            foreach (var hold in holds)
            {
                repository.Hold.Add(new Hold()
                {
                    Id = hold.Id,
                    Name = hold.Name,
                    ParentHoldId = hold.ParentHoldId
                });
            }

            repository.SaveChanges();
        }

        private void PopulateStyleSymbols()
        {
            if (repository.StyleSymbol == null)
            {
                throw new DatabaseException();
            }

            var styleSymbols = GetListOfEntities<ExistingStyleSymbol>("StyleSymbols");

            foreach (var styleSymbol in styleSymbols)
            {
                repository.StyleSymbol.Add(new StyleSymbol()
                {
                    Id = styleSymbol.Id,
                    Name = styleSymbol.Name,
                    Description = styleSymbol.Description
                });
            }

            repository.SaveChanges();
        }

        private void PopulateGrades()
        {
            if (repository.TechGrade == null || repository.BGrade == null || repository.PoveyGrade == null || repository.FurlongGrade == null)
            {
                throw new DatabaseException();
            }

            var techGrades = GetListOfEntities<ExistingGrade>("TechGrades");

            foreach (var grade in techGrades)
            {
                repository.TechGrade.Add(new TechGrade()
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Rank = grade.Rank,
                    GlobalGrade = grade.Rank
                });
            }

            repository.SaveChanges();

            var bGrades = GetListOfEntities<ExistingGrade>("BGrades");

            foreach (var grade in bGrades)
            {
                AddBGrade(grade);
            }

            repository.SaveChanges();

            var poveyGrades = GetListOfEntities<ExistingGrade>("PoveyGrades");

            foreach (var grade in poveyGrades)
            {
                AddPoveyGrade(grade);
            }

            repository.SaveChanges();

            var furlongGrades = GetListOfEntities<ExistingGrade>("FurlongGrades");

            foreach (var grade in furlongGrades)
            {
                AddFurlongGrade(grade);
            }

            repository.SaveChanges();
        }

        private void AddBGrade(ExistingGrade grade)
        {
            if (grade.GlobalGrade is null)
            {
                throw new ArgumentException("Global grade cannot be null.", nameof(grade));
            }

            if (repository.BGrade is null)
            {
                throw new DatabaseException();
            }

            repository.BGrade.Add(new BGrade()
            {
                Id = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank,
                GlobalGrade = grade.GlobalGrade.Value
            });
        }

        private void AddPoveyGrade(ExistingGrade grade)
        {
            if (grade.GlobalGrade is null)
            {
                throw new ArgumentException("Global grade cannot be null.", nameof(grade));
            }

            if (repository.PoveyGrade is null)
            {
                throw new DatabaseException();
            }

            repository.PoveyGrade.Add(new PoveyGrade()
            {
                Id = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank,
                GlobalGrade = grade.GlobalGrade.Value
            });
        }

        private void AddFurlongGrade(ExistingGrade grade)
        {
            if (grade.GlobalGrade is null)
            {
                throw new ArgumentException("Global grade cannot be null.", nameof(grade));
            }

            if (repository.FurlongGrade is null)
            {
                throw new DatabaseException();
            }

            repository.FurlongGrade.Add(new FurlongGrade()
            {
                Id = grade.Id,
                Name = grade.Name,
                Rank = grade.Rank,
                GlobalGrade = grade.GlobalGrade.Value
            });
        }

        private IEnumerable<TEntity> GetListOfEntities<TEntity>(string fileName)
        {
            string path = Path.ChangeExtension(Path.Combine(existingDataPath, fileName), "json");
            string fileContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(fileContent);
        }
    }
}
