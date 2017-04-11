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
using PagedList;
using System.Web.Routing;

namespace ClaimReport.Controllers
{
    public class ClaimsController : Controller
    {
        private ReportClaimEntities db = new ReportClaimEntities();

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = (User)Session["user"];
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                if (user.UserType.name != "Student")
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "NoPermission" }));
                }
            }

            base.OnActionExecuted(filterContext);
        }

        // GET: Claims
        public ActionResult Index(int? page, string txtSearch, int? result)
        {
            if (page == null)
                page = 1;
            if (txtSearch == null)
                txtSearch = "";
            IQueryable<Claim> lstClaim = null;
            var user = (User)Session["user"];
            Student student = db.Students.FirstOrDefault(st => st.userid == user.id);
            if (result == null && user != null)
            {
                lstClaim = db.Claims.Where(c => c.status == true && c.name.Contains(txtSearch) && c.studentid == student.id).Include(c => c.Item).Include(c => c.Coordinator).Include(c => c.Student).OrderByDescending(c => c.datesubmited);
            }
            else
            {
                lstClaim = db.Claims.Where(c => c.status == true && c.name.Contains(txtSearch) && c.result== result && c.studentid == user.id).Include(c => c.Item).Include(c => c.Coordinator).Include(c => c.Student).OrderByDescending(c => c.datesubmited);
            }
            IPagedList<Claim> claims = lstClaim.ToPagedList((int)page, 5);
            return View(claims);
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
            List<Evidence> lstEvidence = db.Evidences.Where(e => e.claimid == claim.id && e.status == true).ToList();
            ViewBag.lstEvidence = lstEvidence;
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            InitViewBagCreate();
            return View();
        }

        public void InitViewBagCreate()
        {
            var user = (User)Session["user"];
            Student student = db.Students.FirstOrDefault(st => st.userid == user.id);
            //Init viewbag
            Academyyear year = db.Academyyears.FirstOrDefault(c => DateTime.Compare(DateTime.Now, (DateTime)c.startReportDate) > 0
    && (DateTime.Compare(DateTime.Now, (DateTime)c.closureReportDate) < 0));
            if(student == null)
            {
                RedirectToAction("Logout", "Login");
            }
            if (year != null && student != null)
            {
                ViewBag.assessmentid = new SelectList(db.Assessments.Where(a => a.academyyearId == year.id && a.facultyid==student.facultyid), "id", "name");
            }
            if (db.Assessments.Where(a => a.academyyearId == year.id).ToList().Count > 0)
            {
                Assessment ass = db.Assessments.Where(a => a.academyyearId == year.id).ToList()[0];
                ViewBag.itemId = new SelectList(db.Items.Where(i => i.assessmentId == ass.id), "id", "name");
            }
            else
            {
                ViewBag.itemId = new SelectList(db.Items.Where(i => i.id < 0));
            }
        }

        public ActionResult GetItemByAssessment(int id)
        {
            List<SelectListItem> listItem = new List<SelectListItem>();
            //The below code is hardcoded for demo. you mat replace with DB data 
            //based on the  input coming to this method ( product id)
            List<Item> lst = db.Items.Where(i => i.assessmentId == id).ToList();
            foreach(Item i in lst)
            {
                listItem.Add(new SelectListItem { Value = i.id+"", Text=i.name });
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,description,itemId")] Claim claim, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                User user = (User)Session["user"];
                Item item = db.Items.FirstOrDefault(a => a.id == claim.itemId);
                var student = db.Students.FirstOrDefault(s => s.userid == user.id && s.status == true);
                if (student != null && item != null)
                {
                    InitViewBagCreate();
                    claim.studentid = student.id;
                    var coordinator = db.Coordinators.FirstOrDefault(c => c.facutyid == student.facultyid && c.status == true);
                    if (coordinator != null && DateTime.Compare(DateTime.Now, (DateTime)item.closureReportDate) < 0)
                    {
                        claim.coordinatorId = coordinator.id;
                        claim.datesubmited = DateTime.Now;
                        claim.result = 0;
                        claim.status = true;
                        db.Claims.Add(claim);
                        db.SaveChanges();

                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                        mail.From = new MailAddress("claimreportcenter@gmail.com");
                        mail.To.Add(coordinator.User.email);
                        mail.Subject = "There is a new claim from student " + student.User.name;
                        mail.Body = "Please verify the report in this mail during 14 days";

                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("claimreportcenter@gmail.com", "123456aA@");
                        SmtpServer.EnableSsl = true;

                        SmtpServer.Send(mail);
                        foreach (var file in files)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                if (DateTime.Compare((DateTime)claim.datesubmited, (DateTime)item.closureEvidenceDate) < 0)
                                {
                                    var fileName = Path.GetFileName(file.FileName);
                                    var myUniqueFileName = string.Format(@"{0}.txt", DateTime.Now.Ticks);
                                    myUniqueFileName = myUniqueFileName.Substring(0, 8);
                                    myUniqueFileName = myUniqueFileName.Replace(",", "");
                                    fileName = myUniqueFileName + fileName;
                                    Evidence e = new Evidence();
                                    e.claimid = claim.id;
                                    e.filename = fileName;
                                    e.dateUpload = claim.datesubmited;
                                    e.status = true;
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
                                else
                                {
                                    ModelState.AddModelError("itemId", "The time to upload evidence is end");
                                    return View();
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("itemId", "The time to upload claim is end");
                        return View();
                    }
                }
                return RedirectToAction("Index");
            }

            ViewBag.academyyearid = new SelectList(db.Academyyears, "id", "id", claim.itemId);
            ViewBag.coordinatorId = new SelectList(db.Coordinators, "id", "id", claim.coordinatorId);
            ViewBag.studentid = new SelectList(db.Students, "id", "id", claim.studentid);
            return View(claim);
        }

        // GET: Claims/Edit/5
        public ActionResult AddEvidence(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.id = id;
            return View();
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEvidence(int? id, IEnumerable<HttpPostedFileBase> files)
        {
            String message = "";
            Claim claim = db.Claims.FirstOrDefault(c => c.id == (int)id);
            if (claim != null)
            {
                if (DateTime.Compare((DateTime)claim.datesubmited, (DateTime)claim.Item.closureEvidenceDate) < 0)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            Evidence e = new Evidence();
                            e.claimid = claim.id;
                            Random random = new Random();
                            int i = random.Next(10000000);
                            fileName = i + fileName;
                            e.filename = fileName;
                            e.status = true;
                            e.dateUpload = DateTime.Now;
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
                            message += "Upload evidence name " + file.FileName + " successfully \n";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "The time to upload evidence is end");
                }
            }
            else
            {
                ModelState.AddModelError(String.Empty, "There is a problem with student account, please log in again");
            }
            ViewBag.message = message;
            return View();
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
            claim.status = false;
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public FileResult Download(string ImageName)
        {
            return File(Path.Combine(Server.MapPath("~/Evidence"), ImageName), System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
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
