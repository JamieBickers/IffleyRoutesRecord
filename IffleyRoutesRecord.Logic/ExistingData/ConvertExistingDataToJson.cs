using IffleyRoutesRecord.Logic.DataAccess;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace IffleyRoutesRecord.Logic.ExistingData
{
    public class ConvertExistingDataToJson
    {
        private const string basePath = @"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\ExistingData";

        private readonly IffleyRoutesRecordContext repository;

        public ConvertExistingDataToJson(IffleyRoutesRecordContext repository)
        {
            this.repository = repository;
        }

        public void Convert()
        {
            var techGrades = repository.TechGrade.AsEnumerable();
            var bGrades = repository.BGrade.AsEnumerable();
            var poveyGrades = repository.PoveyGrade.AsEnumerable();
            var furlongGrades = repository.FurlongGrade.AsEnumerable();

            var styleSymbols = repository.StyleSymbol.AsEnumerable();
            var holds = repository.Hold.AsEnumerable();

            WriteEntitiesToFile(techGrades.Select(grade => new { grade.Id, grade.Name, grade.Rank }), "TechGrades");

            WriteEntitiesToFile(bGrades.Select(grade => new { grade.Id, grade.Name, grade.Rank }), "BGrades");

            WriteEntitiesToFile(poveyGrades.Select(grade => new { grade.Id, grade.Name, grade.Rank }), "PoveyGrades");

            WriteEntitiesToFile(furlongGrades.Select(grade => new { grade.Id, grade.Name, grade.Rank }), "FurlongGrades");

            WriteEntitiesToFile(styleSymbols.Select(styleSymbol => new { styleSymbol.Id, styleSymbol.Name, styleSymbol.Description }), "StyleSymbols");

            WriteEntitiesToFile(holds.Select(hold => new { hold.Id, hold.Name, hold.ParentHoldId }), "Holds");

        }

        private void WriteEntitiesToFile(object entities, string fileName)
        {
            string filePath = Path.ChangeExtension(Path.Join(basePath, fileName), "json");
            File.WriteAllText(filePath, JsonConvert.SerializeObject(entities));
        }
    }
}
