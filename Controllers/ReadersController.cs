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
    public class ReadersController : ControllerBase
    {
        private readonly AppDBContext _context;

        public ReadersController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Readers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReaderDTO>>> GetReaders()
        {
            var actReaders = from r in _context.Readers
                           where r.Lock == false
                           select r;
            return await actReaders
                .Select(b => rdToDTO(b))
                .ToListAsync();
        }

        // GET: api/Readers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<object>>> GetReader(int id)
        {
            var reader = await _context.Readers.FindAsync(id);
            if (reader == null || reader.Lock)
            {
                return NotFound();
            }

            var rr = from r1 in _context.Readers
                     where r1.ID == id
                     select new
                     {
                         r1.ID,
                         r1.FIO,
                         r1.Birthday,
                         IssuesList =
                            (from r2 in _context.Readers
                             join i in _context.Issues on r2.ID equals i.ReaderId
                             join be in _context.BookExamples on i.BookExampleId equals be.ID
                             join b in _context.Books on be.BookId equals b.ID
                             where r2.ID == id && !be.IsAccess
                             select new
                             {
                                 be.ID,
                                 be.BookId,
                                 b.Name,
                                 b.Author
                             }
                            ).ToList()
                     };

            return await rr.ToListAsync();
        }

        // GET: api/Reader/name=substr
        [HttpGet("name={substr}")]
        public async Task<ActionResult<IEnumerable<object>>> GetReader(string substr)
        {
            var actReaders = from r in _context.Readers
                             where r.Lock == false
                             select r;
            if (!string.IsNullOrEmpty(substr))
            {
                actReaders = actReaders.Where(n => n.FIO!.Contains(substr));
            }

            var rr = from r1 in actReaders
                     select new
                     {
                         r1.ID,
                         r1.FIO,
                         r1.Birthday,
                         IssuesList =
                            (from r2 in _context.Readers
                             join i in _context.Issues on r2.ID equals i.ReaderId
                             join be in _context.BookExamples on i.BookExampleId equals be.ID
                             join b in _context.Books on be.BookId equals b.ID
                             where r2.ID == r1.ID && !be.IsAccess
                             select new
                             {
                                 be.ID,
                                 be.BookId,
                                 b.Name,
                                 b.Author
                             }
                            ).ToList()
                     };

            return await rr.ToListAsync();
        }

        // PUT: api/Readers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReader(int id, Reader reader)
        {
            if (id != reader.ID)
            {
                return BadRequest();
            }

            _context.Entry(reader).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReaderExists(id))
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

        // POST: api/Readers
        [HttpPost]
        public async Task<ActionResult<ReaderDTO>> PostReader(ReaderDTO reader)
        {
            var rdr = new Reader
            {
                ID = reader.ID,
                FIO = reader.FIO,
                Birthday = reader.Birthday
            };
            _context.Readers.Add(rdr);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReader), new { id = rdr.ID }, rdToDTO(rdr));
        }

        private static ReaderDTO rdToDTO(Reader rd) =>
            new ReaderDTO
            {
                ID = rd.ID,
                FIO = rd.FIO,
                Birthday = rd.Birthday
            };
        private bool ReaderExists(int id)
        {
            return _context.Readers.Any(e => e.ID == id);
        }
    }
}
