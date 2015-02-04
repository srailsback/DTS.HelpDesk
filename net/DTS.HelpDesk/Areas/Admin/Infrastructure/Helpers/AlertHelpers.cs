using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace System.Web.Mvc
{

    public static class AlertHelpers
    {


        public static void AlertInfo(this Controller controller, string message)
        {
            controller.TempData["info"] = message;
        }
        public static void AlertSuccess(this Controller controller, string message)
        {
            controller.TempData["success"] = message;
        }
        public static void AlertWarning(this Controller controller, string message)
        {
            controller.TempData["warning"] = message;
        }
        public static void AlertError(this Controller controller, string message)
        {
            controller.TempData["error"] = message;
        }

        public static void AlertInfoAppend(this Controller controller, string message)
        {
            if (String.IsNullOrEmpty((controller != null && controller.TempData["info"] != null) ? controller.TempData["info"].ToString() : ""))
                controller.TempData["info"] = message;
            else
                controller.TempData["info"] += "<br />" + message;
        }
        public static void AlertSuccessAppend(this Controller controller, string message)
        {
            if (String.IsNullOrEmpty((controller != null && controller.TempData["success"] != null) ? controller.TempData["success"].ToString() : ""))
                controller.TempData["success"] = message;
            else
                controller.TempData["success"] += "<br />" + message;
        }
        public static void AlertWarningAppend(this Controller controller, string message)
        {
            if (String.IsNullOrEmpty((controller != null && controller.TempData["warning"] != null) ? controller.TempData["warning"].ToString() : ""))
                controller.TempData["warning"] = message;
            else
                controller.TempData["warning"] += "<br />" + message;
        }
        public static void AlertErrorAppend(this Controller controller, string message)
        {
            if (String.IsNullOrEmpty((controller != null && controller.TempData["error"] != null) ? controller.TempData["error"].ToString() : ""))
                controller.TempData["error"] = message;
            else
                controller.TempData["error"] += "<br />" + message;
        }


        public static MvcHtmlString Alert(this HtmlHelper helper)
        {
            var message = "";
            var className = "";

            if (helper.ViewContext.Controller.TempData["info"] != null)
            {
                message = helper.ViewContext.Controller.TempData["info"].ToString();
                className = "alert-info";
            }
            else if (helper.ViewContext.Controller.TempData["success"] != null)
            {
                message = helper.ViewContext.Controller.TempData["success"].ToString();
                className = "alert-success";
            }
            else if (helper.ViewContext.Controller.TempData["warning"] != null)
            {
                message = helper.ViewContext.Controller.TempData["warning"].ToString();
                className = "alert-warning";
            }
            else if (helper.ViewContext.Controller.TempData["error"] != null)
            {
                message = helper.ViewContext.Controller.TempData["error"].ToString();
                className = "alert-danger";
            }

            /**** Template for Bootstrap v3.1.1
             *
                <div class="alert alert-warning alert-dismissable fade in">
                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                    *text*
                </div>             
            */
            var display = (className == "" || message == "") ? "hide" : className;

            var button = new TagBuilder("button");
            button.Attributes.Add("type", "button");
            button.AddCssClass("close");
            button.Attributes.Add("data-dismiss", "alert");
            button.Attributes.Add("aria-hidden", "true");
            button.InnerHtml = "&times;";

            var alertMessage = new TagBuilder("span");
            alertMessage.Attributes.Add("id", "alert-message");
            alertMessage.InnerHtml = message;

            var alertContainer = new TagBuilder("div");
            alertContainer.Attributes.Add("id", "alert-content");
            alertContainer.AddCssClass(string.Format("alert alert-dismissable fade in {0}", display)); //fade in
            alertContainer.InnerHtml = button.ToString() + alertMessage.ToString();

            return MvcHtmlString.Create(alertContainer.ToString());
        }
    }
}