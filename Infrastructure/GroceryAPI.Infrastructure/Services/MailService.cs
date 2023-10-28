using GroceryAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace GroceryAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] {to}, subject, body, isBodyHtml);
        }

        public Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mail.To.Add(to);
            mail.Body = body;
            mail.Subject = subject;
            mail.From = new(_configuration["Mail:Username"], _configuration["Mail:FromName"]);

            SmtpClient smtp = new(_configuration["Mail:Host"], Convert.ToInt32(_configuration["Mail:Port"])); 
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:AppPassword"]);
            smtp.EnableSsl = true;
            

            smtp.Send(mail);
            return Task.CompletedTask;
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Hello,<br> If you want to reset your password, you can reset click link below.<br/><strong>");
            mail.AppendLine("<a href=\"" + _configuration["AngularClientUrl"] + "/reset-password/" + userId + "/" + resetToken + "\" target=\"_blank\">");
            mail.AppendLine("Click for reset password</a></strong><br/><br/><span style='font-size:12px;'>Note: If you do not send this operation, Please don't take it seriously.</span><br/><br/><a href='http://localhost.4200'>Grocery Market</a>");

            await SendMailAsync(to, "Reset Password", mail.ToString());
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderNumber, DateTime orderDate, string userFullName)
        {
            string mail = $"Hello {userFullName}, <br>Your order with number {orderNumber} that you placed on {orderDate} has been received.<br>It will be prepared and delivered to you within 30-45 minutes. <br>If you have any problem about your order, please contact us <strong>###-##-##</strong> <br><a href='http://localhost.4200'>Grocery Market</a>";
            await SendMailAsync(to, "Order Received", mail);
        }

        public async Task SendContactUsMail(string type, string nameSurname, string phoneNumber, string message)
        {
            string mail = $"Dear Customer Services,<br><br>There is a {type} sent to you by {nameSurname}.<br><br>You can find details below.<br><br><b>User Information<b><br>Name Surname : {nameSurname}<br>Phone Number: {phoneNumber}<br><br><b>MessageType:<b> {type}<br><b>Message<b><br>{message}";
            await SendMailAsync(_configuration["Mail:Username"], "User Request", mail);
        }
    }
}
