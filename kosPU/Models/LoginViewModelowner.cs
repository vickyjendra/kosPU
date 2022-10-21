using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace kosPU.Models
{
    public class LoginViewModelowner
    {
        [Display(Name = "Username")]
        public string username_owner { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password_owner { get; set; }
    }
}