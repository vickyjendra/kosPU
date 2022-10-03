using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace kosPU.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult lamanawal()
        {
            return View();
        }
        public ActionResult LoginOwner()
        {
            return View();
        }
        public ActionResult registerowner()
        {
            return View();
        }
        public ActionResult registeruser()
        {
            return View();
        }
    }
}