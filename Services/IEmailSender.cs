using System.Threading.Tasks;

namespace MyWedding.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
