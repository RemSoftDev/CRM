using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections.Generic;
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

        public static void GetMailings()
        {
            using (var client = new ImapClient())
            {
                using (var cancel = new CancellationTokenSource())
                {
                    client.Connect("imap.gmail.com", 993, true, cancel.Token);
                    client.AuthenticationMechanisms.Remove("XOAUTH");

                    client.Authenticate(Email, Password, cancel.Token);

                    // The Inbox folder is always available...
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly, cancel.Token);

                    Console.WriteLine("Total messages: {0}", inbox.Count);
                    Console.WriteLine("Recent messages: {0}", inbox.Recent);
                    // download each message based on the message index
                    for (int i = 0; i < inbox.Count; i++)
                    {
                        var message = inbox.GetMessage(i, cancel.Token);
                        var body = message.GetTextBody(MimeKit.Text.TextFormat.Text);
                        Console.WriteLine("Subject: {0}", message.Subject);
                    }

                    // let try searching for some messages...
                    //var query = SearchQuery.DeliveredAfter(DateTime.Parse("2013-01-12"))
                    //	.And(SearchQuery.SubjectContains("MailKit"))
                    //	.And(SearchQuery.Seen);
                    var query = SearchQuery.NotSeen;

                    foreach (var uid in inbox.Search(query, cancel.Token))
                    {
                        var message = inbox.GetMessage(uid, cancel.Token);
                        Console.WriteLine("[match] {0}: {1}", uid, message.Subject);
                    }

                    client.Disconnect(true, cancel.Token);
                }
            }


        }
    }
}