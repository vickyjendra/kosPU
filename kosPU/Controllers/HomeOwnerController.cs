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
            string query = "  select * from kosanid where kost_owner = @kost_owner";
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
                    koss.kost_location = dr["kost_location"].ToString();
                    kos.Add(koss);
                }
            }


                return View(kos);
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
            string lokasi = form["lokasi"];
            string owner = Session["us_usrname"].ToString();     
;            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = @"insert into kosanid (kost_name, kost_khusus, kost_price, kost_owner, kost_location) values (@kost_name, @kost_khusus, @kost_price, @kost_owner, @kost_location)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();

            sqlcomm.Parameters.AddWithValue("@kost_name", name);
            sqlcomm.Parameters.AddWithValue("@kost_khusus", khusus);
            sqlcomm.Parameters.AddWithValue("@kost_price", price);
            sqlcomm.Parameters.AddWithValue("@kost_owner", owner);
            sqlcomm.Parameters.AddWithValue("@kost_location", lokasi);

            sqlcomm.ExecuteNonQuery();
            sqlconn.Close();

            myConnection.Close();
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
                    detailo.room_name = dr["room_name"].ToString();
                    detailo.room_floor = dr["room_floor"].ToString();
                    detailo.kost_id_room = (int)dr["kost_id_room"];
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
           
            string query = @"INSERT INTO roomid(room_name, room_floor, kost_id_room) " +
                    "values (@room_name, @room_floor, @kost_id_room)";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@room_floor", floor);
            sqlcomm.Parameters.AddWithValue("@room_name", roomname);
            sqlcomm.Parameters.AddWithValue("@kost_id_room", kost_id);
            sqlcomm.ExecuteNonQuery();
            TempData["messsage"] = "succes";
            sqlconn.Close();
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
    }

}