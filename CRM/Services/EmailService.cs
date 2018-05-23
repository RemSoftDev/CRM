using AutoMapper;
using CRM.Interfaces;
using CRM.Models;
using CRM.DAL.Contexts;
using CRM.DAL.Entities;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using static System.Security.Authentication.SslProtocols;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using CRM.DAL.Repository;

namespace CRM.Services
{
    public class EmailService : IEmailService
	{
		private readonly IUnitOfWork _unitOfWork;

		public EmailService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

        private const string Email = "csharpcrm@gmail.com";
        private const string Password = "CRM123qaz@!";
        public void SendEmail<T>(EmailViewModel model, T user) where T : IUser
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
            new Task(() => { SaveEmail(model, user); }).Start();
        }

        public List<EmailViewModel> GetMailings<T>(DateTime? lastReceivedDate, T user) where T : IUser
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
                    client.SslProtocols = Tls | Tls11 | Tls12;
                    client.ServerCertificateValidationCallback = 
                        new RemoteCertificateValidationCallback(ServificateValidationCallBack);

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
                        if (lastReceivedDate != null && message.Date > lastReceivedDate.Value)
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

        private bool ServificateValidationCallBack(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void SaveEmail<T>(List<EmailViewModel> models, T user) where T : IUser
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

			_unitOfWork.EmailsRepository.AddRange(emails);
			_unitOfWork.Save();

		}

		private void SaveEmail<T>(EmailViewModel model, T user) where T : IUser
        {

            using (var client = new ImapClient())
            {
                var item = Mapper.Map<EmailViewModel, Email>(model);

                if (user is LeadViewModel || user is CreateLeadViewModel)
                {
                    item.LeadId = user.Id;
                }
                if (user is UserViewModel)
                {
                    item.UserId = user.Id;
                }
				_unitOfWork.EmailsRepository.Create(item);
				_unitOfWork.Save();
            }
        }
        public List<EmailViewModel> LoadMails<T>(T user) where T : IUser
        {
            List<EmailViewModel> emails = new List<EmailViewModel>();
            {
                if (user is LeadViewModel)
                {
                    var items = _unitOfWork.EmailsRepository.Get()
                    .Where(e => e.LeadId == user.Id)
                    .ToList();
                    emails = Mapper.Map<List<Email>, List<EmailViewModel>>(items);
                }
                else if (user is UserViewModel)
                {
                    var items = _unitOfWork.EmailsRepository.Get()
                    .Where(e => e.UserId == user.Id)
                    .ToList();
                    emails = Mapper.Map<List<Email>, List<EmailViewModel>>(items);
                }
            }
            return emails;
        }
    }
}