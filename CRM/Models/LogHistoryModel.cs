using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class LogHistoryModel
    {
        public string Url { get; set; }
        public string UrlReferrer { get; set; }
        public string UserEmail { get; set; }

        public LogHistoryModel(string url, string urlReffer, string userEmail)
        {
            this.Url = url;
            this.UrlReferrer = urlReffer;
            this.UserEmail = userEmail;
        }
    }
}