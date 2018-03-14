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
    public class AuthorsController : ApiController
    {
        //private AlalaWebAPIContext db = new AlalaWebAPIContext();

        List<Author> authors = new List<Author>() {
            new Author() { Id = 1, Name = "Jane Austen" },
            new Author() { Id = 2, Name = "Charles Dickens" },
            new Author() { Id = 3, Name = "Miguel de Cervantes" }
        };

        // GET: api/Authors
        //public IQueryable<Author> GetAuthors()
        public List<Author> GetAuthors()
        {
            return authors;
        }

        // GET: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult GetAuthor(int id)
        {
            //Author author = await db.Authors.FindAsync(id);
            var author = authors.FirstOrDefault((p) => p.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        // PUT: api/Authors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAuthor(int id, Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != author.Id)
            {
                return BadRequest();
            }

            //db.Entry(author).State = EntityState.Modified;

            try
            {
                //await db.SaveChangesAsync();
                authors.Add(new Author
                {
                    Id = author.Id,
                    Name = author.Name
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
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

        // POST: api/Authors
        [ResponseType(typeof(Author))]
        public IHttpActionResult PostAuthor(Author author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //db.Authors.Add(author);
            //await db.SaveChangesAsync();

            authors.Add(new Author
            {
                Id = author.Id,
                Name = author.Name
            });

            return CreatedAtRoute("DefaultApi", new { id = author.Id }, author);
        }

        // DELETE: api/Authors/5
        [ResponseType(typeof(Author))]
        public IHttpActionResult DeleteAuthor(int id)
        {
            //Author author = await db.Authors.FindAsync(id);
            var author = authors.FirstOrDefault((p) => p.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            authors.Remove(author);

            //db.Authors.Remove(author);
            //await db.SaveChangesAsync();

            return Ok(author);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
                authors.Clear();
            }
            base.Dispose(disposing);
        }

        private bool AuthorExists(int id)
        {
            return authors.Count(e => e.Id == id) > 0;
        }
    }
}