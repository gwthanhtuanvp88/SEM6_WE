using ClaimReport.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClaimReport.Controllers
{
    public class ManagerController : Controller
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
                if (user.UserType.name != "Manager")
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                }
            }

            base.OnActionExecuted(filterContext);
        }

        // GET: Manager
        public ActionResult Index(int? page, string txtSearch)
        {
            if (page == null) { page = 1; }
            IQueryable<Claim> lstClaim = null;
            if (String.IsNullOrEmpty(txtSearch))
            {
                lstClaim = db.Claims.Where(x => x.status == true).OrderByDescending(x => x.datesubmited);
            }
            else
            {
                lstClaim = db.Claims.Where(x => x.status == true && x.name.Contains(txtSearch)).OrderByDescending(x => x.datesubmited);
                ViewBag.ViewAll = true;
            }
            IPagedList<Claim> claims = lstClaim.ToPagedList((int)page, 5);
            return View(claims);
        }

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

        public FileResult Download(string ImageName)
        {
            return File(Path.Combine(Server.MapPath("~/Evidence"), ImageName), System.Net.Mime.MediaTypeNames.Application.Octet, ImageName);
        }

        public ActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Number_of_claims_within_each_Faculty_for_each_academic_year()
        {
            List<Academyyear> acadmemy = db.Academyyears.ToList();
            List<string> labels = new List<string>();
            List<string> label = new List<string>();
            List<List<int?>> datas = new List<List<int?>>();
            var i = 0;
            foreach (var item in acadmemy)
            {
                labels.Add(item.name);
                var result = db.Number_of_claims_within_each_Faculty_for_each_academic_year(item.id).ToList();
                List<int?> data = new List<int?>();
                foreach (var item1 in result)
                {
                    if (i == 0)
                    {
                        label.Add(item1.name);
                    }
                    data.Add(item1.claims);
                }
                datas.Add(data);
                i++;
            }

            return Json(new
            {
                labels = labels,
                label = label,
                datas = datas
            });
        }
    }
}