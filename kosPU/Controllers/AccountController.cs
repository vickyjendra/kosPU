using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using kosPU.Models;

namespace kosPU.Controllers
{
    public class AccountController : Controller
    {
        KosanEntities entity = new KosanEntities();
        // GET: Account
        public ActionResult Login()
        {
            if (Session["us_usrname"] != null)
            {
                Session["us_usrname"].ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult Login(LoginViewModel credentails)
        {
            string encrypt1 = encrypt.MD5Hash(credentails.password_user);
            bool userExist = entity.account_user.Any(x => x.username_user == credentails.username_user && x.password_user == encrypt1);
            account_user u = entity.account_user.FirstOrDefault(x => x.username_user == credentails.username_user && x.password_user == encrypt1);

            if (userExist)
            {
                Session["us_usrname"] = u.username_user.ToString();
                FormsAuthentication.SetAuthCookie(u.username_user, false);

                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "udah ada");
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();

        }
        public ActionResult lamanawal()
        {
            return View();
        }
        public ActionResult LoginOwner()
        {
            if (Session["us_usrname"] != null)
            {
                Session["us_usrname"].ToString();
                return RedirectToAction("Index", "HomeOwner");
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public ActionResult LoginOwner(LoginViewModelowner credentails)
        {
            string encrypt1 = encrypt.MD5Hash(credentails.password_owner);
            bool userExist = entity.account_ownerr.Any(x => x.username_owner == credentails.username_owner && x.password_owner == encrypt1);
            account_ownerr u = entity.account_ownerr.FirstOrDefault(x => x.username_owner == credentails.username_owner && x.password_owner == encrypt1);

            if (userExist)
            {
                Session["us_usrname"] = u.username_owner.ToString();
                FormsAuthentication.SetAuthCookie(u.username_owner, false);

                return RedirectToAction("Index", "HomeOwner");
            }
            ModelState.AddModelError("", "udah ada");
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();


        }
        public ActionResult registerowner()
        {

            return View();
        }
        [HttpPost]
        public ActionResult registerowner(FormCollection form)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();


            var name = form["name_user"].ToString();
            var username = form["username_user"].ToString();
            var phone = form["phone_user"].ToString();
            var email = form["email_user"].ToString();
            var password = form["pass_user"].ToString();
            var pass = encrypt.MD5Hash(password);
            var address = form["address_user"].ToString();
            string gender = form["gender"].ToString();
            int group = 1;
            SqlCommand checktusername = new SqlCommand("select * from account_ownerr where username_owner = '" + username + "'", myConnection);
            SqlDataAdapter sd = new SqlDataAdapter(checktusername);
            DataTable dtt = new DataTable();
            sd.Fill(dtt);
            if (dtt.Rows.Count > 0)
            {

            }
            else
            {
                SqlConnection sqlconn = new SqlConnection(connectionString);
                string query7 = "INSERT INTO account_ownerr(name_owner, username_owner, email_owner, password_owner, group_owner, address_owner, phone, gender_owner, create_date)" +
                    " values (@name_owner, @username_owner, @email_owner, @password_owner, @group_owner, @address_owner, @phone, @gender_owner, CURRENT_TIMESTAMP)";
                SqlCommand sqlcomm = new SqlCommand(query7, sqlconn);
                sqlconn.Open();
                sqlcomm.Parameters.AddWithValue("@name_owner", name);
                sqlcomm.Parameters.AddWithValue("@username_owner", username);
                sqlcomm.Parameters.AddWithValue("@email_owner", email);
                sqlcomm.Parameters.AddWithValue("@password_owner", pass);
                sqlcomm.Parameters.AddWithValue("@group_owner", group);
                sqlcomm.Parameters.AddWithValue("@address_owner", address);
                sqlcomm.Parameters.AddWithValue("@phone", phone);
                sqlcomm.Parameters.AddWithValue("@gender_owner", gender);
                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();
            }
            myConnection.Close();
            return Redirect("LoginOwner");
        }
        public ActionResult registeruser()
        {

            return View();
        }
        [HttpPost]
        public ActionResult registeruser(FormCollection form)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            var name = form["name_user"].ToString();
            var username = form["username_user"].ToString();
            var phone = form["phone_user"].ToString();
            var email = form["email_user"].ToString();
            var password = form["pass_user"].ToString();
            var pass = encrypt.MD5Hash(password);
            var address = form["address_user"].ToString();
            string gender = form["gender"].ToString();
            int group = 2;
            SqlCommand checktusername = new SqlCommand("select * from account_user where username_user = '" + username + "'", myConnection);
            SqlDataAdapter sd = new SqlDataAdapter(checktusername);
            DataTable dtt = new DataTable();
            sd.Fill(dtt);
            if (dtt.Rows.Count > 0)
            {
            }
            else
            {
                SqlConnection sqlconn = new SqlConnection(connectionString);
                string query7 = "INSERT INTO account_user(name_user, username_user, email_user, password_user, group_user, address_user, phone_userr, gender, create_date)" +
                    " values (@name_user, @username_user, @email_user, @password_user, @group_user, @address_user, @phone_userr, @gender, CURRENT_TIMESTAMP)";
                SqlCommand sqlcomm = new SqlCommand(query7, sqlconn);
                sqlconn.Open();
                sqlcomm.Parameters.AddWithValue("@name_user", name);
                sqlcomm.Parameters.AddWithValue("@username_user", username);
                sqlcomm.Parameters.AddWithValue("@email_user", email);
                sqlcomm.Parameters.AddWithValue("@password_user", pass);
                sqlcomm.Parameters.AddWithValue("@group_user", group);
                sqlcomm.Parameters.AddWithValue("@address_user", address);
                sqlcomm.Parameters.AddWithValue("@phone_userr", phone);
                sqlcomm.Parameters.AddWithValue("@gender", gender);
                sqlcomm.ExecuteNonQuery();
                sqlconn.Close();
            }
            myConnection.Close();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult SignOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
        public ActionResult SignOutOwner()
        {
            Session.Abandon();
            return RedirectToAction("LoginOwner", "Account");
        }
        public ActionResult MyAccount()
        {
            if (Session["us_usrname"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            string username = Session["us_usrname"].ToString();
            var connectionString = ConfigurationManager.ConnectionStrings["Koneksi"].ConnectionString;
            SqlConnection myConnection = new SqlConnection();
            myConnection.ConnectionString = connectionString;
            myConnection.Open();
            List<myaccount> jc = new List<myaccount>();
            string query1 = @"  select * from account_user where username_user = @username";

            using (SqlCommand cmd = new SqlCommand(query1, myConnection))
            {

                cmd.Parameters.AddWithValue("@username", username);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    myaccount account = new myaccount();
                    account.name_user = dr["name_user"].ToString();
                    account.email_user = dr["email_user"].ToString();
                    account.phone_userr = dr["phone_userr"].ToString();
                    account.gender = dr["gender"].ToString();
                    account.address_user = dr["address_user"].ToString();
                    account.username_user = dr["username_user"].ToString();
                    jc.Add(account);
                }

               


                return View(jc);

            }

        }
    }
}