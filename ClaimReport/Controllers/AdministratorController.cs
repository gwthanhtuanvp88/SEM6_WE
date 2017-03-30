using ClaimReport.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ClaimReport.Controllers
{
    public class AdministratorController : AdminPermissionController
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

        public ActionResult Users(int? page)
        {
            if (page == null) { page = 1; }
            var lstUser = db.Users.Where(x => x.status == true).OrderByDescending(x => x.id);
            IPagedList<User> Users = lstUser.ToPagedList((int)page, 5);
            return View(Users);
        }

        public ActionResult CreateUser()
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(UserModel model)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("CreateUser");
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
        public ActionResult EditUser(UserModel model)
        {
            ViewBag.Faculty = db.Faculties.ToList();
            ViewBag.UserType = db.UserTypes.ToList();

            if (!ModelState.IsValid)
            {
                return View("CreateUser");
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
                return View("CreateUser");
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

        public ActionResult Academyyear(int? page)
        {
            if (page == null) { page = 1; }
            var lstAcademyYear = db.Academyyears.Where(x => x.status == true).OrderByDescending(x => x.id);
            IPagedList<Academyyear> Academyyear = lstAcademyYear.ToPagedList((int)page, 5);
            return View(Academyyear);
        }

        public ActionResult CreateAcademyYear()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAcademyYear(AcademyYearModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateAcademyYear");
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
                return View("CreateAcademyYear");
            }

            Academyyear ay = new Academyyear();
            ay.name = model.Name;
            ay.startReportDate = model.startReportDate;
            ay.closureReportDate = model.closureReportDate;
            ay.closureEvidenceDate = model.closureEvidenceDate;
            ay.status = true;

            db.Academyyears.Add(ay);
            db.SaveChanges();

            return RedirectToAction("Academyyear");
        }

        public ActionResult EditAcademyYear(int id)
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
        public ActionResult EditAcademyYear(AcademyYearModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("EditAcademyYear");
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
                return View("EditAcademyYear");
            }


            var academyyear = db.Academyyears.SingleOrDefault(x => x.id == model.id);
            academyyear.name = model.Name;
            academyyear.startReportDate = model.startReportDate;
            academyyear.closureReportDate = model.closureReportDate;
            academyyear.closureEvidenceDate = model.closureEvidenceDate;
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