using System.Collections.Generic;

namespace IffleyRoutesRecord.Entities
{
    public class GeneralRule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<ProblemRule> ProblemRules { get; set; }
    }
}
