using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryAPI.Models;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BooksController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDTO>>> GetBooks()
        {
            var actBooks = from b in _context.Books
                           where b.Lock == false
                           select b;
            return await actBooks
                .Select(b=> bkToDTO(b))
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null || book.Lock)
            {
                return NotFound();
            }

            var bb = from b1 in _context.Books
                     where b1.ID == id
                     select new
                     {
                         b1.ID,
                         b1.Name,
                         b1.Author,
                         b1.Article,
                         b1.YearPublic,
                         ExamplesList =
                            (from b2 in _context.Books
                             join ex in _context.BookExamples on b2.ID equals ex.BookId
                             where b2.ID == id
                             select new
                             {
                                 ex.ID,
                                 ex.BookId,
                                 ex.IsAccess
                             }
                         ).ToList()
                     };

            return await bb.ToListAsync();
        }

        // GET: api/Books/name=substr
        [HttpGet("name={substr}")]
        public async Task<ActionResult<IEnumerable<object>>> GetBook(string substr)
        {
            var actBooks = from b in _context.Books
                where b.Lock == false
                select b;
            if (!string.IsNullOrEmpty(substr))
            {
                actBooks = actBooks.Where(n => n.Name!.Contains(substr));
            }
            var bb = from b1 in actBooks
                     select new
                     {
                         b1.ID,
                         b1.Name,
                         b1.Author,
                         b1.Article,
                         b1.YearPublic,
                         ExamplesList =
                            (from b2 in actBooks
                             join ex in _context.BookExamples on b2.ID equals ex.BookId
                             where b2.ID == b1.ID
                             select new
                             {
                                 ex.ID,
                                 ex.BookId,
                                 ex.IsAccess
                             }
                         ).ToList()
                     };

            return await bb.ToListAsync();
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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

            return NoContent();
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<BookDTO>> PostBook(BookDTO book)
        {
            var bk = new Book
            {
                ID = book.ID,
                Name = book.Name,
                Author = book.Author,
                Article = book.Article,
                YearPublic = book.YearPublic
            };
            _context.Books.Add(bk);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBook), new { id = bk.ID }, bkToDTO(bk));
        }

        private static BookDTO bkToDTO(Book bk) =>
            new BookDTO
            {
                ID = bk.ID,
                Name = bk.Name,
                Author = bk.Author,
                Article = bk.Article,
                YearPublic = bk.YearPublic               
            };

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.ID == id);
        }
    }
}
