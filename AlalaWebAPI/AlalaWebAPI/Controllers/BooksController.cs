using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AlalaWebAPI.Models;

namespace AlalaWebAPI.Controllers
{
    public class BooksController : ApiController
    {
        //private AlalaWebAPIContext db = new AlalaWebAPIContext();
        List<Book> books = new List<Book>() {
            new Book() { Id = 1, Title = "Pride and Prejudice", Year = 1813, AuthorId = 1,
                Price = 9.99M, Genre = "Comedy of manners" },
            new Book() { Id = 2, Title = "Northanger Abbey", Year = 1817, AuthorId = 1,
                Price = 12.95M, Genre = "Gothic parody" },
            new Book() { Id = 3, Title = "David Copperfield", Year = 1850, AuthorId = 2,
                Price = 15, Genre = "Bildungsroman" },
            new Book() { Id = 4, Title = "Don Quixote", Year = 1617, AuthorId = 3,
                Price = 8.95M, Genre = "Picaresque" }
        };

        // GET: api/Books
        public List<Book> GetBooks()
        {
            //return db.Books;
            return books;
        }

        // GET: api/Books/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult GetBook(int id)
        {
            //Book book = await db.Books.FindAsync(id);
            var book = books.FirstOrDefault((p) => p.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        // PUT: api/Books/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBook(int id, Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != book.Id)
            {
                return BadRequest();
            }

            //db.Entry(book).State = EntityState.Modified;

            try
            {
                //await db.SaveChangesAsync();
                books.Add(new Book
                {
                    Id = book.Id,
                    Title = book.Title,
                    Year = book.Year,
                    AuthorId = book.AuthorId,
                    Price = book.Price,
                    Genre = book.Genre                    
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Books
        [ResponseType(typeof(Book))]
        public IHttpActionResult PostBook(Book book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            books.Add(new Book
            {
                Id = book.Id,
                Title = book.Title,
                Year = book.Year,
                AuthorId = book.AuthorId,
                Price = book.Price,
                Genre = book.Genre
            });

            //db.Books.Add(book);
            //await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [ResponseType(typeof(Book))]
        public IHttpActionResult DeleteBook(int id)
        {
            //Book book = await db.Books.FindAsync(id);
            var book = books.FirstOrDefault((p) => p.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            books.Remove(book);

            //db.Books.Remove(book);
            //await db.SaveChangesAsync();

            return Ok(book);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                books.Clear();
            }
            base.Dispose(disposing);
        }

        private bool BookExists(int id)
        {
            return books.Count(e => e.Id == id) > 0;
        }
    }
}