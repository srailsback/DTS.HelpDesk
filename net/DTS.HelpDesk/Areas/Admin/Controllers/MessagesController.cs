using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class MessagesController : AsyncBaseController
    {
        public MessagesController(ILogger logger) : base(logger) { }

        // GET: Admin/Messages
        public ActionResult Index()
        {
            return View();
        }
    }
}