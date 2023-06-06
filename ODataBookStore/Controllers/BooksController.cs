using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ODataBookStore.EDM;

namespace ODataBookStore.Controllers
{
    public class BooksController : ODataController
    {
        private BookStoreContext db;

        public BooksController(BookStoreContext context)
        {
            db = context;
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (context.Books.Count() == 0)
            {
                foreach (var item in DataSource.GetBooks())
                {
                    context.Books.Add(item);
                    context.Presses.Add(item.Press);
                }
                context.SaveChanges();
            }
        }

        [EnableQuery(PageSize = 1)]
        public IActionResult Get()
        {
            return Ok(db.Books);
        }

        [EnableQuery]
        public IActionResult Get(int key, string version)
        {
            return Ok(db.Books.FirstOrDefault(c => c.Id == key));
        }

        [EnableQuery]
        public IActionResult Post([FromBody] Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
            return Created(book);
        }

        [EnableQuery]
        public IActionResult Delete([FromBody] int key)
        {
            Book b = db.Books.FirstOrDefault(c => c.Id == key);
            if (b == null)
            {
                return NotFound();
            };

            db.Books.Remove(b);
            db.SaveChanges();
            return Ok();
        }
    }

}
