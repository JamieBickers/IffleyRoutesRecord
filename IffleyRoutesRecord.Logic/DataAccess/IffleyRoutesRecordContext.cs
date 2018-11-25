using IffleyRoutesRecord.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace IffleyRoutesRecord.Logic.DataAccess
{
    /// <summary>
    /// Repository for all problem related entities
    /// </summary>
    public class IffleyRoutesRecordContext : DbContext
    {
        public IffleyRoutesRecordContext(DbContextOptions<IffleyRoutesRecordContext> options) : base(options)
        {

        }

        public DbSet<BGrade> BGrade { get; set; }
        public DbSet<FurlongGrade> FurlongGrade { get; set; }
        public DbSet<GeneralRule> GeneralRule { get; set; }
        public DbSet<Hold> Hold { get; set; }
        public DbSet<HoldRule> HoldRule { get; set; }
        public DbSet<PoveyGrade> PoveyGrade { get; set; }
        public DbSet<Problem> Problem { get; set; }
        public DbSet<ProblemHold> ProblemHold { get; set; }
        public DbSet<ProblemHoldRule> ProblemHoldRule { get; set; }
        public DbSet<ProblemRule> ProblemRule { get; set; }
        public DbSet<ProblemStyleSymbol> ProblemStyleSymbol { get; set; }
        public DbSet<StyleSymbol> StyleSymbol { get; set; }
        public DbSet<TechGrade> TechGrade { get; set; }
        public DbSet<ProblemIssue> ProblemIssue { get; set; }
        public DbSet<Issue> Issue { get; set; }
    }
}
