using de_exceptional_closures.Config;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace de_exceptional_closures.Captcha
{
    public class ReCaptcha
    {
        private readonly HttpClient captchaClient;
        private readonly IHttpClientFactory _notifyClientFactory;
        private readonly IOptions<CaptchaConfig> _captchaConfig;

        public ReCaptcha(HttpClient captchaClient, IHttpClientFactory notifyClientFactory, IOptions<CaptchaConfig> captchaConfig)
        {
            this.captchaClient = captchaClient;
            _notifyClientFactory = notifyClientFactory;
            _captchaConfig = captchaConfig;
        }

        public async Task<bool> IsValid(string captcha)
        {
            var client = _notifyClientFactory.CreateClient("CaptchaClient");

            var postTask = await client
                    .PostAsync($"?secret=&response={captcha}", new StringContent(""));

            var result = await postTask.Content.ReadAsStringAsync();
            var resultObject = JObject.Parse(result);
            dynamic success = resultObject["success"];
            return (bool)success;
        }
    }
}