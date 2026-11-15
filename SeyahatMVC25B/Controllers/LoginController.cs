using SeyahatMVC25B.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace SeyahatMVC25B.Controllers
{
    public class LoginController : Controller
    {
        DbSeyahat25BEntities db = new DbSeyahat25BEntities();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(TBLKULLANICI k)
        {
            var varmi = db.TBLKULLANICI.Where(x => x.KULLANICIADI == k.KULLANICIADI && x.SIFRE == k.SIFRE && x.DURUM == true).FirstOrDefault();

            if (varmi != null && varmi.YETKI == "Y")
            {
                FormsAuthentication.SetAuthCookie(varmi.MAIL, false);
                Session["mail"] = varmi.MAIL;
                Session["username"] = varmi.KULLANICIADI;
                Session["adsoyad"] = varmi.ADSOYAD;
                Session["yetki"] = varmi.YETKI;
                return RedirectToAction("Index", "AdminAnaSayfa");
            }
            else if (varmi != null && varmi.YETKI == "K")
            {
                FormsAuthentication.SetAuthCookie(varmi.MAIL, false);
                Session["mail"] = varmi.MAIL;
                Session["username"] = varmi.KULLANICIADI;
                Session["adsoyad"] = varmi.ADSOYAD;
                Session["yetki"] = varmi.YETKI;
                return RedirectToAction("Index","AnaSayfa");
            }
            else
            {
                ViewBag.error = "Kullanıcı adı veya şifre hatalı!!!";
                return View();
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "AnaSayfa");
        }

        public ActionResult UyeOl()
        {
            var iller=new List<string>() {"Adana", "Adıyaman", "Afyon", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "İçel (Mersin)", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce" };

            ViewBag.sehirler = iller;

            return View();
        }

        [HttpPost]
        public ActionResult UyeOl(TBLUYELER k,HttpPostedFileBase RESIM)
        {
            var iller = new List<string>() { "Adana", "Adıyaman", "Afyon", "Ağrı", "Amasya", "Ankara", "Antalya", "Artvin", "Aydın", "Balıkesir", "Bilecik", "Bingöl", "Bitlis", "Bolu", "Burdur", "Bursa", "Çanakkale", "Çankırı", "Çorum", "Denizli", "Diyarbakır", "Edirne", "Elazığ", "Erzincan", "Erzurum", "Eskişehir", "Gaziantep", "Giresun", "Gümüşhane", "Hakkari", "Hatay", "Isparta", "İçel (Mersin)", "İstanbul", "İzmir", "Kars", "Kastamonu", "Kayseri", "Kırklareli", "Kırşehir", "Kocaeli", "Konya", "Kütahya", "Malatya", "Manisa", "Kahramanmaraş", "Mardin", "Muğla", "Muş", "Nevşehir", "Niğde", "Ordu", "Rize", "Sakarya", "Samsun", "Siirt", "Sinop", "Sivas", "Tekirdağ", "Tokat", "Trabzon", "Tunceli", "Şanlıurfa", "Uşak", "Van", "Yozgat", "Zonguldak", "Aksaray", "Bayburt", "Karaman", "Kırıkkale", "Batman", "Şırnak", "Bartın", "Ardahan", "Iğdır", "Yalova", "Karabük", "Kilis", "Osmaniye", "Düzce" };

            ViewBag.sehirler = iller;

            if (!ModelState.IsValid)
            {
                return View();
            }

            TBLUYELER yeni = new TBLUYELER();

            if (RESIM != null)
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
                    return View();
                }
            }
            else
            {
                yeni.RESIM = "user.png";
            }
            yeni.AD = k.AD;
            yeni.SOYAD = k.SOYAD;
            yeni.DOGUMTARIHI = k.DOGUMTARIHI;
            yeni.MESLEK = k.MESLEK;
            yeni.DOGUMYERI = k.DOGUMYERI;
            yeni.CINSIYET=k.CINSIYET;
            yeni.MAIL = k.MAIL;
            yeni.TEL = k.TEL;
            yeni.OGRENIMDURUMU=k.OGRENIMDURUMU;
            yeni.SIFRE = k.SIFRE;
            yeni.DURUM = true;
            db.TBLUYELER.Add(yeni);
            db.SaveChanges();
            TempData["ekle"] = 1;  //mesaj için
            return RedirectToAction("Login");
        }




    }
}
