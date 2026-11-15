using SeyahatMVC25B.Filters;
using SeyahatMVC25B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace SeyahatMVC25B.Controllers
{
    [YoneticiYetki]   //şifresiz bu controller çalışmaz
    public class YorumlarController : Controller
    {
        DbSeyahat25BEntities db=new DbSeyahat25BEntities();
        public ActionResult Index(int sayfa=1)
        {
            var yorumlar = db.TBLYORUMLAR.OrderByDescending(x => x.TARIH).ToList().ToPagedList(sayfa,4);

            return View(yorumlar);
        }

        public ActionResult GosterGizle(int id)
        {
            var bul = db.TBLYORUMLAR.Find(id);
            if (bul.DURUM==false)
            {
                bul.DURUM = true;
            }
            else
            {
                bul.DURUM = false;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult YorumSil(int id)
        {
            var bul = db.TBLYORUMLAR.Find(id);

            db.TBLYORUMLAR.Remove(bul);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}