using de_exceptional_closures.Config;
using Microsoft.Extensions.Options;
using Notify.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace de_exceptional_closures.Notify
{
    public class NotifyService : INotifyService
    {
        private readonly IOptions<NotifyConfig> _notifyCredentials;
        private readonly NotificationClient _notificationClient;

        public NotifyService(IOptions<NotifyConfig> notifyCredentials)
        {
            _notifyCredentials = notifyCredentials;
            _notificationClient = new NotificationClient(_notifyCredentials.Value.apiKey);
        }

        public void SendEmail(string emailAddress, string subject, string message)
        {
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
                {
                    {"subject", subject },
                    { "message", message }
                };

            _notificationClient.SendEmail(emailAddress, _notifyCredentials.Value.emailTemplate, personalisation);
        }

        public void SendText(string mobileNumber, string message)
        {
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
                {
                    {"message",  message}
                };

            _notificationClient.SendSms(mobileNumber: mobileNumber, templateId: _notifyCredentials.Value.textTemplate, personalisation
              );
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string message)
        {
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
                {
                    {"subject", subject },
                    { "message", message }
                };

            await _notificationClient.SendEmailAsync(emailAddress, _notifyCredentials.Value.emailTemplate, personalisation);
        }
    }
}