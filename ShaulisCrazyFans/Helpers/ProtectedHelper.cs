using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShaulisCrazyFans.Helpers
{
    public class AdminMembershipAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //redirect if not authenticated
            if (filterContext.HttpContext.Session["Admin-Authentication"] == null ||
                filterContext.HttpContext.Session["Admin-Authentication"] != "true")
            {
                //use the current url for the redirect
                string redirectOnSuccess = filterContext.HttpContext.Request.Url.AbsolutePath;

                //send them off to the login page
                string loginUrl = "/PostManager/Login";
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }
}