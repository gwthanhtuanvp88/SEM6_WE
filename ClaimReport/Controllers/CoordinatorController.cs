using ClaimReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ClaimReport.Controllers
{
    public class CoordinatorController : Controller
    {
        private ReportClaimEntities db = new ReportClaimEntities();

        // GET: Coordinator
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var user = (User)Session["user"];
            var coordinator = db.Coordinators.FirstOrDefault(x => x.userid == user.id);
            return View(db.Claims.Where(x => x.coordinatorId == coordinator.id).ToList());
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

            return View(claim);
        }
    }
}