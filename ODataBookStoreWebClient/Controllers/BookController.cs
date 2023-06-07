using Microsoft.AspNetCore.Mvc;
using ODataBookStore.EDM;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;


namespace ODataBookStoreWebClient.Controllers
{
    public class BookController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string PressApiUrl = "";

        public BookController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "http://localhost:5054/odata/Books";
            PressApiUrl = "http://localhost:5054/odata/Presses";
        }


        public async Task<IActionResult> Index()
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var lst = temp.value;
            List<Book> items = ((JArray)temp.value).Select(b => new Book
            {
                Id = (int)b["Id"],
                Author = (string)b["Author"],
                ISBN = (string)b["ISBN"],
                Title = (string)b["Title"],
                Price = (decimal)b["Price"]
            }).ToList();
            return View(items);
        }

        public async Task<IActionResult> Detail(int id)
        {
            string urlDetail = $"{ProductApiUrl}?$filter= Id eq {id}";
            //https://localhost:7111/odata/Books?$filter= Id eq 1
            HttpResponseMessage response = await client.GetAsync(urlDetail);
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var data = temp.value;
            Book book = ((JArray)data).Select(b => new Book
            {
                Id = (int)b["Id"],
                Author = (string)b["Author"],
                ISBN = (string)b["ISBN"],
                Title = (string)b["Title"],
                Price = (decimal)b["Price"],
                Location = new Address
                {
                    City = (string)b["Location"]["City"],
                    Street = (string)b["Location"]["Street"]
                }
            }).FirstOrDefault();
            return View(book);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (Book book)
        {
            if (!ModelState.IsValid)
            {
                return NoContent();
            }
            string urlSortByIdAsc = $"{ProductApiUrl}?$orderby= Id asc";
            //https://localhost:7111/odata/Books?$orderby=%20Id%20asc
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var lst = temp.value;
            Book bookLast = ((JArray)temp.value).Select(b => new Book
            {
                Id = (int)b["Id"],
                Author = (string)b["Author"],
                ISBN = (string)b["ISBN"],
                Title = (string)b["Title"],
                Price = (decimal)b["Price"]
            }).Last();
            book.Id = bookLast.Id + 1;
            HttpResponseMessage responseAdd = await client.PostAsJsonAsync(ProductApiUrl, book);
            responseAdd.EnsureSuccessStatusCode();
            return Redirect("/Book");
        }

        public async Task<IActionResult> Delete(int id)
        {
            string urlDelete = $"{ProductApiUrl}/{id}";
            HttpResponseMessage response = await client.DeleteAsync(urlDelete);
            return Redirect("/Book");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            string urlDetail = $"{ProductApiUrl}?$filter= Id eq {id}";
            //https://localhost:7111/odata/Books?$filter= Id eq 1
            HttpResponseMessage response = await client.GetAsync(urlDetail);
            string strData = await response.Content.ReadAsStringAsync();
            dynamic temp = JObject.Parse(strData);
            var data = temp.value;
            Book book = ((JArray)data).Select(b => new Book
            {
                Id = (int)b["Id"],
                Author = (string)b["Author"],
                ISBN = (string)b["ISBN"],
                Title = (string)b["Title"],
                Price = (decimal)b["Price"],
                Location = new Address
                {
                    City = (string)b["Location"]["City"],
                    Street = (string)b["Location"]["Street"]
                }
            }).FirstOrDefault();
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book book)
        {
            if(!ModelState.IsValid)
            {
                return NoContent();
            }
            string urlDelete = $"{ProductApiUrl}/{book.Id}";
            HttpResponseMessage response = await client.PutAsJsonAsync(urlDelete, book);
            response.EnsureSuccessStatusCode();
            return Redirect("/Book");
        }
    }
}
