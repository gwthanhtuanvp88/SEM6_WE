using ClaimReport.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClaimReport.Controllers
{
    public class AdministratorController : Controller
    {
        ReportClaimEntities db = new ReportClaimEntities();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = (User)Session["user"];
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                if (user.UserType.name != "Administrator")
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "NoPermission" }));
                }
            }

            base.OnActionExecuting(filterContext);
        }

        // GET: Administrator
        public ActionResult Index()
        {
            return View();
        }

        // Role
        // ======================================

        public ActionResult Role()
        {
            return View(db.Roles.ToList());
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateRole");
            }

            if (String.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "Description is required!");
                return View("CreateRole");
            }

            var result = db.Roles.Where(x => x.description == model.description).ToList().Count;
            if (result > 0)
            {
                ModelState.AddModelError(string.Empty, "This role already exists.");
                return View("CreateRole");
            }

            db.Roles.Add(model);
            db.SaveChanges();

            return RedirectToAction("Role");
        }

        public ActionResult EditRole(int id)
        {
            return View(db.Roles.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditRole(Role model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditRole");
            }

            if (String.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "Description is required!");
                return View("EidtRole");
            }

            if (db.Roles.Where(x => x.description == model.description).ToList().Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This role already exists.");
                return View("EditRole");
            }

            var result = db.Roles.SingleOrDefault(x => x.id == model.id);
            if (result != null)
            {
                result.description = model.description;
                result.status = model.status;
                db.SaveChanges();
            }

            return RedirectToAction("Role");
        }

        public ActionResult DeleteRole(int id)
        {
            var result = db.Roles.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.Roles.Remove(result);
                db.SaveChanges();
            }

            return RedirectToAction("Role");
        }

        // User Type
        // ======================================

        public ActionResult UserType()
        {
            return View(db.UserTypes.ToList());
        }

        public ActionResult CreateUserType()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateUserType(UserType model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateUserType");
            }

            if (String.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "Name is required!");
                return View("CreateUserType");
            }

            var result = db.UserTypes.Where(x => x.name == model.name).ToList().Count;
            if (result > 0)
            {
                ModelState.AddModelError(string.Empty, "This User type already exists.");
                return View("CreateUserType");
            }

            db.UserTypes.Add(model);
            db.SaveChanges();

            return RedirectToAction("UserType");
        }

        public ActionResult EditUserType(int id)
        {
            return View(db.UserTypes.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditUserType(UserType model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditUserType");
            }

            if (String.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "Name is required!");
                return View("EidtUserType");
            }

            var duplicate = db.UserTypes.SingleOrDefault(x => x.name == model.name);
            if (duplicate != null && duplicate.id != model.id)
            {
                ModelState.AddModelError(string.Empty, "This User type already exists.");
                return View("EditUserType");
            }

            var result = db.UserTypes.SingleOrDefault(x => x.id == model.id);
            if (result != null)
            {
                result.name = model.name;
                result.description = model.description;
                result.status = model.status;
                db.SaveChanges();
            }

            return RedirectToAction("UserType");
        }

        public ActionResult DeleteUserType(int id)
        {
            var result = db.UserTypes.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.UserTypes.Remove(result);
                db.SaveChanges();
            }

            return RedirectToAction("UserType");
        }

        // User Role
        // ======================================

        public ActionResult UserRole()
        {
            return View(db.UserRoles.ToList());
        }

        public ActionResult CreateUserRole()
        {
            ViewBag.RolesID = db.Roles.ToList();
            ViewBag.UserTypeID = db.UserTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateUserRole(UserRole model)
        {
            ViewBag.RolesID = db.Roles.ToList();
            ViewBag.UserTypeID = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("CreateUserRole");
            }

            if (String.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "Description is required!");
                return View("CreateUserRole");
            }

            var result = db.UserRoles.Where(x => x.description == model.description).ToList().Count;
            if (result > 0)
            {
                ModelState.AddModelError(string.Empty, "This User role already exists.");
                return View("CreateUserRole");
            }

            db.UserRoles.Add(model);
            db.SaveChanges();

            return RedirectToAction("UserRole");
        }

        public ActionResult EditUserRole(int id)
        {
            ViewBag.RolesID = db.Roles.ToList();
            ViewBag.UserTypeID = db.UserTypes.ToList();
            return View(db.UserRoles.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditUserRole(UserRole model)
        {
            ViewBag.RolesID = db.Roles.ToList();
            ViewBag.UserTypeID = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("EditUserRole");
            }

            if (String.IsNullOrEmpty(model.description))
            {
                ModelState.AddModelError("description", "Description is required!");
                return View("EidtUserRole");
            }

            var duplicate = db.UserRoles.SingleOrDefault(x => x.description == model.description);
            if (duplicate != null && duplicate.id != model.id)
            {
                ModelState.AddModelError(string.Empty, "This User role already exists.");
                return View("EditUserRole");
            }

            var result = db.UserRoles.SingleOrDefault(x => x.id == model.id);
            if (result != null)
            {
                result.description = model.description;
                result.userTypeid = model.userTypeid;
                result.roleid = model.roleid;
                result.status = model.status;
                db.SaveChanges();
            }

            return RedirectToAction("UserRole");
        }

        public ActionResult DeleteUserRole(int id)
        {
            var result = db.UserRoles.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.UserRoles.Remove(result);
                db.SaveChanges();
            }

            return RedirectToAction("UserRole");
        }


        // Faculty
        // ======================================

        public ActionResult Faculty()
        {
            return View(db.Faculties.ToList());
        }

        public ActionResult CreateFaculty()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateFaculty(Faculty model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateFaculty");
            }

            if (String.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "Name is required!");
                return View("CreateFaculty");
            }

            var result = db.Faculties.Where(x => x.name == model.name).ToList().Count;
            if (result > 0)
            {
                ModelState.AddModelError(string.Empty, "This Faculty already exists.");
                return View("CreateFaculty");
            }

            db.Faculties.Add(model);
            db.SaveChanges();

            return RedirectToAction("Faculty");
        }

        public ActionResult EditFaculty(int id)
        {
            return View(db.Faculties.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditFaculty(Faculty model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditFaculty");
            }

            if (String.IsNullOrEmpty(model.name))
            {
                ModelState.AddModelError("name", "Name is required!");
                return View("EidtFaculty");
            }

            var duplicate = db.Faculties.SingleOrDefault(x => x.name == model.name);
            if (duplicate != null && duplicate.id != model.id)
            {
                ModelState.AddModelError(string.Empty, "This Faculty already exists.");
                return View("EditFaculty");
            }

            var result = db.Faculties.SingleOrDefault(x => x.id == model.id);
            if (result != null)
            {
                result.name = model.name;
                result.description = model.description;
                result.status = model.status;
                db.SaveChanges();
            }

            return RedirectToAction("Faculty");
        }

        public ActionResult DeleteFaculty(int id)
        {
            var result = db.Faculties.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.Faculties.Remove(result);
                db.SaveChanges();
            }

            return RedirectToAction("Faculty");
        }

        // User
        // ======================================

        public ActionResult Users(int? page, string txtSearch)
        {
            if (page == null) { page = 1; }
            IQueryable<User> lstUser = null;
            if (String.IsNullOrEmpty(txtSearch))
            {
                lstUser = db.Users.Where(x => x.status == true).OrderByDescending(x => x.id);
            }
            else
            {
                lstUser = db.Users.Where(x => x.status == true && x.name.Contains(txtSearch)).OrderByDescending(x => x.id);
                ViewBag.ViewAll = true;
            }

            if (TempData["errorMessage"] != null)
            {
                ViewBag.Error = TempData["errorMessage"].ToString();
            }

            if (TempData["successMessage"] != null)
            {
                ViewBag.Success = TempData["successMessage"].ToString();
            }

            IPagedList<User> Users = lstUser.ToPagedList((int)page, 5);
            return View(Users);
        }

        public ActionResult UserCreate()
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult UserCreate(UserModel model)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("UserCreate");
            }

            var flag = false;

            var userType = db.UserTypes.SingleOrDefault(x => x.id == model.usertypeid);

            if (userType.name == "Student" || userType.name == "Coordinator")
            {
                if (String.IsNullOrEmpty(model.facultyId))
                {
                    ModelState.AddModelError("facultyId", "You must select faculty when user is Student or Coordinator.");
                    flag = true;
                }
            }


            if (db.Users.Where(x => x.username == model.username).ToList().Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This User Name already exists.");
                flag = true;
            }

            if (db.Users.Where(x => x.email == model.email).ToList().Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This Email already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("UserCreate");
            }

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.password.ToString()));
            }

            User user = new User();
            user.username = model.username;
            user.password = hash;
            user.name = model.name;
            user.email = model.email;
            user.phone = model.phone;
            user.address = model.address;
            user.usertypeid = model.usertypeid;
            user.status = true;
            user.active = false;

            var addResult = db.Users.Add(user);

            var usertype = db.UserTypes.SingleOrDefault(x => x.id == model.usertypeid);

            if (usertype.name == "Student")
            {
                Student st = new Student();
                st.facultyid = Convert.ToInt32(model.facultyId);
                st.userid = user.id;
                st.status = true;
                db.Students.Add(st);
            }

            if (usertype.name == "Coordinator")
            {
                Coordinator co = new Coordinator();
                co.facutyid = Convert.ToInt32(model.facultyId);
                co.userid = user.id;
                co.status = true;
                db.Coordinators.Add(co);
            }

            try { db.SaveChanges(); }
            catch (Exception e)
            {
                TempData["errorMessage"] = "Create a new item error.";
                return RedirectToAction("Academyyear");
            }

            TempData["successMessage"] = "Create a new item successful.";
            return RedirectToAction("Users");
        }

        public ActionResult UserEdit(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.userid == id);
            if (student != null)
            {
                ViewBag.FacultySelected = student.facultyid.ToString();
            }

            var coordinator = db.Coordinators.SingleOrDefault(x => x.userid == id);
            if (coordinator != null)
            {
                ViewBag.CoordinatorSelected = coordinator.facutyid.ToString();
            }

            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            User user = db.Users.SingleOrDefault(x => x.id == id);
            UserModel um = new UserModel();
            um.id = user.id;
            um.username = user.username;
            um.name = user.name;
            um.email = user.email;
            um.phone = user.phone;
            um.address = user.address;
            um.usertypeid = user.usertypeid;

            return View(um);
        }

        [HttpPost]
        public ActionResult UserEdit(UserModel model)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("UserEdit");
            }

            var flag = false;

            var userType = db.UserTypes.SingleOrDefault(x => x.id == model.usertypeid);
            if (userType.name == "Student" || userType.name == "Coordinator")
            {
                if (String.IsNullOrEmpty(model.facultyId))
                {
                    ModelState.AddModelError("facultyId", "You must select faculty when user is Student or Coordinator.");
                    flag = true;
                }
            }

            var userNameDuplicate = db.Users.SingleOrDefault(x => x.username == model.username);
            if (userNameDuplicate != null && userNameDuplicate.id != model.id)
            {
                ModelState.AddModelError(string.Empty, "This User Name already exists.");
                flag = true;
            }

            var emailDuplicate = db.Users.SingleOrDefault(x => x.email == model.email);
            if (emailDuplicate != null && emailDuplicate.id != model.id)
            {
                ModelState.AddModelError(string.Empty, "This email already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("UserEdit");
            }

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(model.password.ToString()));
            }

            var user = db.Users.SingleOrDefault(x => x.id == model.id);

            user.username = model.username;
            user.name = model.name;
            user.password = hash;
            user.email = model.email;
            user.phone = model.phone;
            user.address = model.address;
            user.usertypeid = model.usertypeid;

            var usertype = db.UserTypes.SingleOrDefault(x => x.id == model.usertypeid);

            if (usertype.name == "Student")
            {
                var student = db.Students.SingleOrDefault(x => x.userid == model.id);
                student.facultyid = Convert.ToInt32(model.facultyId);
            }

            if (usertype.name == "Coordinator")
            {
                var coordinator = db.Coordinators.SingleOrDefault(x => x.userid == model.id);
                coordinator.facutyid = Convert.ToInt32(model.facultyId);
            }

            try { db.SaveChanges(); }
            catch (Exception e)
            {
                TempData["errorMessage"] = "Edit the item error.";
                return RedirectToAction("Academyyear");
            }

            TempData["successMessage"] = "Edit the item successful.";
            return RedirectToAction("Users");
        }

        public ActionResult UserDelete(int id)
        {
            var user = db.Users.SingleOrDefault(x => x.id == id);
            if (user != null)
            {
                db.Users.Remove(user);


                var student = db.Students.SingleOrDefault(x => x.userid == id);
                if (student != null)
                {
                    db.Students.Remove(student);
                }

                var coordinator = db.Coordinators.SingleOrDefault(x => x.userid == id);
                if (coordinator != null)
                {
                    db.Coordinators.Remove(coordinator);
                }

                try { db.SaveChanges(); }
                catch (Exception e)
                {
                    TempData["errorMessage"] = "Delete the item error.";
                    return RedirectToAction("Academyyear");
                }
            }
            else
            {
                TempData["errorMessage"] = "This item might not exist or is no longer available.";
                return RedirectToAction("Users");
            }

            TempData["successMessage"] = "Delete Successful.";
            return RedirectToAction("Users");
        }


        // Academyyear
        // ======================================

        public ActionResult Academyyear(int? page, string txtSearch)
        {
            if (page == null) { page = 1; }
            IQueryable<Academyyear> lstAcademyYear = null;
            if (String.IsNullOrEmpty(txtSearch))
            {
                lstAcademyYear = db.Academyyears.Where(x => x.status == true).OrderByDescending(x => x.id);
            }
            else
            {
                lstAcademyYear = db.Academyyears.Where(x => x.status == true && x.name.Contains(txtSearch)).OrderByDescending(x => x.id);
                ViewBag.ViewAll = true;
            }

            if (TempData["errorMessage"] != null)
            {
                ViewBag.Error = TempData["errorMessage"].ToString();
            }

            if (TempData["successMessage"] != null)
            {
                ViewBag.Success = TempData["successMessage"].ToString();
            }

            IPagedList<Academyyear> Academyyear = lstAcademyYear.ToPagedList((int)page, 5);
            return View(Academyyear);
        }

        public ActionResult AcademyYearCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AcademyYearCreate(AcademyYearModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AcademyYearCreate");
            }

            var flag = false;

            // Check ordinal
            DateTime startDate = model.startReportDate;
            DateTime closureDate = model.closureReportDate;
            DateTime evidenceDate = model.closureEvidenceDate;
            int startClosureDateResult = DateTime.Compare(startDate, closureDate);
            int startEvidenceDateResult = DateTime.Compare(startDate, evidenceDate);
            int closureEvidenceDateResult = DateTime.Compare(closureDate, evidenceDate);

            if (startClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure report date!");
                flag = true;
            }

            if (startEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            if (closureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            // Check duplicate
            var duplicate = db.Academyyears.Where(x => x.name == model.Name && x.startReportDate == model.startReportDate && x.closureEvidenceDate == model.closureEvidenceDate && x.closureReportDate == model.closureReportDate).ToList();
            if (duplicate.Count > 0)
            {
                ModelState.AddModelError(String.Empty, "This Academy year already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("AcademyYearCreate");
            }

            Academyyear ay = new Academyyear();
            ay.name = model.Name;
            ay.startReportDate = model.startReportDate;
            ay.closureReportDate = model.closureReportDate;
            ay.closureEvidenceDate = model.closureEvidenceDate;
            ay.status = true;

            db.Academyyears.Add(ay);
            db.SaveChanges();

            TempData["successMessage"] = "Create new item successful.";
            return RedirectToAction("Academyyear");
        }

        public ActionResult AcademyYearEdit(int id)
        {
            AcademyYearModel aym = new AcademyYearModel();
            Academyyear ay = db.Academyyears.SingleOrDefault(x => x.id == id);
            aym.id = ay.id;
            aym.Name = ay.name;
            aym.startReportDate = Convert.ToDateTime(ay.startReportDate);
            aym.closureReportDate = Convert.ToDateTime(ay.closureReportDate);
            aym.closureEvidenceDate = Convert.ToDateTime(ay.closureEvidenceDate);

            return View(aym);
        }

        [HttpPost]
        public ActionResult AcademyYearEdit(AcademyYearModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AcademyYearEdit");
            }

            var flag = false;


            DateTime startDate = model.startReportDate;
            DateTime closureDate = model.closureReportDate;
            DateTime evidenceDate = model.closureEvidenceDate;
            int startClosureDateResult = DateTime.Compare(startDate, closureDate);
            int startEvidenceDateResult = DateTime.Compare(startDate, evidenceDate);
            int closureEvidenceDateResult = DateTime.Compare(closureDate, evidenceDate);

            if (startClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure report date!");
                flag = true;
            }

            if (startEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            if (closureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            // Check duplicate
            Academyyear duplicate = db.Academyyears.SingleOrDefault(x => x.name == model.Name && x.startReportDate == model.startReportDate && x.closureEvidenceDate == model.closureEvidenceDate && x.closureReportDate == model.closureReportDate);
            if (duplicate != null && duplicate.id != model.id)
            {
                ModelState.AddModelError(String.Empty, "This Academy year already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("AcademyYearEdit");
            }


            var academyyear = db.Academyyears.SingleOrDefault(x => x.id == model.id);
            academyyear.name = model.Name;
            academyyear.startReportDate = model.startReportDate;
            academyyear.closureReportDate = model.closureReportDate;
            academyyear.closureEvidenceDate = model.closureEvidenceDate;
            db.SaveChanges();

            TempData["successMessage"] = "Edit item successful.";
            return RedirectToAction("Academyyear");
        }

        public ActionResult AcademyYearDelete(int id)
        {
            var item = db.Academyyears.SingleOrDefault(x => x.id == id);
            if (item != null)
            {
                var result = db.Academyyears.Remove(item);

                try { db.SaveChanges(); }
                catch (Exception e)
                {
                    TempData["errorMessage"] = "You can not delte this item because it is being used in Assessment.";
                    return RedirectToAction("Academyyear");
                }

            }
            else
            {
                TempData["errorMessage"] = "This item might not exist or is no longer available.";
                return RedirectToAction("Academyyear");
            }

            TempData["successMessage"] = "Delete Successful.";
            return RedirectToAction("Academyyear");
        }

        // Assessment
        // ======================================

        public ActionResult Assessment(int? page, string txtSearch)
        {
            if (page == null) { page = 1; }
            IQueryable<Assessment> lstAssessment = null;
            if (String.IsNullOrEmpty(txtSearch))
            {
                lstAssessment = db.Assessments.Where(x => x.status == true).OrderByDescending(x => x.id);
            }
            else
            {
                lstAssessment = db.Assessments.Where(x => x.status == true && x.name.Contains(txtSearch)).OrderByDescending(x => x.id);
                ViewBag.ViewAll = true;
            }

            if (TempData["errorMessage"] != null)
            {
                ViewBag.Error = TempData["errorMessage"].ToString();
            }

            if (TempData["successMessage"] != null)
            {
                ViewBag.Success = TempData["successMessage"].ToString();
            }

            IPagedList<Assessment> item = lstAssessment.ToPagedList((int)page, 5);
            return View(item);
        }

        public ActionResult AssessmentCreate()
        {
            ViewBag.AcademyYear = db.Academyyears.ToList();
            ViewBag.Faculty = db.Faculties.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AssessmentCreate(AssessmentModel model)
        {
            ViewBag.AcademyYear = db.Academyyears.ToList();

            if (!ModelState.IsValid)
            {
                return View("AssessmentCreate");
            }

            var flag = false;

            // Check duplicate
            var duplicate = db.Assessments.Where(x => x.name == model.Name && x.academyyearId == model.academyeYearId && x.facultyid == model.facultyid).ToList();
            if (duplicate.Count > 0)
            {
                ModelState.AddModelError(String.Empty, "This item already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("AssessmentCreate");
            }

            Assessment assessment = new Assessment();
            assessment.name = model.Name;
            assessment.description = model.Description;
            assessment.academyyearId = model.academyeYearId;
            assessment.facultyid = model.facultyid;
            assessment.status = true;

            db.Assessments.Add(assessment);
            db.SaveChanges();

            TempData["successMessage"] = "Create new item successful.";
            return RedirectToAction("Assessment");
        }

        public ActionResult AssessmentEdit(int id)
        {
            AssessmentModel AssessmentModel = new AssessmentModel();
            Assessment assessment = db.Assessments.SingleOrDefault(x => x.id == id);
            AssessmentModel.ID = assessment.id;
            AssessmentModel.Name = assessment.name;
            AssessmentModel.Description = assessment.description;
            AssessmentModel.academyeYearId = assessment.academyyearId;
            AssessmentModel.facultyid = assessment.facultyid;

            ViewBag.AcademyYear = db.Academyyears.ToList();
            ViewBag.AcademyYearSelected = assessment.academyyearId;
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.FacultySelected = assessment.facultyid;
            return View(AssessmentModel);
        }

        [HttpPost]
        public ActionResult AssessmentEdit(AssessmentModel model)
        {
            ViewBag.AcademyYear = db.Academyyears.ToList();
            ViewBag.AcademyYearSelected = model.academyeYearId;
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.FacultySelected = model.facultyid;

            if (!ModelState.IsValid)
            {
                return View("AssessmentEdit");
            }

            var flag = false;

            // Check duplicate
            Assessment duplicate = db.Assessments.SingleOrDefault(x => x.name == model.Name && x.academyyearId == model.academyeYearId && x.facultyid == model.facultyid);
            if (duplicate != null && duplicate.id != model.ID)
            {
                ModelState.AddModelError(String.Empty, "This Academy year already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("AssessmentEdit");
            }


            var assessment = db.Assessments.SingleOrDefault(x => x.id == model.ID);
            assessment.name = model.Name;
            assessment.description = model.Description;
            assessment.academyyearId = model.academyeYearId;
            assessment.facultyid = model.facultyid;
            db.SaveChanges();

            TempData["successMessage"] = "Edit successful.";
            return RedirectToAction("Assessment");
        }

        public ActionResult AssessmentDelete(int id)
        {
            var assessment = db.Assessments.SingleOrDefault(x => x.id == id);
            if (assessment != null)
            {
                var result = db.Assessments.Remove(assessment);

                try { db.SaveChanges(); }
                catch (Exception e)
                {
                    TempData["errorMessage"] = "Delete error.";
                    return RedirectToAction("Assessment");
                }

            }
            else
            {
                TempData["errorMessage"] = "This item might not exist or is no longer available.";
                return RedirectToAction("Assessment");
            }

            TempData["successMessage"] = "Delete Successful.";
            return RedirectToAction("Assessment");
        }

        // Item
        // ======================================

        public ActionResult Item(int? page, string txtSearch)
        {
            if (page == null) { page = 1; }
            IQueryable<Item> lstItem = null;
            if (String.IsNullOrEmpty(txtSearch))
            {
                lstItem = db.Items.Where(x => x.status == true).OrderByDescending(x => x.id);
            }
            else
            {
                lstItem = db.Items.Where(x => x.status == true && x.name.Contains(txtSearch)).OrderByDescending(x => x.id);
                ViewBag.ViewAll = true;
            }

            if (TempData["errorMessage"] != null)
            {
                ViewBag.Error = TempData["errorMessage"].ToString();
            }

            if (TempData["successMessage"] != null)
            {
                ViewBag.Success = TempData["successMessage"].ToString();
            }

            IPagedList<Item> item= lstItem.ToPagedList((int)page, 5);
            return View(item);
        }

        public ActionResult ItemCreate()
        {
            ViewBag.Assessments = db.Assessments.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult ItemCreate(ItemModel model)
        {
            ViewBag.Assessments = db.Assessments.ToList();

            if (!ModelState.IsValid)
            {
                return View("ItemCreate");
            }

            var flag = false;

            // Check ordinal
            DateTime startDate = model.startReportDate;
            DateTime closureDate = model.closureReportDate;
            DateTime evidenceDate = model.closureEvidenceDate;

            int startClosureDateResult = DateTime.Compare(startDate, closureDate);
            int startEvidenceDateResult = DateTime.Compare(startDate, evidenceDate);
            int closureEvidenceDateResult = DateTime.Compare(closureDate, evidenceDate);

            if (startClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure report date!");
                flag = true;
            }

            if (startEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            if (closureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            // check item date and academy year date
            Assessment assessment = db.Assessments.SingleOrDefault(x => x.id == model.assessmentID);
            Academyyear academyYear = db.Academyyears.SingleOrDefault(x => x.id == assessment.academyyearId);

            DateTime academyYearStartReportDate = @Convert.ToDateTime(academyYear.startReportDate);
            DateTime academyYearClosureReportDate = @Convert.ToDateTime(academyYear.closureReportDate);
            DateTime academyYearClosureEvidencetDate = @Convert.ToDateTime(academyYear.closureEvidenceDate);

            int startStartDateResult = DateTime.Compare(startDate, academyYearStartReportDate);
            int ClosureClosureDateResult = DateTime.Compare(closureDate, academyYearClosureReportDate);
            int ClosureClosureEvidenceDateResult = DateTime.Compare(evidenceDate, academyYearClosureEvidencetDate);

            if (startStartDateResult < 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must later than Start report date of academy Year!");
                flag = true;
            }

            if (ClosureClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earler than Closure report date of academy Year!");
                flag = true;
            }

            if (ClosureClosureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure Evidence Date must earler than Closure Evidence date of academy Year!");
                flag = true;
            }

            // Check duplicate
            var duplicate = db.Items.Where(x => x.name == model.Name && x.assessmentId == model.assessmentID).ToList();
            if (duplicate.Count > 0)
            {
                ModelState.AddModelError(String.Empty, "This item already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("ItemCreate");
            }

            Item item = new Item();
            item.name = model.Name;
            item.description = model.Description;
            item.startReportDate = model.startReportDate;
            item.closureReportDate = model.closureReportDate;
            item.closureEvidenceDate = model.closureEvidenceDate;
            item.assessmentId = model.assessmentID;
            item.status = true;

            db.Items.Add(item);
            db.SaveChanges();

            TempData["successMessage"] = "Create new item successful.";
            return RedirectToAction("Item");
        }

        public ActionResult ItemEdit(int id)
        {
            ItemModel itemModel = new ItemModel();
            Item item = db.Items.SingleOrDefault(x => x.id == id);
            itemModel.ID = item.id;
            itemModel.Name = item.name;
            itemModel.Description = item.description;
            itemModel.startReportDate = Convert.ToDateTime(item.startReportDate);
            itemModel.closureReportDate = Convert.ToDateTime(item.closureReportDate);
            itemModel.closureEvidenceDate = Convert.ToDateTime(item.closureEvidenceDate);
            itemModel.assessmentID = item.assessmentId;

            ViewBag.Assessments = db.Assessments.ToList();
            ViewBag.Assessmentselected = item.assessmentId;

            return View(itemModel);
        }

        [HttpPost]
        public ActionResult ItemEdit(ItemModel model)
        {
            ViewBag.Assessments = db.Assessments.ToList();
            ViewBag.Assessmentselected = model.assessmentID;

            if (!ModelState.IsValid)
            {
                return View("ItemEdit");
            }

            var flag = false;

            DateTime startDate = model.startReportDate;
            DateTime closureDate = model.closureReportDate;
            DateTime evidenceDate = model.closureEvidenceDate;
            int startClosureDateResult = DateTime.Compare(startDate, closureDate);
            int startEvidenceDateResult = DateTime.Compare(startDate, evidenceDate);
            int closureEvidenceDateResult = DateTime.Compare(closureDate, evidenceDate);

            if (startClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure report date!");
                flag = true;
            }

            if (startEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            if (closureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earlier than Closure Evidence date!");
                flag = true;
            }

            // check item date and academy year date
            Assessment assessment = db.Assessments.SingleOrDefault(x => x.id == model.assessmentID);
            Academyyear academyYear = db.Academyyears.SingleOrDefault(x => x.id == assessment.academyyearId);

            DateTime academyYearStartReportDate = @Convert.ToDateTime(academyYear.startReportDate);
            DateTime academyYearClosureReportDate = @Convert.ToDateTime(academyYear.closureReportDate);
            DateTime academyYearClosureEvidencetDate = @Convert.ToDateTime(academyYear.closureEvidenceDate);

            int startStartDateResult = DateTime.Compare(startDate, academyYearStartReportDate);
            int ClosureClosureDateResult = DateTime.Compare(closureDate, academyYearClosureReportDate);
            int ClosureClosureEvidenceDateResult = DateTime.Compare(evidenceDate, academyYearClosureEvidencetDate);

            if (startStartDateResult < 0)
            {
                ModelState.AddModelError(String.Empty, "Start report Date must later than Start report date of academy Year!");
                flag = true;
            }

            if (ClosureClosureDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure report Date must earler than Closure report date of academy Year!");
                flag = true;
            }

            if (ClosureClosureEvidenceDateResult > 0)
            {
                ModelState.AddModelError(String.Empty, "Closure Evidence Date must earler than Closure Evidence date of academy Year!");
                flag = true;
            }

            // Check duplicate
            Item duplicate = db.Items.SingleOrDefault(x => x.name == model.Name && x.assessmentId == model.assessmentID);
            if (duplicate != null && duplicate.id != model.ID)
            {
                ModelState.AddModelError(String.Empty, "This Academy year already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("ItemEdit");
            }


            var item = db.Items.SingleOrDefault(x => x.id == model.ID);
            item.name = model.Name;
            item.description = model.Description;
            item.startReportDate = model.startReportDate;
            item.closureReportDate = model.closureReportDate;
            item.closureEvidenceDate = model.closureEvidenceDate;
            item.assessmentId = model.assessmentID;
            db.SaveChanges();

            TempData["successMessage"] = "Edit item successful.";
            return RedirectToAction("Item");
        }

        public ActionResult ItemDelete(int id)
        {
            var item = db.Items.SingleOrDefault(x => x.id == id);
            if (item != null)
            {
                var result = db.Items.Remove(item);

                try { db.SaveChanges(); }
                catch (Exception e)
                {
                    TempData["errorMessage"] = "Delete error.";
                    return RedirectToAction("Item");
                }

            }
            else
            {
                TempData["errorMessage"] = "This item might not exist or is no longer available.";
                return RedirectToAction("Item");
            }

            TempData["successMessage"] = "Delete Successful.";
            return RedirectToAction("Item");
        }

        

    }
}