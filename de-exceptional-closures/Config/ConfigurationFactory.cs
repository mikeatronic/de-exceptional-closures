using Steeltoe.Extensions.Configuration.CloudFoundry;
using System.Collections.Generic;

namespace de_exceptional_closures.Config
{
    public static class ConfigurationFactory
    {
        public static NotifyConfig CreateNotifyConfig(Dictionary<string, Credential> credentials)
        {
            return new NotifyConfig
            {
                apiKey = credentials["apiKey"].Value,
                textTemplate = credentials["textTemplate"].Value,
                emailTemplate = credentials["emailTemplate"].Value
            };
        }

        public static void PopulateNotifyConfig(this NotifyConfig notifyConfig, NotifyConfig sourceNotifyConfig)
        {
            notifyConfig.apiKey = sourceNotifyConfig.apiKey;
            notifyConfig.textTemplate = sourceNotifyConfig.textTemplate;
            notifyConfig.emailTemplate = sourceNotifyConfig.emailTemplate;
        }

        public static CaptchaConfig CreateCaptchaConfig(Dictionary<string, Credential> credentials)
        {
            return new CaptchaConfig
            {
                googleUrl = credentials["googleUrl"].Value,
                SiteKey = credentials["siteKey"].Value,
                Secret = credentials["secret"].Value
            };
        }

        public static void PopulateCaptchaConfig(this CaptchaConfig captchaConfig, CaptchaConfig sourceCaptchaConfig)
        {
            captchaConfig.googleUrl = sourceCaptchaConfig.googleUrl;
            captchaConfig.SiteKey = sourceCaptchaConfig.SiteKey;
            captchaConfig.Secret = sourceCaptchaConfig.Secret;
        }
    }
}