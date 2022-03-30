using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

 namespace identity.Reposetory
{
    public class MessageSender : IMessageSender
    {
        public Task SendEmailAsync(string toEmail, string subject, string message, bool isMessageHtml = false)
        {
            //using (var client = new SmtpClient())
            //{

            //    var credentials = new NetworkCredential()
            //    {
            //        UserName = "alirezang7575", // without @gmail.com
            //        Password = "5537544700aarr",

            //    };

            //    client.Credentials = credentials;
            //    client.Host = "smtp.gmail.com";
            //    client.Port = 587;
            //    client.EnableSsl = true;

            //    using var emailMessage = new MailMessage()
            //    {
            //        To = { new MailAddress(toEmail) },
            //        From = new MailAddress("alirezang7575@gmail.com"), // with @gmail.com
            //        Subject = subject,
            //        Body = message,
            //        IsBodyHtml = isMessageHtml,

            //    };

            //    client.Send(emailMessage);
            //}
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("alirezang7575@gmail.com");
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = isMessageHtml;
              

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("alirezang7575@gmail.com", "5537544700aarr");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return Task.CompletedTask;
        }
    }
}
