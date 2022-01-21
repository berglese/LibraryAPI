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
    public class BookExamplesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public BookExamplesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/BookExamples/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookExampleDTO>> GetBookExample(int id)
        {
            var bExample = await _context.BookExamples.FindAsync(id);
            if (bExample == null)
            {
                return NotFound();
            }
            return bkeToDTO(bExample);
        }

        // GET: api/BookExamples/available
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<object>>> GetAvailableBooks()
        {
            var ba = from b in _context.Books
                     join e in _context.BookExamples on b.ID equals e.BookId
                     where e.IsAccess == true && b.Lock == false
                     select new
                     {
                         e.ID,
                         e.BookId,
                         b.Name,
                         b.Author
                     };
            return await ba.ToListAsync();
        }

        // GET: api/BookExamples/given
        [HttpGet("given")]
        public async Task<ActionResult<IEnumerable<object>>> GetGivenBooks()
        {
            var bg = from b in _context.Books
                     join e in _context.BookExamples on b.ID equals e.BookId
                     where e.IsAccess == false
                     select new
                     {
                         e.ID,
                         e.BookId,
                         b.Name,
                         b.Author
                     };
            return await bg.ToListAsync();
        }


        // POST: api/BookExamples
        [HttpPost]
        public async Task<ActionResult<BookExampleDTO>> PostBookExample(BookExampleDTO bookExample)
        {
            var bExample = new BookExample
            {
                ID = bookExample.ID,
                BookId = bookExample.BookId,
                IsAccess = bookExample.IsAccess
            };
            _context.BookExamples.Add(bExample);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookExample), new { id = bExample.ID }, bkeToDTO(bExample));
        }

        // DELETE: api/BookExamples/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookExample(int id)
        {
            var bookExample = await _context.BookExamples.FindAsync(id);
            if (bookExample == null)
            {
                return NotFound();
            }

            _context.BookExamples.Remove(bookExample);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static BookExampleDTO bkeToDTO(BookExample bke) =>
            new BookExampleDTO
            {
                ID = bke.ID,
                BookId = bke.BookId,
                IsAccess = bke.IsAccess
            };
        private bool BookExampleExists(int id)
        {
            return _context.BookExamples.Any(e => e.ID == id);
        }
    }
}
