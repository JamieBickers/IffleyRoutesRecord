namespace IffleyRoutesRecord.Logic.Entities
{
    public class ProblemStyleSymbol
    {
        public int Id { get; set; }
        public int ProblemId { get; set; }
        public int StyleSymbolId { get; set; }

        public Problem Problem { get; set; }
        public StyleSymbol StyleSymbol { get; set; }
    }
}
