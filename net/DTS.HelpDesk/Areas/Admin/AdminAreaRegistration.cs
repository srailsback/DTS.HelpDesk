using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // Admin route so we don't have to say /admin/admin
            context.MapRoute(
                "AdminHome",
                "Admin",
                new { action = "Index", controller = "Admin" }
            );
            
            // password reset route
            context.MapRoute(
                "ResetPassword",
                "Admin/Account/ResetPassword/{code}",
                new { action = "Index", controller = "Account", area = "Admin" }
            );

            // default area route
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );


        }


    }
}