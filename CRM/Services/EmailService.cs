using AutoMapper;
using CRM.Interfaces;
using CRM.Models;
using CRMData.Contexts;
using CRMData.Entities;
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
using System.Threading.Tasks;
using System.Xml;

namespace CRM.Services
{
    public static class EmailService
    {
        private const string Email = "csharpcrm@gmail.com";
        private const string Password = "CRM123qaz@!";
        public static void SendEmail<T>(EmailViewModel model, T user) where T : IUser
        {
            var fromAddress = new MailAddress(Email, "CRM");
            var toAddress = new MailAddress(model.To);

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
                Subject = model.Subject,
                Body = model.Body
            })
            {
                smtp.Send(message);
            }

            model.SentDate = DateTime.Now;
            new Task(() => { SaveEmail<T>(model, user); }).Start();
        }

        public static List<EmailViewModel> GetMailings<T>(DateTime? lastReceivedDate, T user) where T : IUser
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return null;
            }

            List<EmailViewModel> emailsList = new List<EmailViewModel>();

            using (var client = new ImapClient())
            {
                using (var cancel = new CancellationTokenSource())
                {
                    client.Connect("imap.gmail.com", 993, true, cancel.Token);
                    client.AuthenticationMechanisms.Remove("XOAUTH");
                    client.Authenticate(Email, Password, cancel.Token);


                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly, cancel.Token);

                    SearchQuery query = SearchQuery.FromContains(user.Email);

                    if (lastReceivedDate != null)
                    {
                        query = query.And(SearchQuery.SentAfter(lastReceivedDate.Value));
                    }

                    var result = inbox.Search(query, cancel.Token);

                    foreach (var uid in result)
                    {
                        var message = inbox.GetMessage(uid, cancel.Token);
                        if (message.Date > lastReceivedDate.Value)
                        {
                            emailsList.Add(new EmailViewModel(message));
                        }
                    }

                    client.Disconnect(true, cancel.Token);
                }
            }

            if (emailsList.Any())
            {
                new Task(() => { SaveEmail(emailsList, user); }).Start();
            }

            return emailsList;
        }

        private static void SaveEmail<T>(List<EmailViewModel> models, T user) where T : IUser
        {
            using (BaseContext context = new BaseContext())
            {
                var emails = Mapper.Map<List<EmailViewModel>, List<Email>>(models);

                if (user is LeadViewModel)
                {
                    foreach (var email in emails)
                    {
                        email.LeadId = user.Id;
                    }

                }
                else if (user is UserViewModel)
                {
                    foreach (var email in emails)
                    {
                        email.UserId = user.Id;
                    }
                }
                context.Emails.AddRange(emails);
                context.SaveChanges();
            }
        }

        private static void SaveEmail<T>(EmailViewModel model, T user) where T : IUser
        {

            using (var client = new ImapClient())
            using (BaseContext context = new BaseContext())
            {
                var item = Mapper.Map<EmailViewModel, Email>(model);

                if (user is LeadViewModel)
                {
                    item.LeadId = user.Id;
                }
                if (user is UserViewModel)
                {
                    item.UserId = user.Id;
                }
                context.Emails.Add(item);
                context.SaveChanges();
            }
        }
        public static List<EmailViewModel> LoadMails<T>(T user) where T : IUser
        {
            List<EmailViewModel> emails = new List<EmailViewModel>();
            using (BaseContext context = new BaseContext())
            {
                if (user is LeadViewModel)
                {
                    var items = context
                    .Emails
                    .Where(e => e.LeadId == user.Id)
                    .ToList();
                    emails = Mapper.Map<List<Email>, List<EmailViewModel>>(items);
                }
                else if (user is UserViewModel)
                {
                    var items = context
                    .Emails
                    .Where(e => e.UserId == user.Id)
                    .ToList();
                    emails = Mapper.Map<List<Email>, List<EmailViewModel>>(items);
                }
            }
            return emails;
        }
    }
}