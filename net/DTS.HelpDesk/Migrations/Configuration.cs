namespace DTS.HelpDesk.Migrations
{
    using DTS.DbDescriptionUpdater;
    using DTS.HelpDesk.Areas.Admin.Infrastructure.Repositories;
    using DTS.HelpDesk.Areas.Admin.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HelpDeskContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        /// <summary>
        /// Seeds the database
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void Seed(HelpDeskContext context)
        {
            var seeder = new Seeder();
            SeedRoles(context, seeder.GetRoles());
            SeedUsers(context, seeder.GetUsers());
            SeedUserRoles(context, seeder.GetUserRoles());
            

            // update descriptions
            DbDescriptionUpdater<HelpDeskContext> updater = new DbDescriptionUpdater<HelpDeskContext>(context);
            updater.UpdateDatabaseDescriptions();

        }


        /// <summary>
        /// Debugs the seeder
        /// </summary>
        /// <param name="context">The context.</param>
        /// <remarks>Method hooks into DebugController we we can step through and debug seeding</remarks>
        public void Debug(HelpDeskContext context)
        {
            Seed(context);
        }

        /// <summary>
        /// Seeds the roles.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="roles">The roles.</param>
        private void SeedRoles(HelpDeskContext context, IList<IdentityRole> roles)
        {
            // role manager
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role.Name))
                {
                    roleManager.Create(role);
                }
            }
        }

        /// <summary>
        /// Seeds the users.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="users">The users.</param>
        private void SeedUsers(HelpDeskContext context, IList<ApplicationUser> users)
        {

            // user manager
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var item in users)
            {
                var user = userManager.FindById(item.Id);
                if (user == null)
                {
                    userManager.Create(item);
                }
                else
                {
                    user.Update(item);
                    userManager.Update(user);
                }

            }
        }

        /// <summary>
        /// Seeds the user roles.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="userRoles">The user roles.</param>
        private void SeedUserRoles(HelpDeskContext context, IList<IdentityUserRole> userRoles)
        {
           
            // role manager
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            // user manager
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            foreach (var userRole in userRoles)
            {
                var currentRoles = userManager.FindById(userRole.UserId).Roles;
                if (!currentRoles.Any(x => x.RoleId == userRole.RoleId))
                {
                    var roleName = roleManager.FindById(userRole.RoleId).Name;
                    userManager.AddToRole(userRole.UserId, roleName);
                }
            }
        }
    }
}