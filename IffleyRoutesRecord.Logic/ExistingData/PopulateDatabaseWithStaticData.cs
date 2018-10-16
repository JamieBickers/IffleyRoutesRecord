using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData.Models;
using IffleyRoutesRecord.Models.Entities;
using Newtonsoft.Json;
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
            var techGrades = GetListOfEntities<ExistingGrade>("TechGrades");

            foreach (var grade in techGrades)
            {
                repository.TechGrade.Add(new TechGrade()
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Rank = grade.Rank
                });
            }

            repository.SaveChanges();

            var bGrades = GetListOfEntities<ExistingGrade>("BGrades");

            foreach (var grade in bGrades)
            {
                repository.BGrade.Add(new BGrade()
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Rank = grade.Rank
                });
            }

            repository.SaveChanges();

            var poveyGrades = GetListOfEntities<ExistingGrade>("PoveyGrades");

            foreach (var grade in poveyGrades)
            {
                repository.PoveyGrade.Add(new PoveyGrade()
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Rank = grade.Rank
                });
            }

            repository.SaveChanges();

            var furlongGrades = GetListOfEntities<ExistingGrade>("FurlongGrades");

            foreach (var grade in furlongGrades)
            {
                repository.FurlongGrade.Add(new FurlongGrade()
                {
                    Id = grade.Id,
                    Name = grade.Name,
                    Rank = grade.Rank
                });
            }

            repository.SaveChanges();
        }

        private IEnumerable<TEntity> GetListOfEntities<TEntity>(string fileName)
        {
            string path = Path.ChangeExtension(Path.Combine(existingDataPath, fileName), "json");
            string fileContent = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(fileContent);
        }
    }
}
