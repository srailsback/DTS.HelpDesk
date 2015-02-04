using DTS.DbDescriptionUpdater;
using Microsoft.AspNet.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace DTS.HelpDesk.Areas.Admin.Models
{
    public class FAQ
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DbColumnMeta(Description = "FAQ primary key.")]
        [Required(ErrorMessage = "Id required.")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DbColumnMeta(Description = "FAQ title, max length 100 characters.")]
        [Required(ErrorMessage = "Title required.")]
        [MaxLength(100)]
        public string Title { get; set;  }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [DbColumnMeta(Description = "FAQ content required, no max length, but should be short because this is after all an FAQ.")]
        [Required(ErrorMessage = "Content required.")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the create at UTC.
        /// </summary>
        /// <value>
        /// The create at UTC.
        /// </value>
        [DbColumnMeta(Description = "UTC timestamp when FAQ was created.")]
        public DateTime CreateAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the updated at UTC.
        /// </summary>
        /// <value>
        /// The updated at UTC.
        /// </value>
        [DbColumnMeta(Description = "UTC timestamp when FAQ was last modified.")]       
        public DateTime UpdatedAtUtc { get; set; }

        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>
        /// The views.
        /// </value>
        [DbColumnMeta(Description = "Total number of views / clicks.")]
        public int Views { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DbColumnMeta(Description = "Foreign key of user that created or modified the FAQ.")]       
         public string UserId { get; set; }

        [DbColumnMeta(Description = "Bool that sets published state of FAQ.")]
        public bool IsPublished { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public virtual ApplicationUser User { get; set; }


        public FAQ() { }
        public FAQ(FAQViewModel item)
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.Content = item.Content;
            this.CreateAtUtc = item.CreateAtUtc;
            this.UpdatedAtUtc = item.UpdatedAtUtc;
            this.Views = item.Views;
            this.UserId = item.UserId;
            //this.User = user;
        }



        /// <summary>
        /// Updates FAQ.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="user">The user.</param>
        public void Update(FAQViewModel item)
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.Content = item.Content;
            this.CreateAtUtc = item.CreateAtUtc;
            this.UpdatedAtUtc = item.UpdatedAtUtc;
            this.Views = item.Views;
            this.UserId = item.UserId;
            this.IsPublished = item.IsPublished;
        }
    }





    public class FAQViewModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }


        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Display(Name = "Title")]
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "Title must be less than 100 characters")]
        public string Title { get; set; }


        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [Display(Name = "Content")]
        [Required(ErrorMessage = "Content is required")]
        [AllowHtml]
        public string Content { get; set; }


        /// <summary>
        /// Gets or sets the create at UTC.
        /// </summary>
        /// <value>
        /// The create at UTC.
        /// </value>
        [Display(Name = "Created UTC")]
        [DataType(DataType.DateTime)]
        public DateTime CreateAtUtc { get; set; }


        /// <summary>
        /// Gets or sets the updated at UTC.
        /// </summary>
        /// <value>
        /// The updated at UTC.
        /// </value>
        [Display(Name = "Updated UTC")]
        [DataType(DataType.DateTime)]        
        public DateTime UpdatedAtUtc { get; set; }


        /// <summary>
        /// Gets or sets the views.
        /// </summary>
        /// <value>
        /// The views.
        /// </value>
       
        public int Views { get; set; }
        [Display(Name = "Published")]
        public bool IsPublished { get; set; }


        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQViewModel"/> class.
        /// </summary>
        public FAQViewModel()
        {
            this.Id = Guid.NewGuid().ToString();
            this.UserId = HttpContext.Current.User.Identity.GetUserId();
            this.CreateAtUtc = DateTime.UtcNow;
            this.UpdatedAtUtc = DateTime.UtcNow;
            this.Views = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FAQViewModel"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        public FAQViewModel(FAQ item) 
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.Content = item.Content;
            this.CreateAtUtc = item.CreateAtUtc;
            this.UpdatedAtUtc = item.UpdatedAtUtc;
            this.Views = item.Views;
            this.UserId = item.User.Id;
            this.User = item.User;
            this.IsPublished = item.IsPublished;
        }
    }


}