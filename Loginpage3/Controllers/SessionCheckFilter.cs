using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loginpage3.Controllers
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // بررسی اینکه آیا Session کاربر وجود دارد
            if (HttpContext.Current.Session["UserId"] == null)
            {
                // اگر کوکی موجود باشد، Session را بازسازی می‌کنیم
                HttpCookie authCookie = HttpContext.Current.Request.Cookies["UserLogin"];
                if (authCookie != null)
                {
                    HttpContext.Current.Session["UserId"] = authCookie.Values["UserId"];
                    HttpContext.Current.Session["Username"] = authCookie.Values["Username"];
                }
                else
                {
                    // اگر نه Session و نه کوکی وجود داشت، کاربر را به صفحه ورود هدایت می‌کنیم
                    filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                        { "controller", "Account" },
                        { "action", "Login" }
                        });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}