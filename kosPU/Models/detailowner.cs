using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace kosPU.Models
{
    public class detailowner
    {
        //kosan
        public int kost_id { get; set; }

        //resident
        public int resident_id { get; set; }
        public string resident_name { get; set; }
        public string no_telp { get; set; }
        public string email { get; set; }
        public string resident_ket { get; set; }
        public int res_kost_id { get; set; }
        public DateTime? start { get; set; }
        public DateTime? res_end { get; set; }

        //roomid
        public int room_id { get; set; }
        public string room_name { get; set; }
        public string room_floor { get; set; }
        public int kost_id_room { get; set; }

        //benefit
        public int benefit_id { get; set; }
        public string ben_keep { get; set; }
        public string ben_free { get; set; }
        public int benefit_kost_id { get; set; }

        //spek
        public int spek_id { get; set; }
        public string spek_size { get; set; }
        public string spek_bath { get; set; }
        public int spek_kost_id { get; set; }

        //fasilitas
        public int fac_id { get; set; }
        public string AC { get; set; }
        public string BED { get; set; }
        public string cupboard { get; set; }
        public string washing { get; set; }
        public string tabble { get; set; }
        public string regni { get; set; }
        public string wifi { get; set; }
        public string chair { get; set; }
        public string TV { get; set; }
        public int fac_kost_id { get; set; }

        public string kost_name { get; set; }
        public string kost_khusus { get; set; }
        public int kost_price { get; set; }

        public int booking_id { get; set; }
        public string NAME { get; set; }
        public string NOTELP { get; set; }
        public string EMAIL { get; set; }
        public string MAJOR { get; set; }
        public string FLOOR { get; set; }
        public string NUMBER_ROOM { get; set; }
        public DateTime? START { get; set; }
        public DateTime? ENDbooking { get; set; }
        public string booking_usr { get; set; }
        public int periode { get; set; }
        public int total_book { get; set; }
        public string method_pay { get; set; }

        public int payment_id { get; set; }
        public string sender { get; set; }
        public string norekname { get; set; }
        public string bank { get; set; }
        public string norek { get; set; }
        public string bukti { get; set; }
        public string pay_usr { get; set; }
        public string btn_status_pay { get; set; }
        public string pay_status { get; set; }
        public string room_status { get; set; }
        public string kost_photo { get; set; }
        public string kost_norek { get; set; }
        public string kost_bank { get; set; }
        public string kost_kostname { get; set;  }

        public string kost_notelp { get; set; }

        public int empty { get; set; }

        public int totalroom { get; set; }

        public int totalboked { get; set; }
        public int totalempty { get; set; }
    }
}