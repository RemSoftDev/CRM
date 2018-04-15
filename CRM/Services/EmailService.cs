using System.Net;
using System.Net.Mail;

namespace CRM.Services
{
    public static class EmailService
    {
        public static void SendEmail(string recepient, string subject, string body)
        {
            var fromAddress = new MailAddress("from@gmail.com", "CRM");
            var toAddress = new MailAddress(recepient);
            const string fromPassword = "fromPassword";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                //smtp.Send(message);
            }
        }
    }
}