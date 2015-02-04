using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Mailer
{
    public class EmailBase
    {
        public string EmailViewName { get; set; }
        public List<string> To { get; set; }
        public List<string> CC { get; set; }
        public List<string> Bcc { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Subject { get; set; }
        public string Link { get; set; }
        public MailAddress Sender
        {
            get
            {
                return new MailAddress(ConfigurationManager.AppSettings["dts.helpdesk.NoReplyEmailAddress"]);
            }
        }

        public EmailBase()
        {
            this.To = new List<string>();
            this.CC = new List<string>();
            this.Bcc = new List<string>();
        }

    }
}