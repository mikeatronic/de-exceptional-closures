using System.Threading.Tasks;

namespace de_exceptional_closures.Notify
{
    public interface INotifyService
    {
        public Task SendEmail(string emailAddress, string subject, string message);
        public Task SendText(string mobileNumber, string userName, string message);
    }
}