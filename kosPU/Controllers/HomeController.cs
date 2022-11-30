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
    public class HomeController : Controller
    {


        [HttpGet]
        public ActionResult Index(String get)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<kosan> koss = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "select * from kosanid";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomm);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                kosan kos = new kosan();
                kos.kost_name = dr["kost_name"].ToString();
                kos.kost_price = (int)dr["kost_price"];
                kos.kost_id = (int)dr["kost_id"];
                koss.Add(kos);
            }
            return View(koss);
        }

        public ActionResult Detail(int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
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
                    detailo.kost_price = (int)dr["kost_price"];
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

                return View(detail);
            }
        }

        public ActionResult Boking(int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<detailowner> detail = new List<detailowner>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

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
                    detailo.kost_price = (int)dr["kost_price"];
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

            return View(detail);
        }
    }
}



