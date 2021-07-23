using de_exceptional_closures.Config;
using dss_common.Functional;
using Microsoft.Extensions.Options;
using Notify.Client;
using Notify.Models.Responses;
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

        public async Task SendEmail(string emailAddress, string subject, string message)
        {
            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
                {
                    {"subject", subject },
                    { "message", message }
                };

            _notificationClient.SendEmail(emailAddress, _notifyCredentials.Value.emailTemplate, personalisation);
        }

        public async Task<Result<int>> SendText(string mobileNumber, string userName, string message)
        {
            var client = new NotificationClient(_notifyCredentials.Value.apiKey);

            Dictionary<string, dynamic> personalisation = new Dictionary<string, dynamic>
                {
                    {"message",  message}
                };

            SmsNotificationResponse response = client.SendSms(
              mobileNumber: mobileNumber,
              templateId: _notifyCredentials.Value.textTemplate,
              personalisation
              );


            return Result.Ok(1);
        }
    }
}