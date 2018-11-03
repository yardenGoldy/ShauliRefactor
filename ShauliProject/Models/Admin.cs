using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShauliProject.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Provide Username", AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please provide password", AllowEmptyStrings = false)]
        public string Password { get; set; }

    }
}