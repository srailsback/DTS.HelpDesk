using DTS.DataTables.MVC;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Logging;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
using DTS.HelpDesk.Areas.Admin.Infrastructure.Helpers;
using DTS.HelpDesk.Areas.Admin.Models;
using DTS.HelpDesk.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;

namespace DTS.HelpDesk.Areas.Admin.Controllers
{
    public class ArticlesController : AsyncBaseController
    {
        protected readonly IArticleRepository _articleRepository;

        public ArticlesController(ILogger logger, IArticleRepository artcleRepository) : base(logger) {
            this._articleRepository = artcleRepository;
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
            var data = _articleRepository.AllIncluding(x => x.User).ToList().Select(x => new ArticleViewModel(x));
            var filteredData = data;
            var searchValue = "";
            // filter data
            if (!string.IsNullOrWhiteSpace(requestModel.Search.Value))
            {
                searchValue = requestModel.Search.Value.ToLower();
                filteredData = filteredData.Where(x =>
                    x.Title.ToLower().Contains(searchValue) ||
                    x.User.FirstName.ToLower().Contains(searchValue) ||
                    x.User.LastName.ToLower().Contains(searchValue) ||
                    x.User.Email.Contains(searchValue));
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
                new ColumnHeader() { data = "CreatedAtUTC", name = "Created UTC", visible = true, sortable = true, className = "text-center" },
                new ColumnHeader() { data = "UpdatedAtUTC", name = "Updated UTC", visible = true, sortable = true },
                new ColumnHeader() { data = "PublishAtUTC", name = "Publish UTC", visible = true, sortable = true },
                new ColumnHeader() { data = "UnpublishAtUTC", name = "Unpublish UTC", visible = true, sortable = true },
                new ColumnHeader() { data = "Clicks", name = "Clicks", visible = true, sortable = true },
            });
        }

        #endregion



        #region Create

        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            var article = new Article();
            var model = new ArticleViewModel(article);
            return View("Create", model);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Create(ArticleViewModel model)
        {
            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                model.UserId = User.Identity.GetUserId();
                var article = new Article(model);
                _articleRepository.Add(article);
                _articleRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Create", model);
        }


        #endregion



        #region Update

        [Authorize(Roles = "admin")]
        public ActionResult Edit()
        {
            var article = new Article();
            var model = new ArticleViewModel(article);
            return View("Create", model);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(ArticleViewModel model, string toDo = "")
        {
            if (toDo.ToLower() == "delete")
            {
                _articleRepository.Delete(model.Id);
                _articleRepository.Save();
                return RedirectToAction("Index");
            }



            ModelState.Remove("Id");
            if (ModelState.IsValid)
            {
                model.UserId = User.Identity.GetUserId();
                var article = _articleRepository.Find(model.Id);
                _articleRepository.Update(article);
                _articleRepository.Save();
                return RedirectToAction("Index");
            }
            return View("Edit", model);
        }



        #endregion

    }
}