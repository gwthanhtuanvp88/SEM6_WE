using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimReport.Models;
using System.Security.Cryptography;
using System.Text;

namespace ClaimReport.Controllers
{
    public class LoginController : Controller
    {
        ReportClaimEntities db = new ReportClaimEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(String username, String password)
        {
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
            var user = db.Users.FirstOrDefault(u => u.username == username && u.password == hash && u.status == true);
            if (user != null)
            {
                Session["user"] = user;
                return RedirectToAction("Index", "Home");
            }
            TempData["login"] = "fail";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Logout");
        }
    }
}