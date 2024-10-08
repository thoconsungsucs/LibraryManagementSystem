﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Models
{
    public class Member : IdentityUser<int>
    {
        public int? StudentId { get; set; }
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string FirstName { get; set; }
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
    }
}
