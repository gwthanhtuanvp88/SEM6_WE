using ClaimReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ClaimReport.Controllers
{
    public class AdministratorController : Controller
    {
        ReportClaimEntities db = new ReportClaimEntities();

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

        public ActionResult Users()
        {
            return View(db.Users.ToList());
        }

        public ActionResult CreateUser()
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User model, int facultyId, string pass)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("CreateUser");
            }

            var flag = false;

            if (String.IsNullOrEmpty(model.username))
            {
                ModelState.AddModelError("username", "User Name is required!");
                flag = true;
            }

            if (String.IsNullOrEmpty(pass))
            {
                ModelState.AddModelError("password", "Password is required!");
                flag = true;
            }

            if (String.IsNullOrEmpty(model.email))
            {
                ModelState.AddModelError("email", "Email is required!");
                flag = true;
            }

            if (db.Users.Where(x => x.username == model.username).ToList().Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This User Name already exists.");
                flag = true;
            }

            if (db.Users.Where(x => x.email == model.email).ToList().Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This email already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("CreateUser");
            }

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass.ToString()));
            }

            model.password = hash;
            model.status = true;
            model.active = false;
            var user = db.Users.Add(model);

            var usertype = db.UserTypes.SingleOrDefault(x => x.id == model.usertypeid);

            if (usertype.name == "Student")
            {
                Student st = new Student();
                st.facultyid = facultyId;
                st.userid = user.id;
                st.status = model.status;
                db.Students.Add(st);
            }

            if (usertype.name == "Coordinator")
            {
                Coordinator co = new Coordinator();
                co.facutyid = facultyId;
                co.userid = user.id;
                co.status = model.status;
                db.Coordinators.Add(co);
            }

            db.SaveChanges();

            return RedirectToAction("Users");
        }

        public ActionResult EditUser(int id)
        {
            var student = db.Students.SingleOrDefault(x => x.userid == id);
            if (student != null)
            {
                ViewBag.FacultySelected = student.facultyid.ToString();
            }

            var coordinator = db.Coordinators.SingleOrDefault(x => x.userid == id);
            if (coordinator != null)
            {
                ViewBag.FacultySelected = coordinator.facutyid.ToString();
            }

            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();
            return View(db.Users.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditUser(User model, int facultyId, string pass)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("EditUser");
            }

            var flag = false;

            if (String.IsNullOrEmpty(model.username))
            {
                ModelState.AddModelError("username", "User Name is required!");
                flag = true;
            }

            if (String.IsNullOrEmpty(pass))
            {
                ModelState.AddModelError("password", "Password is required!");
                flag = true;
            }

            if (String.IsNullOrEmpty(model.email))
            {
                ModelState.AddModelError("email", "Email is required!");
                flag = true;
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
                return View("CreateUser");
            }

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(pass));
            }

            model.password = hash;

            var user = db.Users.SingleOrDefault(x=> x.id == model.id);

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
                Student st = new Student();
                st.facultyid = facultyId;
                st.userid = user.id;
                st.status = model.status;
                db.Students.Add(st);
            }

            if (usertype.name == "Coordinator")
            {
                Coordinator co = new Coordinator();
                co.facutyid = facultyId;
                co.userid = user.id;
                co.status = model.status;
                db.Coordinators.Add(co);
            }

            db.SaveChanges();

            return RedirectToAction("Users");
        }

        public ActionResult DeleteUser(int id)
        {
            var result = db.Users.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.Users.Remove(result);
            }

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

            db.SaveChanges();

            return RedirectToAction("Users");
        }


        // Academyyear
        // ======================================

        public ActionResult Academyyear()
        {
            return View(db.Academyyears.ToList());
        }

        public ActionResult CreateAcademyYear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAcademyYear(Academyyear model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAcademyYear");
            }

            var flag = false;

            // Check null
            if (model.startReportDate == null)
            {
                ModelState.AddModelError("startReportDate", "Start Report Date is required!");
                flag = true;
            }

            if (model.closureReportDate == null)
            {
                ModelState.AddModelError("closureReportDate", "Closure Report Date is required!");
                flag = true;
            }

            if (model.closureEvidenceDate == null)
            {
                ModelState.AddModelError("closureEvidenceDate", "Closure Evidence Date is required!");
                flag = true;
            }


            // Check ordinal
            if (model.startReportDate != null && model.closureReportDate != null && model.closureEvidenceDate != null)
            {
                DateTime startDate = model.startReportDate.Value;
                DateTime closureDate = model.closureReportDate.Value;
                DateTime evidenceDate = model.closureEvidenceDate.Value;
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
            }

            // Check duplicate
            var duplicate = db.Academyyears.Where(x => x.startReportDate == model.startReportDate && x.closureEvidenceDate == model.closureEvidenceDate && x.closureReportDate == model.closureReportDate).ToList();
            if (duplicate != null)
            {
                ModelState.AddModelError(String.Empty, "This Academy year already exists.");
                flag = true;
            }

            if (flag)
            {
                return View("CreateAcademyYear");
            }


            db.Academyyears.Add(model);
            db.SaveChanges();

            return RedirectToAction("Academyyear");
        }

        public ActionResult EditAcademyYear(int id)
        {
            return View(db.Academyyears.SingleOrDefault(x => x.id == id));
        }

        [HttpPost]
        public ActionResult EditAcademyYear(Academyyear model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditAcademyYear");
            }

            var flag = false;

            if (model.startReportDate == null)
            {
                ModelState.AddModelError("startReportDate", "Start Report Date is required!");
                flag = true;
            }

            if (model.closureReportDate == null)
            {
                ModelState.AddModelError("closureReportDate", "Closure Report Date is required!");
                flag = true;
            }

            if (model.closureEvidenceDate == null)
            {
                ModelState.AddModelError("closureEvidenceDate", "Closure Evidence Date is required!");
                flag = true;
            }

            if (model.startReportDate != null && model.closureReportDate != null && model.closureEvidenceDate != null)
            {
                DateTime startDate = model.startReportDate.Value;
                DateTime closureDate = model.closureReportDate.Value;
                DateTime evidenceDate = model.closureEvidenceDate.Value;
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
            }

            if (flag)
            {
                return View("EditAcademyYear");
            }


            var academyyear = db.Academyyears.SingleOrDefault(x => x.id == model.id);
            academyyear.startReportDate = model.startReportDate;
            academyyear.closureReportDate = model.closureReportDate;
            academyyear.closureEvidenceDate = model.closureEvidenceDate;
            academyyear.status = model.status;
            db.SaveChanges();

            return RedirectToAction("Academyyear");
        }

        public ActionResult DeleteAcademyYear(int id)
        {
            var result = db.Academyyears.SingleOrDefault(x => x.id == id);
            if (result != null)
            {
                db.Academyyears.Remove(result);
            }

            db.SaveChanges();

            return RedirectToAction("Academyyear");
        }

    }
}