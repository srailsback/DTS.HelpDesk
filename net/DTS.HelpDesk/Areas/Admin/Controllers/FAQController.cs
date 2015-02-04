using DTS.DataTables.MVC;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
using DTS.HelpDesk.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Helpers;
using System;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class FAQController : AsyncBaseController
    {
        protected readonly IFAQRepository _faqRepository;

        public FAQController(ILogger logger, IFAQRepository faqRepository) : base(logger) {
            this._faqRepository = faqRepository;
        }

        #region List

        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            ViewBag.Columns = GetColumns();
            return View("Index");
        }
        
        [AllowAnonymous]
        public JsonResult GetData([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel)
        {
            // get all users
            var data = _faqRepository.AllIncluding(x => x.User).ToList().Select(x => new FAQViewModel(x));
            var filteredData = data;
            var searchValue = "";
            // filter data
            if (!string.IsNullOrWhiteSpace(requestModel.Search.Value))
            {
                searchValue = requestModel.Search.Value.ToLower();
                filteredData = filteredData.Where(x =>
                    x.Title.ToLower().Contains(searchValue) ||
                    x.Editor.ToLower().Contains(searchValue));
            }

            // get sorting
            var sortColumns = string.Join(", ",
                requestModel.Columns.GetSortedColumns()
                .Select(
                    x => string.Format("{0} {1}", x.Data, x.SortDirection.ToString().ToLower().Contains("asc") ? "ASC" : "DESC")
                ).ToArray());

            // get paged data
            var pagedData = filteredData
                .OrderBy(sortColumns)
                .Skip(requestModel.Start)
                .Take(requestModel.Length);

            var result = new DataTablesResponse(requestModel.Draw, pagedData, data.Count(), data.Count());
            return Json(result);
        }

        private string GetColumns()
        {
            return JsonConvert.SerializeObject(new List<ColumnHeader>() { 
                new ColumnHeader() { data = "Id", name = "", visible = true, sortable = false, width = "50px", className = "text-center"  },
                new ColumnHeader() { data = "Title", name = "Title", visible = true, sortable = true },
                new ColumnHeader() { data = "Editor", name = "Editor", visible = true, sortable = true },
                new ColumnHeader() { data = "IsPublished", name = "Published", visible = true, sortable = true, className = "text-center" },
                new ColumnHeader() { data = "Views", name = "Views", visible = true, sortable = true },
                new ColumnHeader() { data = "CreateAtUtc", name = "Created UTC", visible = true, sortable = true },
                new ColumnHeader() { data = "UpdatedAtUtc", name = "Updated UTC", visible = true, sortable = true },

            });
        }


        #endregion

        #region Create

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            AddToViewBag();
            return View("Create", new FAQViewModel());
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(FAQViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                model.UserId = User.Identity.GetUserId();
                model.CreateAtUtc = DateTime.Now;
                model.UpdatedAtUtc = DateTime.Now;
                var faq = new FAQ(model);
                _faqRepository.Add(faq);
                _faqRepository.Save();
                return RedirectToAction("Index");
            }
            AddToViewBag();
            return View("Create", model);
        }

        #endregion

        #region Edit

        [Authorize(Roles = "admin")]
        public ActionResult Edit(string id)
        {
            var faq = _faqRepository.Find(id);
            var model = new FAQViewModel(_faqRepository.Find(id));
            AddToViewBag();
            return View("Edit", model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FAQViewModel model, string toDo = "")
        {
            if (toDo.ToLower() == "delete") {
                _faqRepository.Delete(model.Id);
                _faqRepository.Save();
                return RedirectToAction("Index");
            }


            if (ModelState.IsValid)
            {
                var faq = _faqRepository.Find(model.Id);
                model.UserId = User.Identity.GetUserId();
                faq.Update(model);
                _faqRepository.Update(faq);
                _faqRepository.Save();
                return RedirectToAction("Index");
            }
            AddToViewBag();
            return View("Edit", model);
        }

        [HttpPost]
        public void Publish(string id)
        {
            var faq = _faqRepository.Find(id);
            faq.IsPublished = !faq.IsPublished ? true : false;
            _faqRepository.Update(faq);
            _faqRepository.Save();
        }
        #endregion



        private void AddToViewBag() {
            ViewBag.YesOrNo = new List<SelectListItem>() { 
                new SelectListItem() { Text = "No", Value = false.ToString() },
                new SelectListItem() { Text = "Yes", Value = true.ToString() }
            };

        }



    }
}