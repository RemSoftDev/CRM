using CRM.Interfaces;
using CRM.Models;
using System;
using System.Collections.Generic;

namespace CRM.Services
{
	public interface IEmailService
	{
		List<EmailViewModel> GetMailings<T>(DateTime? lastReceivedDate, T user) where T : IUser;
		List<EmailViewModel> LoadMails<T>(T user) where T : IUser;
		void SendEmail<T>(EmailViewModel model, T user) where T : IUser;
		void SendEmail(EmailViewModel model);
	}
}