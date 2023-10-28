namespace GroceryAPI.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetMailAsync(string to, string userId, string resetToken);
        Task SendCompletedOrderMailAsync(string to, string orderNumber, DateTime orderDate, string userFullName);
        Task SendContactUsMail(string type, string nameSurname, string phoneNumber, string message);
    }
}
