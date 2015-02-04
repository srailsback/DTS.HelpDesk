using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;

namespace DTS.HelpDesk.Areas.Admin.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PasswordResetToken { get; set; }
        public bool RequirePasswordReset { get; set; }
        public string TimezoneId { get; set; }

        public void Update(ApplicationUser item)
        {
            this.Id = item.Id;
            this.UserName = item.UserName;
            this.Email = item.Email;
            this.EmailConfirmed = item.EmailConfirmed;
            this.FirstName = item.FirstName;
            this.LastName = item.LastName;
            this.PasswordHash = item.PasswordHash;
            this.PasswordResetToken = item.PasswordResetToken;
            this.PhoneNumber = item.PhoneNumber;
            this.RequirePasswordReset = item.RequirePasswordReset;
        }

        public void Update(ProfileViewModel item)
        {
            this.UserName = item.Email;
            this.Email = item.Email;
            this.FirstName = item.FirstName;
            this.LastName = item.LastName;
            this.PhoneNumber = item.Phone;
            this.LockoutEnabled = item.IsLockedOut;
            this.RequirePasswordReset = item.RequirePasswordReset;
            this.PasswordResetToken = item.PasswordResetToken;
        }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("HelpDeskContext", throwIfV1Schema: false)        
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}