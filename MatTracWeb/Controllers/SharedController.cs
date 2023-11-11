using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MatTracWeb.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult _Layout()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Clear();
            return Redirect("~/Login/Index");
        }
    }
}