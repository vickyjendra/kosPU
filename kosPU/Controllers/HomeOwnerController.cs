using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }
        public ActionResult DetailPenghuni()
        {
            return View();
        }
    }
}