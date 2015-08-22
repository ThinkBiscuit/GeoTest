using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeoTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult PlotPoints()
        {
            return View();
        }

        public ActionResult PlotPointsSockets()
        {
            return View();
        }

        public ActionResult Distance()
        {
            return View();
        }
    }
}