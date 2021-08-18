using de_exceptional_closures.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        public InstitutionController(IHttpClientFactory pointerClient)
        {
            _pointerClient = pointerClient;
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

            var result = await client.GetAsync("SearchByName?name=" + name);

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
    }
}