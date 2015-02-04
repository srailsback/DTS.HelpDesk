using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Models
{
    public class LookupBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}