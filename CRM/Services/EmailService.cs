using CRM.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Xml;

namespace CRM.Services
{
    public static class EmailService
    {
        private const string Email = "csharpcrm@gmail.com";
        private const string Password = "CRM123qaz@!";
        public static void SendEmail(string recepient, string subject, string body)
        {
            var fromAddress = new MailAddress(Email, "CRM");
            var toAddress = new MailAddress(recepient);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Email, Password)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }

        public static List<EmailViewModel> GetMailings(string sentFrom)
        {
            List<EmailViewModel> emailsList = new List<EmailViewModel>();
            string searchDate = "01.04.2018 8:34:56";

            using (var client = new ImapClient())
            {
                using (var cancel = new CancellationTokenSource())
                {
                    client.Connect("imap.gmail.com", 993, true, cancel.Token);
                    client.AuthenticationMechanisms.Remove("XOAUTH");

                    client.Authenticate(Email, Password, cancel.Token);

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly, cancel.Token);

                    var query = SearchQuery.NotSeen
                        .And(SearchQuery.DeliveredAfter(DateTime.Parse(searchDate)));

                    if (!string.IsNullOrEmpty(sentFrom))
                    {
                        query = query.And(SearchQuery.FromContains(sentFrom));
                    }
                        
                    var result = inbox.Search(query, cancel.Token);

                    foreach (var uid in result)
                    {
                        var message = inbox.GetMessage(uid, cancel.Token);
                        emailsList.Add(new EmailViewModel(message));
                    }

                    client.Disconnect(true, cancel.Token);
                }
            }
            return emailsList;
        }
    }
}