using ClaimReport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClaimReport.Controllers
{
    public class AdminPermissionController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = (User)Session["user"];
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            } else
            {
                if (user.UserType.name != "Admin")
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}