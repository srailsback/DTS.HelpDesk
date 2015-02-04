using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public string RoleId { get; set; }
        public string Name { get; set; }

        public RoleViewModel() {}
        public RoleViewModel(IdentityRole item) {
            this.RoleId = item.Id;
            this.Name = item.Name;
        }
    }
}