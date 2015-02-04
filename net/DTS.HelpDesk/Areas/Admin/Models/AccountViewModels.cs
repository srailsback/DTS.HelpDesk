using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
namespace DTS.HelpDesk.Areas.Admin.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ProfileViewModel
    {
        private ApplicationUser user;
        private ICollection<Microsoft.AspNet.Identity.EntityFramework.IdentityUserRole> collection;

        [Required(ErrorMessage = "Id required.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Email address required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First name required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last name required.")]
        public string LastName { get; set; }


        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Password invalid.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage = "Invalid phone.")]
        public string Phone { get; set; }

        public ICollection<RoleViewModel> Roles { get; set; }
        public List<string> RoleIds { get; set; }

        public string EmailConfirmed { get; set; }

        public bool IsLockedOut { get; set; }

        public bool RequirePasswordReset { get; set; }

        public string PasswordResetToken { get; set; }

        public string DirectoryName
        {
            get
            {
                return string.Format("{0}, {1}", this.LastName, this.FirstName);
            }
        }

        public string ProperName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public ProfileViewModel() {
            this.Roles = new List<RoleViewModel>();
            this.RoleIds = new List<string>();
        }

        public ProfileViewModel(ApplicationUser user, IQueryable<RoleViewModel> allRoles)
        {

            this.Id = user.Id;
            this.Email = user.Email;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Phone = user.PhoneNumber;
            this.EmailConfirmed = user.EmailConfirmed ? "Yes" : "No";
            this.IsLockedOut = user.LockoutEnabled;
            this.RequirePasswordReset = user.RequirePasswordReset;
            this.PasswordResetToken = user.PasswordResetToken;
            this.Roles = new List<RoleViewModel>();
            foreach (var role in user.Roles)
            {
                this.Roles.Add(allRoles.First(x => x.RoleId == role.RoleId));
            }
            this.RoleIds = this.Roles.Select(x => x.RoleId).ToList();

        }

    }
}
