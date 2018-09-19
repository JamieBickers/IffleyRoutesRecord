namespace IffleyRoutesRecord.Models.Entities
{
    public class ProblemStyleSymbol : BaseEntity
    {
        public int ProblemId { get; set; }
        public int StyleSymbolId { get; set; }

        public Problem Problem { get; set; }
        public StyleSymbol StyleSymbol { get; set; }
    }
}
