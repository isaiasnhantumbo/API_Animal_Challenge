using System;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Http;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Mail
{
    public class SendMail : ISendConfirmationEmail
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SendMail(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendConfirmationEmail(AppUser user)
        {
            string baseUrl = _httpContextAccessor.HttpContext.Request.Host.Value;
            var apiKey = "";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("majorkey548@gmail.com", "Admin");
            var subject = "Confirmação de email";
            var to = new EmailAddress(user.Email, user.FullName);
            var plainTextContent = "Confirmação de email";
            var htmlContent = $"<strong>Olá {user.FullName}</strong>" +
                              $"<p>Por favor verifique seu endereço de email</p>" +
                              $"<a href='https://www.{baseUrl}/api/verifyemail?key={user.Id}'>Confirmar email</a>";
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(message);
        }
    }
}
