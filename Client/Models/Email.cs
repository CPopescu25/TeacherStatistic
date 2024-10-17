using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Client.Models
{
    public class Email:IEmail
    {
        private const string fromPassword = "Test-1234";

        private const string fromEmail = "test1234@gmail.com";

        private const string fromTitle = "From Statistics Application";

        private const string Host = "smtp.gmail.com";

        private const int Port = 587;

        private string toAddress;

        private string toTitle;

        private string Subject;

        private string Body;
        
        protected MailAddress MailAddress(string a, string b)
        {
            return new MailAddress(a, b);
        }

        protected NetworkCredential NetworkCredential()
        {
            return new NetworkCredential(MailAddress(fromEmail, fromTitle).Address, fromPassword);
        }

        protected SmtpClient SmtpClient()
        {
            return new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = NetworkCredential()
            };
        }

        public MailMessage MailMessage()
        {
            return new MailMessage(MailAddress(fromEmail, fromTitle), MailAddress(toAddress, toTitle))
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = true
            };
        }

        public virtual void SendEmail(Emailmodel model)
        {
            toAddress = model.toAddress;
            toTitle = model.Username;
            Subject = model.Subject;
            Body = model.Body;

            SmtpClient().Send(MailMessage());
        }
    }
}
