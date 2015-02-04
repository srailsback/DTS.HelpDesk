using Mvc.Mailer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace DTS.HelpDesk.Areas.Admin.Infrastructure.Mailer
{
    public interface IMailer
    {
        MvcMailMessage ForgotPassword(string to, string resetToken);
        MvcMailMessage AccountCreated(string to, string userName, string password);
        MvcMailMessage AccountUpdated(string to, string userName, string password);
    }

    public class Mailer : MailerBase, IMailer
    {
        private string NoReplyEmailAddress = ConfigurationManager.AppSettings["NoReplyEmailAddress"];

        public Mailer()
        {
            MasterName = "_EmailLayout";
        }


        public MvcMailMessage ForgotPassword(string to, string resetToken)
        {
            ViewBag.ResetToken = resetToken;
            return BuildMailMessage(to, "DTS HelpDesk Password Reset", this.NoReplyEmailAddress, "ForgotPassword");
        }

        public MvcMailMessage AccountCreated(string to, string userName, string password)
        {
            ViewBag.Username = userName;
            ViewBag.Password = password;
            return BuildMailMessage(to, "DTS HelpDesk Account Created", this.NoReplyEmailAddress, "AccountCreated");
        }

        public MvcMailMessage AccountUpdated(string to, string userName, string password)
        {
            ViewBag.Username = userName;
            ViewBag.Password = password;
            return BuildMailMessage(to, "DTS HelpDesk Account Updated", this.NoReplyEmailAddress, "AccountUpdated");
        }

        private MvcMailMessage BuildMailMessage(string to, string subject, string sender, string viewName)
        {
            return Populate(x => { 
                x.To.Add(to);
                x.Sender = new MailAddress(!string.IsNullOrEmpty(sender) ?  sender : this.NoReplyEmailAddress);
                x.Subject = subject;
                x.ViewName = viewName;
            });
        }


    }
}