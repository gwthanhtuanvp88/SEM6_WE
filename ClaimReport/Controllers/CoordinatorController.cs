using ClaimReport.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ClaimReport.Controllers
{
    public class CoordinatorController : CoordinatorPermissionController
    {
        private ReportClaimEntities db = new ReportClaimEntities();

        // GET: Coordinator
        public ActionResult Index(int? page)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["user"];
            var coordinator = db.Coordinators.FirstOrDefault(x => x.userid == user.id);

            if (page == null) { page = 1;} 
            var lstClaim = db.Claims.Where(c => c.status == true && c.coordinatorId == coordinator.id).Include(c => c.Item).Include(c => c.Coordinator).Include(c => c.Student).OrderByDescending(c => c.datesubmited);
            IPagedList<Claim> claims = lstClaim.ToPagedList((int)page, 5);
            return View(claims);
        }

        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    Claim claim = db.Claims.Find(id);

        //    if (claim == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    ViewBag.evidence = db.Evidences.Where(x => x.claimid == id).ToList();

        //    return View(claim);
        //}

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = db.Claims.Find(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            List<Evidence> lstEvidence = db.Evidences.Where(e => e.claimid == claim.id && e.status == true).ToList();
            if (lstEvidence.Count == 0)
            {
                ViewBag.result = "No evidence to process this claim";
            }
            ViewBag.lstEvidence = lstEvidence;
            DateTime submited = (DateTime)claim.datesubmited;
            if (DateTime.Compare(DateTime.Now, submited.AddDays(15)) > 0)
            {
                ViewBag.result = "The claim is out of 14 days since claim upload date " + submited.ToShortDateString();
            }
            return View(claim);
        }

        [HttpPost]
        public ActionResult Edit(int id, bool result)
        {
            String strResult = "";
            Claim claim = db.Claims.FirstOrDefault(c => c.id == id);


            if (result)
            {
                claim.result = 1;
                strResult = "Accepted";
            }
            else
            {
                claim.result = 2;
                strResult = "Rejected";
            }
            db.SaveChanges();
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress("claimreportcenter@gmail.com");
            mail.To.Add(claim.Student.User.email);
            mail.Subject = "There is a claim result from teacher " + claim.Coordinator.User.name;
            mail.Body = "The claim " + claim.name + " with description " + claim.description + " is " + strResult;
            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("claimreportcenter@gmail.com", "123456aA@");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return RedirectToAction("Index");
        }
    }
}