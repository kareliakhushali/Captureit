using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Captureit.Models
{
    public class User
    {
       
        public int Id { get; set; }

        public string RollNo { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Display(Name = "Mobile Number:")]      
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        public string ContactNo { get; set; }
        
        [EmailAddress]
        [JsonPropertyName("semail")]
        public string Email { get; set; }
        [JsonPropertyName("class")]
        
        public int Semester { get; set; }
        
        public string Department { get; set; }
        
        [Display(Name = "Mobile Number:")]       
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Please Enter Valid Mobile Number.")]
        [JsonPropertyName("pmobileno")]
        public string ReferenceMobileNo { get; set; }

       
        [EmailAddress]
        [JsonPropertyName("pemail")]
        public string ReferenceEmail { get; set; }

        public string Role { get; set; }      

        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public float Attendance { get; set; }

        public string Image { get; set; }

    }
}
