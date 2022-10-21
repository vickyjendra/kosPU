using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace kosPU.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        public string username_user { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password_user { get; set; }
    }
}