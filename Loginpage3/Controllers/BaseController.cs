using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loginpage3.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["UserId"] == null && Request.Cookies["UserLogin"] != null)
            {
                HttpCookie authCookie = Request.Cookies["UserLogin"];
                if (authCookie != null)
                {
                    string userId = authCookie.Values["UserId"];
                    string username = authCookie.Values["Username"];

                    // بازیابی Session از کوکی
                    Session["UserId"] = userId;
                    Session["Username"] = username;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}