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
    public class adminController : Controller
    {
        // GET: admin
        public ActionResult Index()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("lamanawal", "Account");
            }
            string owner = Session["us_usrname"].ToString();
            List<kosan> kos = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "select * from kosanid";
            using (SqlCommand cmd = new SqlCommand(query, sqlconn))
            {             
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
                    koss.kost_status = dr["kost_status"].ToString();
                    koss.kost_owner = dr["kost_owner"].ToString();

                    kos.Add(koss);
                }
            }
            //
            return View(kos);
        }
        [HttpPost]
        public ActionResult approve(FormCollection form)
        {

            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var kost_id = form["kost_id"];
            int kost = int.Parse(kost_id);
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            //
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "update kosanid set kost_status = @kost_status where kost_id = @kost_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@kost_id", kost);
            sqlcomm.Parameters.AddWithValue("@kost_status", "A");
            
            sqlcomm.ExecuteNonQuery();
            return Redirect("index");
        }
        [HttpPost]
        public ActionResult reject(FormCollection form)
        {

            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var kost_id = form["kost_id"];
            int kost = int.Parse(kost_id);
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            //
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string query = "update kosanid set kost_status = @kost_status where kost_id = @kost_id";

            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@kost_id", kost);
            sqlcomm.Parameters.AddWithValue("@kost_status", "C");

            sqlcomm.ExecuteNonQuery();
            return Redirect("index");
        }
    }
    
}