using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class ModulesController : AsyncBaseController
    {
        public ModulesController(ILogger logger) : base(logger) { }
        
        // GET: Admin/Modules
        public ActionResult Index()
        {
            return View();
        }
    }
}