using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    public class Student : IdentityUser
    {
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string FirstName { get; set; }
        [Required]
        [Length(1, 15, ErrorMessage = "Length is larger than 0 and shorter than 16")]
        public string LastName { get; set; }
        [Required]
        [Length(1, 50, ErrorMessage = "Length is larger than 0 and shorter than 51")]
        public string Address { get; set; }
    }
}
