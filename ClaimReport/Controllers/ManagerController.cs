using ClaimReport.Models;
using PagedList;
using System;
using System.Collections.Generic;
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
        public ActionResult Index(int? page)
        {
            if (page == null) { page = 1; }
            var lstClaim = db.Claims.Where(c => c.status == true).OrderByDescending(c => c.datesubmited);
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
    }
}