using System.Threading.Tasks;

namespace MyWedding.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
