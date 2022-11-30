using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kosPU.Models
{
    public class kosan
    {
        public int kost_id { get; set; }
        public string kost_name { get; set; }
        public string kost_khusus { get; set; }
        public int kost_price { get; set; }
        public string kost_owner { get; set; }
        public string kost_location { get; set; }
    }
}