using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Payroll_Manager.Utilities
{
    public class MailHelper
    {
        public IConfiguration Configuration;
        public void SendMail(string toEmailAddress, string subject, string content,string image)
        {
            var fromAddress = new MailAddress("khoitedu99@gmail.com", "Tin nhắn hệ thống EventNet");
            var toAddress = new MailAddress(toEmailAddress, "Mr Test");
            const string fromPassword = "Fizz1999";
            string body = content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                if (image == null)
                {
                    smtp.Send(message);
                }
                else
                {
                    Attachment attachment = new Attachment("wwwroot/Product_images/" + image);
                    message.Attachments.Add(attachment);
                    smtp.Send(message);
                }
            }
        }
    }
}