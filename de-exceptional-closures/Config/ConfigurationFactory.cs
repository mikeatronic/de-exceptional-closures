﻿using Microsoft.Extensions.Configuration;
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

        public static string PopulateLocalConnectionString(IConfiguration configuration)
        {
            var host = configuration["mysql:client:server"];
            var name = configuration["mysql:client:database"];
            var password = configuration["mysql:client:password"];
            var port = configuration["mysql:client:port"];
            var username = configuration["mysql:client:username"];

            return "Host=" + host + ";" + "database=" + name + "; " + "username=" + username + ";" +
                "password=" + password + ";" + "port=" + port + ";";
        }

        public static MySqlCredentials CreateDatabaseConfig(Dictionary<string, Credential> credentials)
        {
            return new MySqlCredentials
            {
                Host = credentials["host"].Value,
                Port = credentials["port"].Value,
                Name = credentials["name"].Value,
                Username = credentials["username"].Value,
                Password = credentials["password"].Value,
            };
        }

        public static string PopulateConnectionString(MySqlCredentials database)
        {
            return "host=" + database.Host + ";" + "database=" + database.Name + ";" + "password=" + database.Password + ";" + "port=" + database.Port + ";" + "username=" + database.Username + ";";
        }
    }
}