using System;
using System.ComponentModel.DataAnnotations;

namespace ShauliProject.Models
{
    public enum Gender
    {
        Male,
        Female
    }

    public class Fan
    {
        public int Id { get; set; }
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of Birth")]
        public DateTime? DateOfBirth { get; set; }
        [Display(Name = "Years Of Seniority")]
        public int YearsOfSeniority { get; set; }
        public string Address { get; set; }
    }
}