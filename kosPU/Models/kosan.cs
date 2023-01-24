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
        public string kost_long { get; set; }
        public string kost_lat { get; set; }
        public int payment_id { get; set; }
        public string sender { get; set; }
        public string norekname { get; set; }
        public string bank { get; set; }
        public string norek { get; set; }
        public string bukti { get; set; }
        public int booking_id { get; set; }
        public string pay_usr { get; set; }
        public string pay_status { get; set; }
        public int total_book { get; set; }
        public int periode { get; set; }
        public int room_id { get; set; }
        public string room_floor { get; set; }
        public string room_name { get; set; }
        public string room_status { get; set; }
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string MAJOR { get; set; }
        public DateTime? START { get; set; }
         public string NOTELP { get; set; }
        public string kost_photo { get; set; }

        public string kost_norek { get; set; }
        public string kost_bank { get; set; }
        public string kost_kostname { get; set; }
        public string kost_notelp { get; set; }
    }
}