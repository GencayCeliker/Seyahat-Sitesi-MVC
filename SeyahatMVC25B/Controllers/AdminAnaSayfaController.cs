using SeyahatMVC25B.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeyahatMVC25B.Controllers
{
    [YoneticiYetki]
    public class AdminAnaSayfaController : Controller
    {
        // GET: AdminAnaSayfa
        public ActionResult Index()
        {
            return View();
        }
    }
}