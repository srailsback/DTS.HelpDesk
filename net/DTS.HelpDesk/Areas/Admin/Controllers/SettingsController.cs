using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class SettingsController : AsyncBaseController
    {
        public SettingsController(ILogger logger) : base(logger) { }
 
        // GET: Admin/Settings
        public ActionResult Index()
        {
            return View();
        }
    }
}