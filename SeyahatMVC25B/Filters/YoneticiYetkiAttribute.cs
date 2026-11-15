using System.Web;
using System.Web.Mvc;

namespace SeyahatMVC25B.Filters
{
    /// <summary>
    /// Yönetim sayfalarını yalnızca YETKI = "Y" (yönetici) olan kullanıcılara açar.
    /// [Authorize] sadece giriş yapılmış olmasını kontrol ettiği için "K" (üye) yetkili
    /// kullanıcılar da yönetim paneline girebiliyordu.
    /// </summary>
    public class YoneticiYetkiAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return base.AuthorizeCore(httpContext)
                && httpContext.Session != null
                && httpContext.Session["yetki"] as string == "Y";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login/Login");
        }
    }
}
