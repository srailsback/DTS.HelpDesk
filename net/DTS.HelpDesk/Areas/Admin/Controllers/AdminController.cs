using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class AdminController : AsyncBaseController
    {
        protected readonly IFAQRepository _faqRepository;
        public AdminController(ILogger logger, IFAQRepository faqRepository) : base(logger) {
            this._faqRepository = faqRepository;
        }

        // GET: Admin/Home
        public  ActionResult Index()
        {
            this.AlertError("This is an alert");
            var tempDataKeys = new string[] { "info", "success", "warning", "error" };

            var faqs = _faqRepository.All;

            ViewBag.TotalFAQs = faqs.Count();
            ViewBag.NewFAQs = faqs.Where(x => x.IsPublished == false).Count();

            return View("Index");
        }
    }
}