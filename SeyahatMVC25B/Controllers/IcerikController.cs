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
    public class IcerikController : Controller
    {
        DbSeyahat25BEntities db = new DbSeyahat25BEntities();
        public ActionResult Index()
        {
            var icerikler = db.TBLICERIK.ToList();

            return View(icerikler);
        }

        public ActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]   //summernote için!!!...
        public ActionResult Ekle(TBLICERIK b, HttpPostedFileBase RESIM)
        {

            TBLICERIK yeni = new TBLICERIK();

            if (RESIM != null & RESIM.ContentLength > 0)
            {

                //bu resim projede fazla yer kaplamaması için yeniden ölçeklendirilip isminin SQL e dosyanında bizim 'ResimlerGezi' klasörüne kaydedilmesi gerekir

                var fi = new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır

                var filename = Path.GetFileName(RESIM.FileName);//resmin adını alır

                filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                var path = Path.Combine(Server.MapPath("~/Dosyalar/"), filename);//resmi klasöre at

                RESIM.SaveAs(path);

                yeni.DOSYA = filename;

            }

            yeni.BASLIK = b.BASLIK;
            yeni.TARIH = b.TARIH;
            yeni.ICERIK = b.ICERIK;
            db.TBLICERIK.Add(yeni);
            db.SaveChanges();
            TempData["ekle"] = 1;  //mesaj için
            return RedirectToAction("Index");
        }

        public ActionResult IcerikGetir(int id)
        {
            var icerik = db.TBLICERIK.Find(id);

            return View("IcerikGetir", icerik);
        }

        [ValidateInput(false)]  //summernote için
        public ActionResult Kaydet(TBLICERIK b, HttpPostedFileBase RESIM)
        {
            var gunck = db.TBLICERIK.Find(b.ID);

            if (RESIM != null)
            {
                //kullanıcı dosya değiştirdi demekki!!!
                //o zaman eski dosya silinip yerine yeni dosya kaydedilmeli

                //DOSYA SİL.........................
                var dosyayolu = Request.MapPath("~/Dosyalar/" + gunck.DOSYA);
                if (System.IO.File.Exists(dosyayolu))
                {
                    System.IO.File.Delete(dosyayolu);
                }
                //.................................
                var fi = new FileInfo(RESIM.FileName); //resmin adı ve uzantısını alır

                var filename = Path.GetFileName(RESIM.FileName);//resmin adını alır

                filename = Guid.NewGuid().ToString() + fi.Extension;//isim değiştir

                var path = Path.Combine(Server.MapPath("~/Dosyalar/"), filename);//resmi klasöre at

                RESIM.SaveAs(path);

                gunck.DOSYA = filename;
            }

            gunck.BASLIK = b.BASLIK;
            gunck.TARIH = b.TARIH;           
            gunck.ICERIK = b.ICERIK;                       
            db.SaveChanges();
            TempData["kaydet"] = 1;
            return RedirectToAction("Index");
        }


    }
}