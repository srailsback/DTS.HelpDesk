using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
using DTS.HelpDesk.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class DebugController : Controller
    {
        private HelpDeskContext _context;

        public DebugController(HelpDeskContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Debug the seeder
        /// </summary>
        /// <returns>Hit action when in debug mode</returns>
        public ActionResult Index()
        {
            var config = new Configuration();
            config.Debug(this._context);
            return RedirectToAction("Index", new { controller = "Admin", area = "Admin" });
        }
    }

}