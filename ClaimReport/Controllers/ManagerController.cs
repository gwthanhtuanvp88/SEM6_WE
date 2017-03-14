using ClaimReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClaimReport.Controllers
{
    public class ManagerController : Controller
    {
        private ReportClaimEntities db = new ReportClaimEntities();

        // GET: Manager
        public ActionResult Index()
        {
            return View(db.Claims.ToList());
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

            ViewBag.evidence = db.Evidences.Where(x => x.claimid == id).ToList();
            //ViewBag.claimResult = db.ClaimReults.Where(x => x.claimid == id).ToList();

            return View(claim);
        }
    }
}