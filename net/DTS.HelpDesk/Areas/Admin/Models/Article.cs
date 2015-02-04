using DTS.DbDescriptionUpdater;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Models
{
    public class Article
    {
        [DbColumnMeta(Description = "Article primary key.")]
        public string Id { get; set; }

        [DbColumnMeta(Description = "Article title.")]  
        public string Title { get; set; }

        [DbColumnMeta(Description = "Article friendly url slug.")]  
        public string FriendlyTitle { get; set; }

        [DbColumnMeta(Description = "Article content.")]
        public string Content { get; set; }

        [DbColumnMeta(Description = "UTC timestamp when article was created.")]
        public DateTime CreatedAtUTC { get; set; }

        [DbColumnMeta(Description = "UTC timestamp when article was last updated.")]
        public DateTime UpdatedAtUTC { get; set; }

        [DbColumnMeta(Description = "UTC timestamp when article should be published.")]
        public DateTime PublishAtUTC { get; set; }

        [DbColumnMeta(Description = "UTC timestamp when article should be unplublished.")]
        public DateTime? UnpublishAtUTC { get; set; }

        public int Clicks { get; set; }

        [DbColumnMeta(Description = "Id of user that created or updated the article.")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public Article() {
            this.PublishAtUTC = DateTime.UtcNow;
            this.UnpublishAtUTC = DateTime.UtcNow.AddYears(1);
        }
        public Article(ArticleViewModel item) 
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.FriendlyTitle = item.FriendlyTitle;
            this.Content = item.Content;
            this.CreatedAtUTC = item.CreatedAtUTC;
            this.UpdatedAtUTC = item.UpdatedAtUTC;
            this.PublishAtUTC = item.PublishAtUTC;
            this.UnpublishAtUTC = item.UnpublishAtUTC;
            this.UserId = item.UserId;
        }

        public void Update(ArticleViewModel item)
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.FriendlyTitle = item.FriendlyTitle;
            this.Content = item.Content;
            this.CreatedAtUTC = item.CreatedAtUTC;
            this.UpdatedAtUTC = item.UpdatedAtUTC;
            this.PublishAtUTC = item.PublishAtUTC;
            this.UnpublishAtUTC = item.UnpublishAtUTC;
            this.UserId = item.UserId;
            this.User = item.User;

        }

    }


    public class ArticleViewModel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(100, ErrorMessage = "Title cannot be more than 100 characters.")]
        public string Title { get; set; }

        [Display(Name = "Friendly Url")]
        [Required(ErrorMessage = "Friendly Url is required." )]
        public string FriendlyTitle { get; set; }

        [Display(Name = "Content")]
        [AllowHtml]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAtUTC { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime UpdatedAtUTC { get; set; }

        [Display(Name = "Publish")]
        [DataType(DataType.DateTime)]
        public DateTime PublishAtUTC { get; set; }

        [Display(Name = "Unpublish")]
        [DataType(DataType.DateTime)]
        public DateTime? UnpublishAtUTC { get; set; }

        public int Clicks { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string Editor
        {
            get
            {
                var editorName = this.User.FirstName + " " + this.User.LastName;
                return !string.IsNullOrWhiteSpace(editorName) ? editorName : this.User.Email;
            }
        }



        public ArticleViewModel() { }

        public ArticleViewModel(Article item)
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.FriendlyTitle = item.FriendlyTitle;
            this.Content = item.Content;
            this.CreatedAtUTC = item.CreatedAtUTC;
            this.UpdatedAtUTC = item.UpdatedAtUTC;
            this.PublishAtUTC = item.PublishAtUTC;
            this.UnpublishAtUTC = item.UnpublishAtUTC;
            this.Clicks = item.Clicks;
            this.UserId = item.UserId;
            this.User = item.User;
        }

    }
}