using DTS.HelpDesk.Areas.Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories
{
    public class Seeder
    {
        /// <summary>
        /// Sets up roles to seed.
        /// </summary>
        /// <returns></returns>
        public IList<IdentityRole> GetRoles()
        {
            return new List<IdentityRole>() {
                new IdentityRole() { 
                    Id = "46a1b5cf-02c6-4275-8e30-f3ca73faf56b", 
                    Name = "admin" 
                },
                new IdentityRole() {
                    Id = "cdd23f82-0916-4237-938e-7392ea18da9c",
                    Name = "test"
                }
            };
        }

        /// <summary>
        /// Sets up users to seed
        /// </summary>
        /// <returns></returns>
        public IList<ApplicationUser> GetUsers()
        {
            var passwordHasher = new PasswordHasher();
            var list = new List<ApplicationUser>();
            list.Add(new ApplicationUser() {
                Id = "5a56c13c-f26b-41b5-87e3-db209c4afeaf",
                UserName = "srailsback@dtsagile.com",
                Email = "srailsback@dtsagile.com",
                FirstName = "Steve",
                LastName = "Railsback",
                PasswordHash = passwordHasher.HashPassword("secret"),
                EmailConfirmed = true
            });
            
            list.Add(new ApplicationUser()
            {
                Id = "2c959275-472a-4de6-93ed-76ea8077dd00",
                UserName = "admin@dtsagile.com",
                Email = "admin@dtsagile.com",
                FirstName = "DTS",
                LastName = "Admin",
                PasswordHash = passwordHasher.HashPassword("secret"),
                EmailConfirmed = true
            });
            return list;
        }

        /// <summary>
        /// Sets up users with roles to seed
        /// </summary>
        /// <returns></returns>
        public IList<IdentityUserRole> GetUserRoles()
        {
            var list = new List<IdentityUserRole>();
            var adminRoleId = GetRoles().First(x => x.Name == "admin").Id;
            GetUsers().Where(x => x.Email.Contains("dtsagile.com"))
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => list.Add(new IdentityUserRole() { RoleId = adminRoleId, UserId = x }));
            
            var testRoleId = GetRoles().First(x => x.Name == "test").Id;
            GetUsers().Where(x => x.Email.Contains("dtsagile.com"))
                .Select(x => x.Id)
                .ToList()
                .ForEach(x => list.Add(new IdentityUserRole() { RoleId = testRoleId, UserId = x }));
            return list;
        }
    }
}