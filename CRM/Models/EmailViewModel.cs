using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class EmailViewModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime SentDate { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailViewModel(MimeMessage mail)
        {
            this.From = mail.From.ToString();
            this.To = mail.To.ToString();
            this.SentDate = DateTime.Parse(mail.Date.ToString());
            this.Subject = mail.Subject;
            this.Body = mail.TextBody;
        }
    }
}