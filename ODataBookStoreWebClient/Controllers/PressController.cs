using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ODataBookStore.EDM;
using System.Net.Http.Headers;

namespace ODataBookStoreWebClient.Controllers
{
    public class PressController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        public PressController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "http://localhost:5054/odata/Presses";
        }
        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var lst = temp.value;
            List<Press> items = ((JArray)temp.value).Select(b => new Press
            {
                Id = (int)b["Id"],
                Name = (string)b["Name"]
            }).ToList();
            return View(items);
        }
    }
}
