using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Dynamic;
using System.IO;
using Microsoft.Ajax.Utilities;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Web.UI;
using System.Web.Helpers;
using System.Net.Mail;
using kosPU.Models;
namespace kosPU.Controllers
{
    public class HomeOwnerController : Controller
    {
        // GET: HomeOwner
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult dashboardowner()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string owner = Session["us_usrname"].ToString();
            List<kosan> kos = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "select * from kosanid where kost_owner = @kost_owner";
            using (SqlCommand cmd = new SqlCommand(query, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_owner", owner);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    kosan koss = new kosan();
                    koss.kost_id = (int)dr["kost_id"];
                    koss.kost_name = dr["kost_name"].ToString();
                    koss.kost_khusus = dr["kost_khusus"].ToString();
                    koss.kost_price = (int)dr["kost_price"];
                    koss.kost_long = dr["kost_long"].ToString();
                    koss.kost_lat = dr["kost_lat"].ToString();
                    koss.kost_photo = dr["kost_photo"].ToString();
                    koss.kost_norek = dr["kost_norek"].ToString();
                    koss.kost_notelp = dr["kost_notelp"].ToString();
                    koss.kost_bank = dr["kost_bank"].ToString();
                    koss.kost_kostname = dr["kost_kostname"].ToString();
                    
                    kos.Add(koss);
                }
            }
            //
            return View(kos);
        }
        public ActionResult request()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string owner = Session["us_usrname"].ToString();
            List<kosan> kos = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query1 = "select sender, bank, norek, bukti, periode, total_book, pay_status, NOTELP , kosanid.kost_name, NAME, kost_name, START, periode, kost_price, kosanid.kost_id, room_id, room_floor, room_name, room_status, NAME, payment_id, pay_usr from paymentid " +
                "join booking on booking.booking_id = paymentid.booking_id " +
                "join kosanid on kosanid.kost_id = booking.kost_id " +
                "join roomid on roomid.kost_id_room = kosanid.kost_id where kosanid.kost_owner = @kost_owner and(pay_status = 'waiting')";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_owner", owner);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    kosan koss = new kosan();
                    koss.NOTELP = dr["NOTELP"].ToString();
                    koss.sender = dr["sender"].ToString();
                    koss.bank = dr["bank"].ToString();
                    koss.norek = dr["norek"].ToString();
                    koss.bukti = dr["bukti"].ToString();
                    koss.periode = (int)dr["periode"];
                    koss.total_book = (int)dr["total_book"];
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.kost_name = dr["kost_name"].ToString();
                    koss.kost_name = dr["kost_name"].ToString();
                    koss.kost_price = (int)dr["kost_price"];
                    koss.room_floor = dr["room_floor"].ToString();
                    koss.room_name = dr["room_name"].ToString();
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.NAME = dr["NAME"].ToString();
                    koss.START = (DateTime)dr["START"];
                    koss.periode = (int)dr["periode"];
                    koss.room_id = (int)dr["room_id"];
                    koss.payment_id = (int)dr["payment_id"];
                    koss.pay_usr = dr["pay_usr"].ToString();
                    koss.kost_id = (int)dr["kost_id"];
                    kos.Add(koss);
                }
            }
            return View(kos);
        }
        [HttpPost]
        public ActionResult editkosan(HttpPostedFileBase file, FormCollection form, int price, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string name = form["name"];
            string khusus = form["khusus"];
            string lokasi1 = form["lokasi11"];
            string lokasi2 = form["lokasi21"];
            string norek = form["norek"];
            string rekname = form["rekname"];
            string Bank = form["Bank"];
            string contact = form["contact"];
            string owner = Session["us_usrname"].ToString();
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/document/photokost/"), filename);
                if (file.ContentLength > 0)
                {
                    string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(mainconn);
                    string query = "update kosanid set kost_name = @kost_name, kost_khusus= @kost_khusus, kost_price = @kost_price, kost_owner = @kost_owner, kost_long = @kost_long, kost_lat = @kost_lat, kost_photo = @kost_photo, kost_norek = @kost_norek, kost_bank = @kost_bank, kost_kostname = @kost_kostname, kost_notelp = @kost_notelp where kost_id = @kost_id";

                    SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@kost_id", kost_id);
                    sqlcomm.Parameters.AddWithValue("@kost_photo", filename);
                    sqlcomm.Parameters.AddWithValue("@kost_name", name);
                    sqlcomm.Parameters.AddWithValue("@kost_khusus", khusus);
                    sqlcomm.Parameters.AddWithValue("@kost_price", price);
                    sqlcomm.Parameters.AddWithValue("@kost_owner", owner);
                    sqlcomm.Parameters.AddWithValue("@kost_long", lokasi1);
                    sqlcomm.Parameters.AddWithValue("@kost_lat", lokasi2);
                    sqlcomm.Parameters.AddWithValue("@kost_norek", norek);
                    sqlcomm.Parameters.AddWithValue("@kost_bank", Bank);
                    sqlcomm.Parameters.AddWithValue("@kost_kostname", rekname);
                    sqlcomm.Parameters.AddWithValue("@kost_notelp", contact);
                    file.SaveAs(filepath);
                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();

                    myConnection.Close();
                }
            }
            return Redirect("dashboardowner");
        }
        [HttpPost]
        public ActionResult editroom(FormCollection form, int room_id, int kost_id)
        {

            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string floor = form["floor"];
            string roomname = form["roomname"];
          
                string query = @"update roomid set room_floor =@room_floor, room_name = @room_name where room_id = @room_id";

                SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
                sqlconn.Open();
                sqlcomm.Parameters.AddWithValue("@room_floor", floor);
                sqlcomm.Parameters.AddWithValue("@room_name", roomname);
            sqlcomm.Parameters.AddWithValue("@room_id", room_id);
                
                sqlcomm.ExecuteNonQuery();
                TempData["messsage"] = "succes";
                sqlconn.Close();
            
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult deleteroom (int kost_id, int room_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;

            string query = @" delete roomid where room_id = @room_id";
            using (SqlCommand cmd = new SqlCommand(query, myConnection))
            {
                myConnection.Open();
                cmd.Parameters.AddWithValue("@room_id", room_id);


                cmd.ExecuteNonQuery();
                myConnection.Close();


            }
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult deletekosan (int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
        
            string query = @" delete kosanid where kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query, myConnection))
            {
                myConnection.Open();
                cmd.Parameters.AddWithValue("@kost_id", kost_id);


                cmd.ExecuteNonQuery();
                myConnection.Close();


            }
            return Redirect("dashboardowner");
        }
        [HttpPost]
        public ActionResult inputkosan(HttpPostedFileBase file, FormCollection form, int price)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string name = form["name"];
            string khusus = form["khusus"];
            string lokasi1 = form["lokasi1"];
            string lokasi2 = form["lokasi2"];
            string norek = form["norek"];
            string rekname = form["rekname"];
            string Bank = form["Bank"];
            string contact = form["contact"];
            string owner = Session["us_usrname"].ToString();     
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/document/photokost/"), filename);
                if (file.ContentLength > 0)
                {
                    string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(mainconn);
                    string query = "insert into kosanid (kost_name, kost_khusus, kost_price, kost_owner, kost_long, kost_lat, kost_photo, kost_norek, kost_bank, kost_kostname, kost_notelp) " +
                        "values (@kost_name, @kost_khusus, @kost_price, @kost_owner, @kost_long, @kost_lat, @kost_photo, @kost_norek, @kost_bank, @kost_kostname, @kost_notelp)";

                    SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@kost_photo", filename);
                    sqlcomm.Parameters.AddWithValue("@kost_name", name);
                    sqlcomm.Parameters.AddWithValue("@kost_khusus", khusus);
                    sqlcomm.Parameters.AddWithValue("@kost_price", price);
                    sqlcomm.Parameters.AddWithValue("@kost_owner", owner);
                    sqlcomm.Parameters.AddWithValue("@kost_long", lokasi1);
                    sqlcomm.Parameters.AddWithValue("@kost_lat", lokasi2);
                    sqlcomm.Parameters.AddWithValue("@kost_norek", norek);
                    sqlcomm.Parameters.AddWithValue("@kost_bank", Bank);
                    sqlcomm.Parameters.AddWithValue("@kost_kostname", rekname);
                    sqlcomm.Parameters.AddWithValue("@kost_notelp", contact);
                    file.SaveAs(filepath);
                    sqlcomm.ExecuteNonQuery();
                    sqlconn.Close();

                    myConnection.Close();
                }
            }
            return Redirect("dashboardowner");
        }
        public ActionResult DetailPenghuni(int kost_id)
        {
            
            List<detailowner> detail = new List<detailowner>();
            //kos

            //ress
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "select * from residentid where res_kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.resident_id = (int)dr["resident_id"];
                    detailo.resident_name = dr["resident_name"].ToString();
                    detailo.no_telp = dr["no_telp"].ToString();
                    detailo.email = dr["email"].ToString();
                    detailo.resident_ket = dr["resident_ket"].ToString();
                    detailo.res_kost_id = (int)dr["res_kost_id"];
                    
                        detailo.start = (DateTime)dr["start"];
                   
                   
                        detailo.res_end = (DateTime)dr["res_end"];
                    
                    detail.Add(detailo);
                }
            }
            //kos
            string querykos = "select * from kosanid where kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(querykos, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.kost_id = (int)dr["kost_id"];
                    detailo.kost_name = dr["kost_name"].ToString();
                    detail.Add(detailo);
                }
            }
            //room
            string query1 = "select * from roomid where kost_id_room = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.room_id = (int)dr["room_id"];
                    detailo.room_name = dr["room_name"].ToString();
                    detailo.room_floor = dr["room_floor"].ToString();
                    detailo.kost_id_room = (int)dr["kost_id_room"];
                    detailo.room_status = dr["room_status"].ToString();
                    detail.Add(detailo);
                }
            }
            //BENEFIT
            string query2 = "select * from benefitid where benefit_kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query2, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.benefit_id = (int)dr["benefit_id"];
                    detailo.ben_keep = dr["ben_keep"].ToString();
                    detailo.ben_free = dr["ben_free"].ToString();
                    detailo.benefit_kost_id = (int)dr["benefit_kost_id"];
                    detail.Add(detailo);
                }
            }
            //spek
            string query3 = "select * from spesicationid where spek_kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query3, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.spek_id = (int)dr["spek_id"];
                    detailo.spek_size = dr["spek_size"].ToString();
                    detailo.spek_bath = dr["spek_bath"].ToString();
                    detailo.spek_kost_id = (int)dr["spek_kost_id"];
                    detail.Add(detailo);
                }
            }
            //fasilitas
            string query4 = "select * from facid where fac_kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query4, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.fac_id = (int)dr["fac_id"];
                    detailo.AC = dr["AC"].ToString();
                    detailo.BED = dr["BED"].ToString();
                    detailo.cupboard = dr["cupboard"].ToString();
                    detailo.washing = dr["washing"].ToString();
                    detailo.tabble = dr["tabble"].ToString();
                    detailo.regni = dr["regni"].ToString();
                    detailo.wifi = dr["wifi"].ToString();
                    detailo.chair = dr["chair"].ToString();
                    detailo.TV = dr["TV"].ToString();
                    detailo.fac_kost_id = (int)dr["fac_kost_id"];
                    detail.Add(detailo);
                }
            }
            //total room
            string username = Session["us_usrname"].ToString();

            string query5 = "select COUNT(room_name) as totalroom from kosanid join roomid on kosanid.kost_id = roomid.kost_id_room where kost_owner = @kost_owner and kosanid.kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query5, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                cmd.Parameters.AddWithValue("@kost_owner", username);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.totalroom = (int)dr["totalroom"];
                    detail.Add(detailo);
                }
            }
            //total boked
           

            string query6 = "select COUNT(room_name) as totalboked from kosanid join roomid on kosanid.kost_id = roomid.kost_id_room where kost_owner = @kost_owner and kosanid.kost_id = @kost_id and roomid.room_status = 'booked'";
            using (SqlCommand cmd = new SqlCommand(query6, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                cmd.Parameters.AddWithValue("@kost_owner", username);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.totalboked = (int)dr["totalboked"];
                    detail.Add(detailo);
                }
            }

            //total empty


            string query7 = "select COUNT(room_name) as totalempty from kosanid join roomid on kosanid.kost_id = roomid.kost_id_room where kost_owner = @kost_owner and kosanid.kost_id = @kost_id and roomid.room_status = 'nothing'";
            using (SqlCommand cmd = new SqlCommand(query7, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                cmd.Parameters.AddWithValue("@kost_owner", username);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.totalempty = (int)dr["totalempty"];
                    detail.Add(detailo);
                }
            }
            return View(detail);
        }

        //addroom
        [HttpPost]
        public ActionResult Addroom(FormCollection form, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
           
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string floor = form["floor"];
            string roomname = form["roomname"];

            SqlCommand checkroom = new SqlCommand("select * from roomid where room_name = '" + roomname + "' AND room_floor = '" + floor + "' and kost_id_room ='" + kost_id +"'", myConnection);
            SqlDataAdapter sdcdi = new SqlDataAdapter(checkroom);
            DataTable dtcdi = new DataTable();
            sdcdi.Fill(dtcdi);
            if (dtcdi.Rows.Count > 0)
            {

            }
            else
            {

          
            string query = @"INSERT INTO roomid(room_name, room_floor, kost_id_room, room_status) " +
                    "values (@room_name, @room_floor, @kost_id_room, @room_status)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@room_floor", floor);
            sqlcomm.Parameters.AddWithValue("@room_name", roomname);
            sqlcomm.Parameters.AddWithValue("@kost_id_room", kost_id);
            sqlcomm.Parameters.AddWithValue("@room_status", "nothing");
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            }
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        //add resident
        [HttpPost]
        public ActionResult Addresident(FormCollection form, int kost_id, DateTime start, DateTime end)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string resident_name = form["resident_name"];
            string notelp = form["notelp"];
            var penghuni = form["penghuni"];
            string roomname = form["roomname"];
            
            string query = @"INSERT INTO residentid(resident_name, no_telp, start, res_end, res_kost_id) " +
                    "values (@resident_name, @no_telp, @start, @res_end, @res_kost_id)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@resident_name", resident_name);
            sqlcomm.Parameters.AddWithValue("@no_telp", notelp);
            sqlcomm.Parameters.AddWithValue("@res_kost_id", kost_id);
            sqlcomm.Parameters.AddWithValue("@start", start);
            sqlcomm.Parameters.AddWithValue("@res_end", end);
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult editbenefit(FormCollection form, int kost_id, int benefit_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string penjaga = form["penjaga"];
            string admin = form["admin"];


            string query = @"update benefitid set ben_keep = @ben_keep, ben_free = @ben_free where benefit_id =@benefit_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();

            sqlcomm.Parameters.AddWithValue("@ben_keep", penjaga);
            sqlcomm.Parameters.AddWithValue("@benefit_id", benefit_id);
            sqlcomm.Parameters.AddWithValue("@ben_free", admin);

            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult Addbenefit(FormCollection form, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string penjaga = form["penjaga"];
            string admin = form["admin"];
          

            string query = @"INSERT INTO benefitid(ben_keep, ben_free, benefit_kost_id) " +
                    "values (@ben_keep, @ben_free, @benefit_kost_id)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
           
            sqlcomm.Parameters.AddWithValue("@ben_keep", penjaga);
            sqlcomm.Parameters.AddWithValue("@benefit_kost_id", kost_id);
            sqlcomm.Parameters.AddWithValue("@ben_free", admin);
     
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        
       
        //add spek
        [HttpPost]
        public ActionResult editSpek (FormCollection form, int kost_id, int spek_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string size = form["size"];
            string bathroom = form["bathroom"];


            string query = @"update spesicationid set spek_size = @spek_size, spek_bath = @spek_bath where spek_id =@spek_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();

            sqlcomm.Parameters.AddWithValue("@spek_size", size);
            sqlcomm.Parameters.AddWithValue("@spek_id", spek_id);
            sqlcomm.Parameters.AddWithValue("@spek_bath", bathroom);

            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult AddSpek(FormCollection form, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string size = form["size"];
            string bathroom = form["bathroom"];


            string query = @"INSERT INTO spesicationid(spek_size, spek_bath, spek_kost_id) " +
                    "values (@spek_size, @spek_bath, @spek_kost_id)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();

            sqlcomm.Parameters.AddWithValue("@spek_size", size);
            sqlcomm.Parameters.AddWithValue("@spek_kost_id", kost_id);
            sqlcomm.Parameters.AddWithValue("@spek_bath", bathroom);

            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult Editfasilitas (FormCollection form, int kost_id, int fac_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string bed = form["bed"];
            string cup = form["cup"];
            string wash = form["wash"];
            string table = form["table"];
            string kulkas = form["kulkas"];
            string Wifi = form["Wifi"];
            string Chair = form["Chair"];
            string TV = form["TV"];
            string AC = form["AC"];
            string query = @"update facid set AC = @AC, BED = @BED, cupboard =@cupboard, washing = @washing, tabble =@tabble, regni =@regni, wifi = @wifi, chair = @chair, TV = @TV where fac_id =@fac_id";
            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@fac_id", fac_id);
            sqlcomm.Parameters.AddWithValue("@AC", AC);
            
            sqlcomm.Parameters.AddWithValue("@BED", bed);
            sqlcomm.Parameters.AddWithValue("@cupboard", cup);
            sqlcomm.Parameters.AddWithValue("@washing", wash);
            sqlcomm.Parameters.AddWithValue("@tabble", table);
            sqlcomm.Parameters.AddWithValue("@regni", kulkas);
            sqlcomm.Parameters.AddWithValue("@wifi", Wifi);
            sqlcomm.Parameters.AddWithValue("@chair", Chair);
            sqlcomm.Parameters.AddWithValue("@TV", TV);

            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        //add fasilitas
        [HttpPost]
        public ActionResult Addfasilitas(FormCollection form, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string bed = form["bed"];
            string cup = form["cup"];
            string wash = form["wash"];
            string table = form["table"];
            string kulkas = form["kulkas"];
            string Wifi = form["Wifi"];
            string Chair = form["Chair"];
            string TV = form["TV"];
            string AC = form["AC"];         
            string query = @"INSERT INTO facid(AC, BED, cupboard, washing, tabble, regni, wifi, chair, TV, fac_kost_id) " +
                    "values (@AC, @BED, @cupboard, @washing, @tabble, @regni, @wifi, @chair, @TV, @fac_kost_id)";
            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@AC", AC);
            sqlcomm.Parameters.AddWithValue("@fac_kost_id", kost_id);
            sqlcomm.Parameters.AddWithValue("@BED", bed);
            sqlcomm.Parameters.AddWithValue("@cupboard", cup);
            sqlcomm.Parameters.AddWithValue("@washing", wash);
            sqlcomm.Parameters.AddWithValue("@tabble", table);
            sqlcomm.Parameters.AddWithValue("@regni", kulkas);
            sqlcomm.Parameters.AddWithValue("@wifi", Wifi);
            sqlcomm.Parameters.AddWithValue("@chair", Chair);
            sqlcomm.Parameters.AddWithValue("@TV", TV);

            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "DetailPenghuni?kost_id=" + kost_id;
            return Redirect(balik);
        }
        public ActionResult history()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string owner = Session["us_usrname"].ToString();
            List<kosan> kos = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            //string query1 = "select paymentid.sender, paymentid.pay_status, booking.NAME, " +
            //    "booking.method_pay, bukti, norek, bank, periode, total_book, payment_id, room_id, room_floor, room_name from paymentid " +
            //    "join booking on booking.booking_id = paymentid.payment_id " +
            //    "join kosanid on kosanid.kost_id = booking.kost_id " +
            //    "join roomid on roomid.kost_id_room = kosanid.kost_id where kosanid.kost_owner = @kost_owner";
            string query1 = "select sender, bank, norek, bukti, periode, total_book, pay_status, kosanid.kost_name, kost_price," +
                " kosanid.kost_id, room_id, room_floor, room_name, room_status, NAME, payment_id, pay_usr from paymentid " +
                "join booking on booking.booking_id = paymentid.booking_id " +
                "join kosanid on kosanid.kost_id = booking.kost_id " +
                "join roomid on roomid.kost_id_room = kosanid.kost_id where kosanid.kost_owner = @kost_owner and(room_status = 'booked')";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_owner", owner);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    kosan koss = new kosan();
                    koss.sender = dr["sender"].ToString();
                    koss.bank = dr["bank"].ToString();
                    koss.norek = dr["norek"].ToString();
                    koss.bukti = dr["bukti"].ToString();
                    koss.periode = (int)dr["periode"];
                    koss.total_book = (int)dr["total_book"];
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.kost_name = dr["kost_name"].ToString();
                    koss.kost_price = (int)dr["kost_price"];
                    koss.room_floor = dr["room_floor"].ToString();
                    koss.room_name = dr["room_name"].ToString();
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.NAME = dr["NAME"].ToString();
                    koss.room_id = (int)dr["room_id"];
                    koss.payment_id = (int)dr["payment_id"];
                    koss.pay_usr = dr["pay_usr"].ToString();
                    koss.kost_id = (int)dr["kost_id"];
                    kos.Add(koss);
                }
            }
            return View(kos);
        }
        
       public ActionResult booked()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string owner = Session["us_usrname"].ToString();
            List<kosan> kos = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query1 = "select sender, bank, norek, bukti, periode, total_book, pay_status, kosanid.kost_name, kost_price," +
                " kosanid.kost_id, room_id, room_floor, room_name, room_status, NAME, payment_id, pay_usr, NOTELP, EMAIL, MAJOR, START, periode from paymentid " +
                "join booking on booking.booking_id = paymentid.booking_id " +
                "join kosanid on kosanid.kost_id = booking.kost_id " +
                "join roomid on roomid.kost_id_room = kosanid.kost_id where kosanid.kost_owner = @kost_owner and(room_status = 'booked')";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_owner", owner);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    kosan koss = new kosan();
                    koss.sender = dr["sender"].ToString();
                    koss.bank = dr["bank"].ToString();
                    koss.norek = dr["norek"].ToString();
                    koss.bukti = dr["bukti"].ToString();
                    koss.periode = (int)dr["periode"];
                    koss.total_book = (int)dr["total_book"];
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.kost_name = dr["kost_name"].ToString();
                    koss.kost_price = (int)dr["kost_price"];
                    koss.room_floor = dr["room_floor"].ToString();
                    koss.room_name = dr["room_name"].ToString();
                    koss.pay_status = dr["pay_status"].ToString();
                    koss.NAME = dr["NAME"].ToString();
                    koss.room_id = (int)dr["room_id"];
                    koss.payment_id = (int)dr["payment_id"];
                    koss.pay_usr = dr["pay_usr"].ToString();
                    koss.kost_id = (int)dr["kost_id"];
                    koss.EMAIL = dr["EMAIL"].ToString();
                    koss.MAJOR = dr["MAJOR"].ToString();
                    koss.START = (DateTime)dr["START"];
                    kos.Add(koss);
                }
            }
            return View(kos);
        }

        [HttpPost]
        public ActionResult approve(FormCollection form)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var username = Session["us_usrname"];
            var room_id = form["room_id"];
            int room = int.Parse(room_id);
            var payment_id = form["payment_id"];
            int payment = int.Parse(payment_id);

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            //
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "update paymentid set last_update = CURRENT_TIMESTAMP, last_up_by = @last_up_by," +
                " pay_status = @pay_status where payment_id = @payment_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@last_up_by", username);
            sqlcomm.Parameters.AddWithValue("@pay_status", "approve");
            sqlcomm.Parameters.AddWithValue("@payment_id", payment);
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            string query1 = @"update roomid set room_status = @room_status where room_id = @room_id";

            SqlCommand sqlcomm1 = new SqlCommand(query1, sqlconn);
            sqlconn.Open();
            sqlcomm1.Parameters.AddWithValue("@room_status", "booked");
            sqlcomm1.Parameters.AddWithValue("@room_id", room);
           
            sqlcomm1.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            var kos = form["kost_id"];
            int kost_id = int.Parse(kos);
            string user = form["username"];
            string query2 = @"update account_user set kost_id = @kost_id where username_user = @username_user";

            SqlCommand sqlcomm2 = new SqlCommand(query2, sqlconn);
            sqlconn.Open();
            sqlcomm2.Parameters.AddWithValue("@kost_id", kost_id);
            sqlcomm2.Parameters.AddWithValue("@username_user", user);

            sqlcomm2.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();

            myConnection.Close();
          
            return Redirect("request");
        }


        [HttpPost]
        public ActionResult reject(FormCollection form)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var username = Session["us_usrname"];
            var room_id = form["room_id"];
            int room = int.Parse(room_id);
            var payment_id = form["payment_id"];
            int payment = int.Parse(payment_id);

            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            //
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "update paymentid set last_update = CURRENT_TIMESTAMP, last_up_by = @last_up_by," +
                " pay_status = @pay_status where payment_id = @payment_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@last_up_by", username);
            sqlcomm.Parameters.AddWithValue("@pay_status", "reject");
            sqlcomm.Parameters.AddWithValue("@payment_id", payment);
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            string query1 = @"update roomid set room_status = @room_status where room_id = @room_id";

            SqlCommand sqlcomm1 = new SqlCommand(query1, sqlconn);
            sqlconn.Open();
            sqlcomm1.Parameters.AddWithValue("@room_status", "nothing");
            sqlcomm1.Parameters.AddWithValue("@room_id", room);

            sqlcomm1.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            var kos = form["kost_id"];
            int kost_id = int.Parse(kos);
            string user = form["username"];
            string query2 = @"delete from booking where kost_id = @kost_id and booking_usr = @username_user";

            SqlCommand sqlcomm2 = new SqlCommand(query2, sqlconn);
            sqlconn.Open();
            sqlcomm2.Parameters.AddWithValue("@kost_id", kost_id);
            sqlcomm2.Parameters.AddWithValue("@username_user", user);

            sqlcomm2.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();

            myConnection.Close();

            return Redirect("request");
        }
        [HttpPost]
        public ActionResult end(FormCollection form)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var username = Session["us_usrname"];
            var kost_id = form["kost_id"];
            var room_id = form["room_id"];
            string userpay = form["userpay"];
            var pay_id = form["pay_id"];

            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "update paymentid set last_update = CURRENT_TIMESTAMP, last_up_by = @last_up_by," +
                " pay_status = @pay_status where payment_id = @payment_id";
            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@last_up_by", username);
            sqlcomm.Parameters.AddWithValue("@pay_status", "end");
            sqlcomm.Parameters.AddWithValue("@payment_id", pay_id);
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            string query1 = @"update roomid set room_status = @room_status where room_id = @room_id";

            SqlCommand sqlcomm1 = new SqlCommand(query1, sqlconn);
            sqlconn.Open();
            sqlcomm1.Parameters.AddWithValue("@room_status", "nothing");
            sqlcomm1.Parameters.AddWithValue("@room_id", room_id);

            sqlcomm1.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            string query2 = @"delete from booking where kost_id = @kost_id and booking_usr = @username_user";
           
            SqlCommand sqlcomm2 = new SqlCommand(query2, sqlconn);
            sqlconn.Open();
            sqlcomm2.Parameters.AddWithValue("@kost_id", kost_id);
            sqlcomm2.Parameters.AddWithValue("@username_user", userpay);

            sqlcomm2.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            //
            string query3 = "update  account_user set kost_id = null where username_user = @username_user";
            SqlCommand sqlcomm3 = new SqlCommand(query3, sqlconn);
            sqlconn.Open();
            sqlcomm3.Parameters.AddWithValue("@kost_id", kost_id);
            sqlcomm3.Parameters.AddWithValue("@username_user", userpay);

            sqlcomm3.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
            return Redirect("booked");
        }
    }

}