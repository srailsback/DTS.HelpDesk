using DTS.HelpDesk.Areas.Admin.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories
{
    //public class HelpDeskContext : ApplicationDbContext
    public class HelpDeskContext : IdentityDbContext
    {
        public HelpDeskContext()
            : base("HelpDeskContext")
        {
        }

        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Article> Articles { get; set; }
    }

}