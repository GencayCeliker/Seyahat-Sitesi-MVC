using SeyahatMVC25B.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SeyahatMVC25B.Controllers
{
    public class AnaSayfaController : Controller
    {
        DbSeyahat25BEntities db = new DbSeyahat25BEntities();
        Sinifim cs = new Sinifim();

        public ActionResult Index()
        {
            cs.bloglistesi = db.TBLBLOG.Where(x => x.DURUM == true).ToList();

            cs.kategoriler = db.TBLKATEGORI.ToList();

            cs.blogresimleri = db.TBLBLOGRESIMLER.ToList();

            cs.hakkimda = db.TBLHAKKIMDA.FirstOrDefault();

            return View(cs);
        }

        public ActionResult Detay(int id)
        {
            var bul = db.TBLBLOG.Find(id);
            ViewBag.il = bul.SEHIR;
            bul.GORUNTULENME++;
            db.SaveChanges();
            return View(bul);
        }

        public PartialViewResult PartialSongeziler()
        {
            var geziler = db.TBLBLOG.OrderByDescending(x => x.BLOGID).ToList();

            return PartialView(geziler);
        }

        public PartialViewResult PartialYorumlar(int id)
        {
            var yorumlar = db.TBLYORUMLAR.Where(x => x.BLOGID == id && x.DURUM == true).ToList();

            return PartialView(yorumlar);
        }

        public void KodOlustur()
        {
            Random rnd=new Random();
            int s1=rnd.Next(1,50);
            int s2=rnd.Next(1,10);

            Session["dogrulama"] = s1 + s2;

            ViewBag.soru = $"{s1} +{s2}=?";
        }
        public PartialViewResult PartialYorumKaydet(int? id)
        {
            ViewBag.idd = id;

            KodOlustur();

            return PartialView();
        }
        public PartialViewResult PartialYorumaYorum(int id)
        {
            var yorumlar = db.TBLYORUMLAR.Where(x => x.BLOGID == id && x.DURUM == true).ToList();

            ViewBag.idd = id;

            return PartialView(yorumlar);
        }
      
        [HttpPost]
        public PartialViewResult PartialYorumKaydet(TBLYORUMLAR y, int kod)
        {
            int dk = Convert.ToInt32(Session["dogrulama"]);

            if (dk != kod)
            {
                ViewBag.error = "Doğrulama Yanlış!!!";

                KodOlustur();

                return PartialView(y);
            }

            TBLYORUMLAR yeni = new TBLYORUMLAR();

            yeni.ADSOYAD = y.ADSOYAD;
            yeni.YILDIZ = y.YILDIZ;
            yeni.TARIH = DateTime.Now;
            yeni.YORUM = y.YORUM;
            yeni.BLOGID = y.BLOGID;
            yeni.USTYID = y.USTYID;
            yeni.DURUM = true;

            db.TBLYORUMLAR.Add(yeni);
            db.SaveChanges();

            Response.Redirect("/AnaSayfa/Detay/" + y.BLOGID);

            return PartialView();
        }
        [HttpPost]
        public PartialViewResult PartialYorumKaydet2(TBLYORUMLAR y) 
            //BUNU YORUMA YORUM YAPARKEN DOĞRULAMA KODU ALMADAN YAPMAK İÇİN YAPTIK
        {
            TBLYORUMLAR yeni = new TBLYORUMLAR();

            yeni.ADSOYAD = y.ADSOYAD;
            yeni.YILDIZ = y.YILDIZ;
            yeni.TARIH = DateTime.Now;
            yeni.YORUM = y.YORUM;
            yeni.BLOGID = y.BLOGID;
            yeni.USTYID = y.USTYID;
            yeni.DURUM = true;

            db.TBLYORUMLAR.Add(yeni);
            db.SaveChanges();

            Response.Redirect("/AnaSayfa/Detay/" + y.BLOGID);

            return PartialView();
        }
        public ActionResult IcerikGoster(int id)
        {
            var icerikbul=db.TBLICERIK.Find(id);

            return View(icerikbul);
        }

        public ActionResult hava()
        {
            return View();
        }

       

        [HttpPost]
        public JsonResult YorumEkle(TBLYORUMLAR y)
        {
            y.TARIH = DateTime.Now;
            y.DURUM = true;

            db.TBLYORUMLAR.Add(y);
            db.SaveChanges();

            return Json(true);
        }
        public PartialViewResult YorumListesi(int blogId)
        {
            var yorumlar = db.TBLYORUMLAR
                .Where(x => x.BLOGID == blogId)
                .ToList();

            return PartialView("YorumListesi", yorumlar);
        }
    }
}