using de_exceptional_closures.Config;
using de_exceptional_closures.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace de_exceptional_closures.Controllers
{
    public class InstitutionController : Controller
    {
        private readonly IHttpClientFactory _pointerClient;
        private readonly IOptions<SchoolsApiConfig> _schoolApiConfig;

        public InstitutionController(IHttpClientFactory pointerClient, IOptions<SchoolsApiConfig> schoolApiConfig)
        {
            _pointerClient = pointerClient;
            _schoolApiConfig = schoolApiConfig;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetInstitutions(string name)
        {
            var client = _pointerClient.CreateClient("InstitutionsClient");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + CreateJwtToken());

            var result = await client.GetAsync("SearchSchoolByName?name=" + name);

            List<Institution> institutions = new List<Institution>();

            if (result.IsSuccessStatusCode)
            {
                using (HttpContent content = result.Content)
                {
                    var resp = content.ReadAsStringAsync();
                    institutions = JsonConvert.DeserializeObject<IEnumerable<Institution>>(resp.Result).ToList();
                }
            }

            return Json(institutions);
        }

        private string CreateJwtToken()
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var iat = Math.Round((DateTime.UtcNow - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                { "iat", iat },
                { "kid", _schoolApiConfig.Value.Kid }
            };

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var jwtToken = encoder.Encode(payload, _schoolApiConfig.Value.Secret);
            return jwtToken;
        }
    }
}