using SeyahatMVC25B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeyahatMVC25B.Controllers
{
    public class UyeAraController : Controller
    {
        DbSeyahat25BEntities db=new DbSeyahat25BEntities();
        public ActionResult Index(string sehir)
        {
            sehir = sehir ?? "";

            ViewBag.il = sehir;

            var uyeler=db.TBLUYELER.Where(x=>x.DOGUMYERI.ToLower()==sehir && x.DURUM==true).ToList();

            return View(uyeler);
        }
    }
}