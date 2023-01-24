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
            string query = "select distinct kost_price, kost_name, kosanid.kost_id, kost_lat, kost_long, kost_photo from kosanid join roomid on kosanid.kost_id = kost_id_room  where room_status = 'nothing'";

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
               
                //kos.kost_location = dr["kost_location"].ToString();
                kos.kost_lat = dr["kost_lat"].ToString();
                kos.kost_long = dr["kost_long"].ToString();
                kos.kost_photo = dr["kost_photo"].ToString();
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
                    detailo.kost_photo = dr["kost_photo"].ToString();
                    detailo.kost_khusus = dr["kost_khusus"].ToString();
                    detail.Add(detailo);
                }
            }
            //room
            string query1 = "SELECT COUNT(room_status) as empty FROM roomid WHERE room_status = 'nothing' and kost_id_room =@kost_id";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.empty = (int)dr["empty"];
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
            //spec
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
                //booking

            }
            var user = Session["us_usrname"];
            string query5 = "select * from booking where booking_usr = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query5, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", user);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.booking_usr = dr["booking_usr"].ToString();
                    detail.Add(detailo);
                }
            }

            return View(detail);
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
            string query1 = "select * from roomid where kost_id_room = @kost_id and (room_status = 'nothing')";
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
                    detail.Add(detailo);
                }
            }



            return View(detail);
        }
        [HttpPost]
        public ActionResult booking(FormCollection form, DateTime startbooking, int harga)
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
            string name = form["name"];
            string telp = form["telp"];
            string email = form["email"];
            string major = form["major"];
            string floor = form["floor"];
            string room = form["room"];
            var kost_id = form["kost_id"];
            var periode = form["period"];
            var price = form["harga"];
            int per = int.Parse(periode);
            int total = harga * per;
            int kosid = int.Parse(kost_id);
            var username = Session["us_usrname"];
            SqlCommand checktoyprd = new SqlCommand("select * from booking where booking_usr = '" + username + "'", myConnection);
            SqlDataAdapter sdprd = new SqlDataAdapter(checktoyprd);
            DataTable dtprd = new DataTable();
            sdprd.Fill(dtprd);
            if (dtprd.Rows.Count > 0)
            {
                TempData["alreadybooked"] = "already booked";
                var balikk = "Boking?kost_id=" + kost_id;
                return Redirect(balikk);
            }
            else
            {
                string query = @"INSERT INTO booking(NAME, NOTELP, EMAIL, MAJOR, FLOOR, NUMBER_ROOM, START, periode, kost_id, booking_usr, total_book) " +
                        "values (@NAME, @NOTELP, @EMAIL, @MAJOR, @FLOOR, @NUMBER_ROOM, @START, @periode, @kost_id, @booking_usr, @total_book)";
                SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
                sqlconn.Open();
                sqlcomm.Parameters.AddWithValue("@NAME", name);
                sqlcomm.Parameters.AddWithValue("@kost_id", kosid);
                sqlcomm.Parameters.AddWithValue("@NOTELP", telp);
                sqlcomm.Parameters.AddWithValue("@EMAIL", email);
                sqlcomm.Parameters.AddWithValue("@MAJOR", major);
                sqlcomm.Parameters.AddWithValue("@FLOOR", floor);
                sqlcomm.Parameters.AddWithValue("@NUMBER_ROOM", room);
                sqlcomm.Parameters.AddWithValue("@START", startbooking);
                sqlcomm.Parameters.AddWithValue("@periode", per);
                sqlcomm.Parameters.AddWithValue("@total_book", total);
                sqlcomm.Parameters.AddWithValue("@booking_usr", username);
                sqlcomm.ExecuteNonQuery();
                //
                SqlConnection sqlconn1 = new SqlConnection(mainconn);
                string query1 = @"update roomid set room_status = @room_status where kost_id_room = @kost_id_room and room_name = @room_name";
                SqlCommand sqlcomm1 = new SqlCommand(query1, sqlconn1);
                sqlconn1.Open();
                sqlcomm1.Parameters.AddWithValue("@room_status", "waiting");
                sqlcomm1.Parameters.AddWithValue("@kost_id_room", kosid);
                sqlcomm1.Parameters.AddWithValue("@room_name", room);
               
                sqlcomm1.ExecuteNonQuery();
                sqlconn1.Close();               
            }
            TempData["messsage"] = "succes";
            sqlconn.Close();
            myConnection.Close();
            var balik = "book?kost_id=" + kost_id + "&booking_usr=" + username;
            return Redirect(balik);
        }
        public ActionResult book(int kost_id, string booking_usr)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            List<detailowner> detail = new List<detailowner>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string querykos = "select * from booking where kost_id = @kost_id and booking_usr = @booking_usr";
            using (SqlCommand cmd = new SqlCommand(querykos, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);
                cmd.Parameters.AddWithValue("@booking_usr", booking_usr);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.booking_id = (int)dr["booking_id"];
                    detailo.NAME = dr["NAME"].ToString();
                    detailo.NOTELP = dr["NOTELP"].ToString();
                    detailo.EMAIL = dr["EMAIL"].ToString();
                    detailo.MAJOR = dr["MAJOR"].ToString();
                    detailo.FLOOR = dr["FLOOR"].ToString();
                    detailo.NUMBER_ROOM = dr["NUMBER_ROOM"].ToString();
                    detailo.START = (DateTime)dr["START"];
                    detailo.kost_id = (int)dr["kost_id"];
                    detailo.periode = (int)dr["periode"];
                    detailo.booking_usr = dr["booking_usr"].ToString();
                    detailo.total_book = (int)dr["total_book"];

                    detail.Add(detailo);
                }
            }
            return View(detail);
        }
        public ActionResult payment(int booking_id, int kost_id)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<detailowner> detail = new List<detailowner>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);

            string querykos = "select * from booking where booking_id = @booking_id";
            using (SqlCommand cmd = new SqlCommand(querykos, sqlconn))
            {
                cmd.Parameters.AddWithValue("@booking_id", booking_id);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.booking_id = (int)dr["booking_id"];
                    detailo.NAME = dr["NAME"].ToString();
                    detailo.NOTELP = dr["NOTELP"].ToString();
                    detailo.EMAIL = dr["EMAIL"].ToString();
                    detailo.MAJOR = dr["MAJOR"].ToString();
                    detailo.FLOOR = dr["FLOOR"].ToString();
                    detailo.NUMBER_ROOM = dr["NUMBER_ROOM"].ToString();
                    detailo.START = (DateTime)dr["START"];
                    detailo.kost_id = (int)dr["kost_id"];
                    detailo.periode = (int)dr["periode"];
                    detailo.booking_usr = dr["booking_usr"].ToString();
                    detailo.total_book = (int)dr["total_book"];
                    detailo.method_pay = dr["method_pay"].ToString();
                    
                    detail.Add(detailo);
                }
            }
            
            string query = "select * from paymentid where booking_id = @booking_id";
            using (SqlCommand cmd = new SqlCommand(query, sqlconn))
            {
                cmd.Parameters.AddWithValue("@booking_id", booking_id);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.payment_id = (int)dr["payment_id"];
                    detailo.booking_id = (int)dr["booking_id"];
                    detailo.sender = dr["sender"].ToString();
                    detailo.norekname = dr["norekname"].ToString();
                    detailo.bank = dr["bank"].ToString();
                    detailo.norek = dr["norek"].ToString();
                    detailo.bukti = dr["bukti"].ToString();
                    detailo.pay_usr = dr["pay_usr"].ToString();
                    detailo.btn_status_pay = dr["btn_status_pay"].ToString();
                    detailo.pay_status = dr["pay_status"].ToString();
                    detail.Add(detailo);
                }
            }

            string query1 = "select * from kosanid where kost_id = @kost_id";
            using (SqlCommand cmd = new SqlCommand(query1, sqlconn))
            {
                cmd.Parameters.AddWithValue("@kost_id", kost_id);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.kost_id = (int)dr["kost_id"];
                    detailo.kost_norek = dr["kost_norek"].ToString();
                    detailo.kost_bank = dr["kost_bank"].ToString();
                    detailo.kost_kostname = dr["kost_kostname"].ToString();
                    detailo.kost_notelp = dr["kost_notelp"].ToString();
                    detail.Add(detailo);
                }
            }
            return View(detail);
        }
        public ActionResult paymethod(FormCollection form, int booking_id, int kost_id)
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
            string method = form["method"];
            string query = @"UPDATE booking SET  method_pay = @method_pay WHERE booking_id = @booking_id";
            SqlCommand sqlcomm = new SqlCommand(query, sqlconn);
            sqlconn.Open();
            sqlcomm.Parameters.AddWithValue("@booking_id", booking_id);
            sqlcomm.Parameters.AddWithValue("@method_pay", method);


            sqlcomm.ExecuteNonQuery();


            sqlconn.Close();
            var balik = "payment?booking_id=" + booking_id + "&kost_id=" + kost_id;
            myConnection.Close();
            return Redirect(balik);
        }

        public ActionResult checkout()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string username = Session["us_usrname"].ToString();
            List<detailowner> detail = new List<detailowner>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string querykos = "select * from booking where booking_usr = @booking_usr";
            using (SqlCommand cmd = new SqlCommand(querykos, sqlconn))
            {
                cmd.Parameters.AddWithValue("@booking_usr", username);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.booking_id = (int)dr["booking_id"];
                    detailo.NAME = dr["NAME"].ToString();
                    detailo.NOTELP = dr["NOTELP"].ToString();
                    detailo.EMAIL = dr["EMAIL"].ToString();
                    detailo.MAJOR = dr["MAJOR"].ToString();
                    detailo.FLOOR = dr["FLOOR"].ToString();
                    detailo.NUMBER_ROOM = dr["NUMBER_ROOM"].ToString();
                    detailo.START = (DateTime)dr["START"];
                    detailo.kost_id = (int)dr["kost_id"];
                    detailo.periode = (int)dr["periode"];
                    detailo.booking_usr = dr["booking_usr"].ToString();
                    detailo.total_book = (int)dr["total_book"];
                    detailo.method_pay = dr["method_pay"].ToString();                   
                    detail.Add(detailo);

                    var kost_id = detailo.kost_id;
                    string query1 = "select * from kosanid where kost_id = @kost_id";
                    using (SqlCommand cmd1 = new SqlCommand(query1, sqlconn))
                    {
                        cmd1.Parameters.AddWithValue("@kost_id", kost_id);

                        SqlDataAdapter sda1 = new SqlDataAdapter(cmd1);
                        DataTable dt1 = new DataTable();
                        sda1.Fill(dt1);
                        foreach (DataRow dr1 in dt1.Rows)
                        {
                            detailowner detailo1 = new detailowner();
                            detailo1.kost_id = (int)dr1["kost_id"];
                            detailo1.kost_notelp = dr1["kost_notelp"].ToString();
                            detail.Add(detailo1);
                        }
                    }

                }

            }
            string query = "select * from paymentid where pay_usr = @pay_usr";
            using (SqlCommand cmd = new SqlCommand(query, sqlconn))
            {
                cmd.Parameters.AddWithValue("@pay_usr", username);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.payment_id = (int)dr["payment_id"];
                    detailo.booking_id = (int)dr["booking_id"];
                    detailo.sender = dr["sender"].ToString();
                    detailo.norekname = dr["norekname"].ToString();
                    detailo.bank = dr["bank"].ToString();
                    detailo.norek = dr["norek"].ToString();
                    detailo.bukti = dr["bukti"].ToString();
                    detailo.pay_usr = dr["pay_usr"].ToString();
                    detailo.pay_status = dr["pay_status"].ToString();
                    detail.Add(detailo);
                }
            }
            
            return View(detail);
        }

        [HttpPost]
        public ActionResult paycash(HttpPostedFileBase file, FormCollection form)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string btn_status = "A";
            string sender = form["sender"];
            string norekname = form["norekname"];
            string bank = form["bank"];
            var booking_id = form["booking_id"];
            int book = int.Parse(booking_id);
            var kost_id = form["kost_id"];
            //int kost_id = int.Parse(kost);

            string status = "waiting";
            string norek = form["norek"];
            string kwitansi = form["kwitansi"];
            string username = Session["us_usrname"].ToString();
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();


            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/document/payment/"), filename);
                if (file.ContentLength > 0)
                {
                    string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(mainconn);

                    //send mail
                    //ToMail = form["ToMail"];
                    //subject = "file already uploaded";
                    //var emailfrom = "vickyjendranugraha.danur@mattel.com";
                    //var password = "Tgl10042000.";
                    //SmtpClient smtpClient = new SmtpClient();
                    //smtpClient.Host = "10.1.30.16";
                    //smtpClient.Port = 25;
                    //smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Credentials = new System.Net.NetworkCredential(emailfrom, password);
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.EnableSsl = false;

                    //MailMessage mail = new MailMessage();

                    //var from = emailfrom;

                    //string[] Multi = ToMail.Split(',');
                    //foreach (string Multimail in Multi)
                    //{
                    //    mail.To.Add(new MailAddress(Multimail));
                    //}

                    //mail.From = new MailAddress(from);
                    //mail.Body = PopulateBody(subject);

                    //mail.Subject = "MQ2 System Information " + filetype + " Uploaded for " + pd_toynum + " By " + username;
                    //mail.IsBodyHtml = true;
                    //smtpClient.Send(mail);

                    string queryupdate = "INSERT INTO paymentid (bukti, booking_id, pay_usr, btn_status_pay, pay_status) " +
                        "values (@bukti, @booking_id, @pay_usr, @btn_status_pay, @pay_status)";

                    SqlCommand sqlcomm = new SqlCommand(queryupdate, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@btn_status_pay", btn_status);
                  
                    sqlcomm.Parameters.AddWithValue("@bukti", filename);
                    sqlcomm.Parameters.AddWithValue("@booking_id", book);
                   
                    sqlcomm.Parameters.AddWithValue("@pay_usr", username);
                    sqlcomm.Parameters.AddWithValue("@pay_status", "waiting");
                    file.SaveAs(filepath);
                    TempData["AlertMessage"] = "Payment Uploaded Succesfully";
                    sqlcomm.ExecuteNonQuery();
                    //}
                    sqlconn.Close();
                    ViewData["Message"] = "sukses";
                }
            }

            var balik = "payment?booking_id=" + booking_id + "&kost_id=" + kost_id;
            myConnection.Close();
            return Redirect(balik);
        }
        [HttpPost]
        public ActionResult pay(HttpPostedFileBase file, FormCollection form, string subject, string ToMail)
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string btn_status = "A";
            string sender = form["sender"];
            string norekname = form["norekname"];
            string bank = form["bank"];
            var booking_id = form["booking_id"];
            int book = int.Parse(booking_id);
            var kost_id = form["kost_id"];
            //int kost_id = int.Parse(kost);

            string status = "waiting";
            string norek = form["norek"];
            string bukti = form["bukti"];
            string username = Session["us_usrname"].ToString();
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            

            if (file != null)
            {
                string filename = Path.GetFileName(file.FileName);
                string filepath = Path.Combine(Server.MapPath("~/document/payment/"), filename);
                if (file.ContentLength > 0)
                {
                    string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
                    SqlConnection sqlconn = new SqlConnection(mainconn);

                    //send mail
                    //ToMail = form["ToMail"];
                    //subject = "file already uploaded";
                    //var emailfrom = "vickyjendranugraha.danur@mattel.com";
                    //var password = "Tgl10042000.";
                    //SmtpClient smtpClient = new SmtpClient();
                    //smtpClient.Host = "10.1.30.16";
                    //smtpClient.Port = 25;
                    //smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Credentials = new System.Net.NetworkCredential(emailfrom, password);
                    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.EnableSsl = false;

                    //MailMessage mail = new MailMessage();

                    //var from = emailfrom;

                    //string[] Multi = ToMail.Split(',');
                    //foreach (string Multimail in Multi)
                    //{
                    //    mail.To.Add(new MailAddress(Multimail));
                    //}

                    //mail.From = new MailAddress(from);
                    //mail.Body = PopulateBody(subject);

                    //mail.Subject = "MQ2 System Information " + filetype + " Uploaded for " + pd_toynum + " By " + username;
                    //mail.IsBodyHtml = true;
                    //smtpClient.Send(mail);

                    string queryupdate = "INSERT INTO paymentid (sender, bank, norekname, norek, bukti, booking_id, pay_usr, btn_status_pay, pay_status) " +
                        "values (@sender, @norekname, @bank, @norek, @bukti, @booking_id, @pay_usr, @btn_status_pay, @pay_status)";

                    SqlCommand sqlcomm = new SqlCommand(queryupdate, sqlconn);
                    sqlconn.Open();
                    sqlcomm.Parameters.AddWithValue("@btn_status_pay", btn_status);
                    sqlcomm.Parameters.AddWithValue("@sender", sender);                  
                    sqlcomm.Parameters.AddWithValue("@bank", bank);
                    sqlcomm.Parameters.AddWithValue("@norek", norek);
                    sqlcomm.Parameters.AddWithValue("@bukti", filename);
                    sqlcomm.Parameters.AddWithValue("@booking_id", book);
                    sqlcomm.Parameters.AddWithValue("@norekname", norekname);
                    sqlcomm.Parameters.AddWithValue("@pay_usr", username);
                    sqlcomm.Parameters.AddWithValue("@pay_status", "waiting");
                    file.SaveAs(filepath);

                    
                    TempData["AlertMessage"] = "Payment Uploaded Succesfully";
                    sqlcomm.ExecuteNonQuery();
                    //}
                    sqlconn.Close();
                    ViewData["Message"] = "sukses";
                }
            }
           
            var balik = "payment?booking_id=" + booking_id + "&kost_id=" + kost_id;
            myConnection.Close();
            return Redirect(balik);
        }

        public ActionResult MyKost()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //if (Session["us_usrname"] != Session["us_usrname_secret"])
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            if (Session["kost_id"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
              var kost_id = Session["kost_id"];
            var username = Session["us_usrname"];
            List<detailowner> detail = new List<detailowner>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
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
                    detailo.kost_photo = dr["kost_photo"].ToString();
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
            }
            //booking
            string query5 = "select * from booking where booking_usr = @booking_usr";
            using (SqlCommand cmd = new SqlCommand(query5, sqlconn))
            {
                cmd.Parameters.AddWithValue("@booking_usr", username);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    detailowner detailo = new detailowner();
                    detailo.START = (DateTime)dr["START"];
                    detailo.periode = (int)dr["periode"];
                    detailo.FLOOR = dr["FLOOR"].ToString();
                    detailo.NUMBER_ROOM = dr["NUMBER_ROOM"].ToString();
                    detail.Add(detailo);
                }
            }
            return View(detail);
        }

        selectroom.db dblayer = new selectroom.db();
        public JsonResult State_Bind(string kost_id)
        {

            DataSet ds = dblayer.Get_TorsoType(kost_id);
            List<detailowner> detail = new List<detailowner>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                detail.Add(new detailowner
                {
                    //kost_id = (int)dr["kost_id"],
                    room_floor = dr["room_floor"].ToString(),
                    kost_id_room = (int)dr["kost_id_room"],
                    //sw_charName = Convert.ToString(dr["sw_charName"]),
                    room_name = dr["room_name"].ToString()
                    //sw_charID = Convert.ToInt32(dr["sw_charID"]),
                    //trs_ID = Convert.ToInt32(dr["sw_charID"]),
                    //trs_name = Convert.ToString(dr["sw_charName"])
                }); ;
            }
            return Json(detail, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult product(FormCollection form)
        {
            List<kosan> koss = new List<kosan>();
            string mainconn = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "select * from kosanid";
            var input = form["pilih"];
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
                //kos.kost_location = dr["kost_location"].ToString();
                kos.kost_lat = dr["kost_lat"].ToString();
                kos.kost_long = dr["kost_long"].ToString();
                kos.kost_photo = dr["kost_photo"].ToString();
                koss.Add(kos);
               
               
                //var input = form["pilih"];
                
               
            }
           

            return Redirect("~/Home/Index");
        }
    }
}
   



