using Loginpage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loginpage3.Controllers
{
    public class AccountController :  BaseController
    {
        private readonly AppDbContext _context = new AppDbContext();
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, bool RememberMe = false)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

                if (user != null)
                {
                    // ذخیره Session
                    Session["UserId"] = user.Id;
                    Session["Username"] = user.Username;

                    // ایجاد کوکی اگر کاربر "مرا به خاطر بسپار" را انتخاب کرده باشد
                    if (RememberMe)
                    {
                        HttpCookie authCookie = new HttpCookie("UserLogin");
                        authCookie.Values["UserId"] = user.Id.ToString();
                        authCookie.Values["Username"] = user.Username;
                        authCookie.Expires = DateTime.Now.AddDays(30); // اعتبار کوکی برای 30 روز
                        Response.Cookies.Add(authCookie);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "نام کاربری یا رمز عبور نادرست است.");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();

            // حذف کوکی
            if (Request.Cookies["UserLogin"] != null)
            {
                HttpCookie authCookie = new HttpCookie("UserLogin");
                authCookie.Expires = DateTime.Now.AddDays(-2); // تاریخ انقضا گذشته
                Response.Cookies.Add(authCookie);
            }

            return RedirectToAction("Login");
        }
    }
}