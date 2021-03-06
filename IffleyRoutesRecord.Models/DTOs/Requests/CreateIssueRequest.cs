﻿using System.ComponentModel.DataAnnotations;

namespace IffleyRoutesRecord.Models.DTOs.Requests
{
    public class CreateIssueRequest
    {
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }
    }
}
