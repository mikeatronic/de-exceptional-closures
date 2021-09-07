using System.Threading.Tasks;

namespace de_exceptional_closures.Notify
{
    public interface INotifyService
    {
        public void SendEmail(string emailAddress, string subject, string message);
        public void SendText(string mobileNumber, string message);
        public Task SendEmailAsync(string emailAddress, string subject, string message);
    }
}