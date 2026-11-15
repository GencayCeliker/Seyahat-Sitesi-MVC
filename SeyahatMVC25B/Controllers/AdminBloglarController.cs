using SeyahatMVC25B.Filters;
using SeyahatMVC25B.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace SeyahatMVC25B.Controllers
{
    [YoneticiYetki]
    public class AdminBloglarController : Controller
    {
        DbSeyahat25BEntities db=new DbSeyahat25BEntities();
        public ActionResult Index()
        {
            var bloglar=db.TBLBLOG.OrderByDescending(x=>x.BLOGID).ToList();
           
            return View(bloglar);
        }

        public ActionResult BlogEkle()
        {
            var kategoriler = db.TBLKATEGORI.ToList();

            ViewBag.ktg=kategoriler;

            return View();
        }


        [HttpPost]
        [ValidateInput(false)]   //summernote için!!!...
           public ActionResult BlogEkle(TBLBLOG b,HttpPostedFileBase RESIM)
        {

            TBLBLOG yeni = new TBLBLOG();

            if (RESIM!=null & RESIM.ContentLength>0)
            {
                if (RESIM.ContentType=="image/jpeg" || RESIM.ContentType == "image/jpg" || RESIM.ContentType == "image/gif" || RESIM.ContentType == "image/png" )
                {
                    //bu resim projede fazla yer kaplamaması için yeniden ölçeklendirilip isminin SQL e dosyanında bizim 'ResimlerGezi' klasörüne kaydedilmesi gerekir

                    var fi=new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır
                 
                    var filename=Path.GetFileName(RESIM.FileName);//resmin adını alır

                    filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                    var path=Path.Combine(Server.MapPath("~/ResimlerGezi/"), filename);//resmi klasöre at

                    //bu resmi gerekirse küçült
                    WebImage rr = new WebImage(RESIM.InputStream);//resmi al

                    if (rr.Width > 1000) rr.Resize(1000, 1000);

                    rr.Save(path); //klasöre eklediğini değiştir

                    yeni.RESIM = filename;                                    
                }
                else
                {
                    return View();//sayfada kal
                }
            }        
            yeni.BASLIK=b.BASLIK;           
            yeni.TARIH=b.TARIH;
            yeni.OZET=b.OZET;
            yeni.ACIKLAMA=b.ACIKLAMA;
            yeni.BEGENI = 0;
            yeni.KATEGORI = b.KATEGORI;
            yeni.GORUNTULENME = 0;
            yeni.YAZAR=b.YAZAR;
            yeni.DURUM = true;
            db.TBLBLOG.Add(yeni);
            db.SaveChanges();
            TempData["ekle"] = 1;  //mesaj için
            return RedirectToAction("Index");
        }


        public ActionResult BlogGetir(int id)
        {
            var blog=db.TBLBLOG.Find(id);

            var kategoriler = db.TBLKATEGORI.ToList();

            ViewBag.ktg = kategoriler;

            return View("BlogGetir", blog);
        }

        [ValidateInput(false)]  //summernote için
        public ActionResult Kaydet(TBLBLOG b,HttpPostedFileBase RESIM)
        {
            var gunck = db.TBLBLOG.Find(b.BLOGID);

            if (RESIM != null)
            {
                if (RESIM.ContentType == "image/jpeg" || RESIM.ContentType == "image/jpg" || RESIM.ContentType == "image/gif" || RESIM.ContentType == "image/png")
                {
                    //kullanıcı resim değiştirdi demekki!!!
                    //o zaman eski resim silinip yerine yeni resim kaydedilmeli
                    
                    //RESMİ SİL.........................
                    var dosyayolu = Request.MapPath("~/ResimlerGezi/" + gunck.RESIM);
                    if (System.IO.File.Exists(dosyayolu))
                    {
                        System.IO.File.Delete(dosyayolu);
                    }
                    //.................................

                    //bu resim projede fazla yer kaplamaması için yeniden ölçeklendirilip isminin SQL e dosyanında bizim 'ResimlerGezi' klasörüne kaydedilmesi gerekir

                    var fi = new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır

                    var filename = Path.GetFileName(RESIM.FileName);//resmin adını alır

                    filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                    var path = Path.Combine(Server.MapPath("~/ResimlerGezi/"), filename);//resmi klasöre at

                    //bu resmi gerekirse küçült
                    WebImage rr = new WebImage(RESIM.InputStream);//resmi al

                    if (rr.Width > 1000) rr.Resize(1000, 1000);

                    rr.Save(path); //klasöre eklediğini değiştir

                    gunck.RESIM = filename;
                }
                else
                {
                    return View();//sayfada kal
                }
            }
            gunck.BASLIK = b.BASLIK;
            gunck.TARIH = b.TARIH;
            gunck.OZET = b.OZET;
            gunck.ACIKLAMA = b.ACIKLAMA;
            //gunck.BEGENI = 0;
            gunck.KATEGORI = b.KATEGORI;
            //gunck.GORUNTULENME = 0;
            gunck.YAZAR = b.YAZAR;
            //gunck.DURUM = true;            
            db.SaveChanges();
            TempData["kaydet"] = 1;
            return RedirectToAction("Index");
        }

        public ActionResult Hakkimda()
        {
            var hakkimda = db.TBLHAKKIMDA.FirstOrDefault();

            return View(hakkimda);
        }

        public ActionResult HakkimdaKaydet(TBLHAKKIMDA h,HttpPostedFileBase RESIM)
        {
            var gunck = db.TBLHAKKIMDA.FirstOrDefault();

            if (RESIM != null)
            {
                if (RESIM.ContentType == "image/jpeg" || RESIM.ContentType == "image/jpg" || RESIM.ContentType == "image/gif" || RESIM.ContentType == "image/png")
                {
                    //kullanıcı resim değiştirdi demekki!!!
                    //o zaman eski resim silinip yerine yeni resim kaydedilmeli

                    //RESMİ SİL.........................
                    var dosyayolu = Request.MapPath("~/ResimlerGezi/" + gunck.FOTO);
                    if (System.IO.File.Exists(dosyayolu))
                    {
                        System.IO.File.Delete(dosyayolu);
                    }
                    //.................................

                    //bu resim projede fazla yer kaplamaması için yeniden ölçeklendirilip isminin SQL e dosyanında bizim 'ResimlerGezi' klasörüne kaydedilmesi gerekir

                    var fi = new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır

                    var filename = Path.GetFileName(RESIM.FileName);//resmin adını alır

                    filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                    var path = Path.Combine(Server.MapPath("~/ResimlerGezi/"), filename);//resmi klasöre at

                    //bu resmi gerekirse küçült
                    WebImage rr = new WebImage(RESIM.InputStream);//resmi al

                    if (rr.Width > 1000) rr.Resize(1000, 1000);

                    rr.Save(path); //klasöre eklediğini değiştir

                    gunck.FOTO = filename;
                }
                else
                {
                    return View();//sayfada kal
                }
            }
            gunck.ADSOYAD = h.ADSOYAD;
            gunck.ADRES = h.ADRES;
            gunck.TEL =h.TEL;
            gunck.INSTAGRAM = h.INSTAGRAM;
            gunck.FACE = h.FACE;
            gunck.HAKKIMDA = h.HAKKIMDA;                    
            db.SaveChanges();
            TempData["kaydet"] = 1;
            return RedirectToAction("Index","AdminAnaSayfa");
        }

        public ActionResult Gorseller(int id)
        {
            var gorseller=db.TBLBLOGRESIMLER.Where(x=>x.BLOG==id).ToList();

            ViewBag.grs = gorseller;

            ViewBag.gezi = db.TBLBLOG.Find(id).BASLIK;

            return View();
        }

        [HttpPost]
        public ActionResult GorselKaydet(TBLBLOGRESIMLER r,HttpPostedFileBase RESIM)
        {
            TBLBLOGRESIMLER yeni = new TBLBLOGRESIMLER();

            if (RESIM != null & RESIM.ContentLength > 0)
            {
                if (RESIM.ContentType == "image/jpeg" || RESIM.ContentType == "image/jpg" || RESIM.ContentType == "image/gif" || RESIM.ContentType == "image/png")
                {
                    //bu resim projede fazla yer kaplamaması için yeniden ölçeklendirilip isminin SQL e dosyanında bizim 'ResimlerGezi' klasörüne kaydedilmesi gerekir

                    var fi = new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır

                    var filename = Path.GetFileName(RESIM.FileName);//resmin adını alır

                    filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                    var path = Path.Combine(Server.MapPath("~/ResimlerGezi/"), filename);//resmi klasöre at

                    //bu resmi gerekirse küçült
                    WebImage rr = new WebImage(RESIM.InputStream);//resmi al

                    if (rr.Width > 1000) rr.Resize(1000, 1000);

                    rr.Save(path); //klasöre eklediğini değiştir

                    yeni.RESIM = filename;
                }
                else
                {
                    return View();//sayfada kal
                }
            }
            yeni.YAZI = r.YAZI;
            yeni.BLOG = r.BLOG;           
            db.TBLBLOGRESIMLER.Add(yeni);
            db.SaveChanges();
            TempData["ekle"] = 1;  //mesaj için
            return RedirectToAction("Gorseller","AdminBloglar");
        }

    }
}