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

            var user = (User)Session["user"];
            User cUser = null;
            var cookie = Request.Cookies["user"];

            if (cookie != null)
            {
                int value = Int32.Parse(cookie.Value);
                cUser = db.Users.FirstOrDefault(u => u.id == value);
                if(cUser != null)
                {
                    user = cUser;
                    Session["user"] = user;
                }
            }
            if (user != null)
            {
                if (user.UserType.name == "Administrator")
                {
                    return RedirectToAction("Users", "Administrator");
                }
                else if (user.UserType.name == "Manager")
                {
                    return RedirectToAction("Index", "Manager");
                }
                else if (user.UserType.name == "Coordinator")
                {
                    return RedirectToAction("Index", "Coordinator");
                }
                else
                {
                    return RedirectToAction("Index", "Claims");
                }
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult CheckLogin(String username, String password, bool? remember)
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
                if(remember != null)
                {
                    var cookie = new HttpCookie("user", user.id.ToString());
                    Response.AppendCookie(cookie);
                }
                if (user.UserType.name == "Administrator")
                {
                    return RedirectToAction("Users", "Administrator");
                }
                else if (user.UserType.name == "Manager")
                {
                    return RedirectToAction("Index", "Manager");
                }
                else if (user.UserType.name == "Coordinator")
                {
                    return RedirectToAction("Index", "Coordinator");
                }
                else
                {
                    return RedirectToAction("Index", "Claims");
                }

            }
            ViewBag.strErrorMessage = "Username an password are incorrect";
            TempData["login"] = "fail";
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Logout()
        {
            Session["user"] = null;
            Response.Cookies["user"].Value = "-1";
            return RedirectToAction("Index");
        }

        public ActionResult NoPermission()
        {
            return View();
        }

        public ActionResult ForgetPassword(string email)
        {

            return RedirectToAction("index");
        }
    }
}