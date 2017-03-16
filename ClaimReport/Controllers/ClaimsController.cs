using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClaimReport.Models;
using System.IO;
using System.Net.Mail;

namespace ClaimReport.Controllers
{
    public class ClaimsController : Controller
    {
        private ReportClaimEntities db = new ReportClaimEntities();

        // GET: Claims
        public ActionResult Index()
        {
            var claims = db.Claims.Include(c => c.Academyyear).Include(c => c.Coordinator).Include(c => c.Student);
            return View(claims.ToList());
        }

        // GET: Claims/Details/5
        public ActionResult Details(int? id)
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
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            ViewBag.academyyearid = new SelectList(db.Academyyears, "id", "name");
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,description,academyyearid")] Claim claim, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                User user = (User)Session["user"];
                Academyyear academy = db.Academyyears.FirstOrDefault(a => a.id == claim.academyyearid);
                var student = db.Students.FirstOrDefault(s => s.userid == user.id && s.status == true);
                if (student != null && academy != null)
                {
                    claim.studentid = student.id;
                    User st = db.Users.FirstOrDefault(u => u.id == student.userid && u.status == true);
                    var coordinator = db.Coordinators.FirstOrDefault(c => c.facutyid == student.facultyid && c.status == true);
                    if(coordinator != null && DateTime.Compare(DateTime.Now,(DateTime)academy.closureReportDate) < 0)
                    {
                        claim.coordinatorId = coordinator.id;
                        claim.datesubmited = DateTime.Now;
                        db.Claims.Add(claim);
                        db.SaveChanges();
                        User c = db.Users.FirstOrDefault(u => u.id == coordinator.userid && u.status == true);

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("claimreportcenter@gmail.com");
                        mail.To.Add(c.email);
                        mail.Subject = "There is a new claim from student " + st.name;
                        mail.Body = "Please verify the report in this mail during 14 days";

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("claimreportcenter@gmail.com", "123456aA@");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        foreach (var file in files)
                        {
                            if (file.ContentLength > 0 && DateTime.Compare((DateTime)claim.datesubmited, (DateTime)academy.closureEvidenceDate) < 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                Evidence e = new Evidence();
                                e.claimid = claim.id;
                                e.filename = fileName;
                                if (fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf(".")).Equals(".pdf"))
                                {
                                    e.type = 0;
                                }
                                else
                                {
                                    e.type = 1;
                                }
                                var path = Path.Combine(Server.MapPath("~/Evidence"), fileName);
                                file.SaveAs(path);
                                db.Evidences.Add(e);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                return RedirectToAction("Index");         
            }

            ViewBag.academyyearid = new SelectList(db.Academyyears, "id", "id", claim.academyyearid);
            ViewBag.coordinatorId = new SelectList(db.Coordinators, "id", "id", claim.coordinatorId);
            ViewBag.studentid = new SelectList(db.Students, "id", "id", claim.studentid);
            return View(claim);
        }

        // GET: Claims/Edit/5
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
            ViewBag.academyyearid = new SelectList(db.Academyyears, "id", "id", claim.academyyearid);
            ViewBag.coordinatorId = new SelectList(db.Coordinators, "id", "id", claim.coordinatorId);
            ViewBag.studentid = new SelectList(db.Students, "id", "id", claim.studentid);
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,studentid,academyyearid,coordinatorId,result,status")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.academyyearid = new SelectList(db.Academyyears, "id", "id", claim.academyyearid);
            ViewBag.coordinatorId = new SelectList(db.Coordinators, "id", "id", claim.coordinatorId);
            ViewBag.studentid = new SelectList(db.Students, "id", "id", claim.studentid);
            return View(claim);
        }

        // GET: Claims/Delete/5
        public ActionResult Delete(int? id)
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
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Claim claim = db.Claims.Find(id);
            db.Claims.Remove(claim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
