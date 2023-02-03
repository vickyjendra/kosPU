using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace kosPU.Models
{
    public class adminm
    {
        
        
        public int admin_id { get; set; }
        [Display(Name = "Username")]
        public string usr_admin { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string pass_admin { get; set; }
    }
}