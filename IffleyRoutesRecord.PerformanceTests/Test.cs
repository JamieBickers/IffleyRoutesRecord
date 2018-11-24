using BenchmarkDotNet.Attributes;
using IffleyRoutesRecord.Logic.DataAccess;
using IffleyRoutesRecord.Logic.ExistingData;
using IffleyRoutesRecord.Logic.Managers;
using IffleyRoutesRecord.Logic.Validators;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace IffleyRoutesRecord.PerformanceTests
{
    public class Test
    {
        private const string existingDataFilePath = @"C:\Users\bicke\OneDrive\Desktop\IffleyRoutesRecord\IffleyRoutesRecord\ExistingData";

        [Benchmark]
        public void RunTest()
        {
            var cache = new MockCache();

            var connectionStringBuilder = new SqliteConnectionStringBuilder() { DataSource = ":memory:" };
            string connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<IffleyRoutesRecordContext>();
            dbContextOptionsBuilder.UseSqlite(connection);
            var repository = new IffleyRoutesRecordContext(dbContextOptionsBuilder.Options);
            repository.Database.OpenConnection();
            repository.Database.EnsureCreated();

            var styleSymbolManager = new StyleSymbolManager(repository, cache);
            var ruleManager = new RuleManager(repository, cache);
            var holdManager = new HoldManager(repository, cache, ruleManager);
            var gradeManager = new GradeManager(repository, cache);
            var globalGradeAssigner = new GlobalGradeAssigner(gradeManager);
            var problemReader = new ProblemReader(repository, cache, styleSymbolManager, ruleManager, holdManager, gradeManager, globalGradeAssigner);
            var validator = new ProblemRequestValidator(repository);

            var staticDataPopulater = new PopulateDatabaseWithStaticData(repository, existingDataFilePath);
            staticDataPopulater.Populate();

            var populator = new PopulateDatabaseWithExistingProblems(repository, existingDataFilePath, validator);
            populator.Populate(false);
        }
    }
}
