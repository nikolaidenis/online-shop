using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;

namespace OnlineShop.Api.Helpers
{
    public class EmailService : IIdentityMessageService, IDisposable
    {
        #region Creds
        private string From => "onlineshoptestproj@gmail.com";
        private string Password => "adminadmin";
        private string ConfirmationUrl => "http://localhost:3315/api/confirmation";
#endregion
        public async Task CreateConfirmationEmail(string token, string emailTo)
        {
            var msg = new IdentityMessage
            {
                Subject = "Online Shop Email Confirmation",
                Destination = emailTo,
                Body = token
            };
            await SendAsync(msg);
        }

        public Task SendAsync(IdentityMessage message)
        {
            return SendMail(message);
        }

        Task SendMail(IdentityMessage message)
        {
            #region formatter
            string text = $"Please click on this link to {message.Subject}: {message.Body}";
            string html = "Please confirm your account by clicking this  <a href='"+ ConfirmationUrl+ "/" + message.Body + "'>link</a>.<br/>";

//            html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
            #endregion

            var msg = new MailMessage();
            msg.From = new MailAddress(From);
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            var smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(587));
            var credentials = new System.Net.NetworkCredential(From, Password);
            smtpClient.Credentials = credentials;
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            
        }
    }
}