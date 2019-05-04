namespace IffleyRoutesRecord.Models.DTOs.Responses
{
    public class BGradeResponse : BaseGradeResponse
    {
        public BGradeResponse(int gradeId, string name, int rank, int globalGrade)
            : base(gradeId, name, rank, globalGrade)
        {

        }
    }
}
