using System.ComponentModel.DataAnnotations;

namespace IffleyRoutesRecord.Models.DTOs.Requests
{
    public class CreateProblemIssueRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProblemId { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }
    }
}
