using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClaimReport.Models;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;

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
            
        [HttpPost]
        public ActionResult ForgetPassword(string email)
        {
            User user = db.Users.FirstOrDefault(u => u.email == email);
            if(user != null)
            {
                Random random = new Random();
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                string str = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray());
                byte[] hash;
                using (MD5 md5 = MD5.Create())
                {
                    hash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                }
                user.password = hash;
                db.SaveChanges();
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("claimreportcenter@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Reset the password";
                mail.Body = "Your username is " + user.username +", your new password is : " + str;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("claimreportcenter@gmail.com", "123456aA@");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                TempData["reset"] = "success";
            }
            else
            {
                TempData["reset"] = "fail";
            }
            
            return RedirectToAction("index");
        }
    }
}