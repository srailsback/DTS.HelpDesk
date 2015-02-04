using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        public static string SetActiveMenuItem(this HtmlHelper helper, string controllerName, string actionName)
        {
            var routeData = helper.ViewContext.RouteData;
            var currentController = routeData.GetRequiredString("controller");
            var currentAction = routeData.GetRequiredString("action");
            if (currentAction == actionName && currentController == controllerName)
            {
                return "active";
            }
            return "";
        }
    }
}