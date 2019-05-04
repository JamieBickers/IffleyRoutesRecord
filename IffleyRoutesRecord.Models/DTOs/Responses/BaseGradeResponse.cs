namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public abstract class BaseGradeResponse
    {
        public int GradeId { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public int GlobalGrade { get; set; }

        protected BaseGradeResponse(int gradeId, string name, int rank, int globalGrade)
        {
            GradeId = gradeId;
            Name = name;
            Rank = rank;
            GlobalGrade = globalGrade;
        }
    }
}
