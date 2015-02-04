using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class SocialController : AsyncBaseController
    {
        public SocialController(ILogger logger) : base(logger) { } 

        // GET: Admin/Social
        public ActionResult Index()
        {
            return View();
        }
    }
}