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
    public class IssuesController : ControllerBase
    {
        private readonly AppDBContext _context;

        public IssuesController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IssueDTO>>> GetIssues()
        {
            return await _context.Issues
                .Select(i => isuToDTO(i))
                .ToListAsync();
        }

        // GET: api/Issues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IssueDTO>> GetIssue(int id)
        {
            var isu = await _context.Issues.FindAsync(id);
            if (isu == null)
            {
                return NotFound();
            }
            return isuToDTO(isu);
        }

        // PUT: api/Issues/bookIn/5
        [HttpPut("bookIn/{id}")]
        public async Task<IActionResult> PutIssue(int id, Issue issue)
        {
            if (id != issue.ID)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var query = from ex in _context.BookExamples
                        where ex.ID == issue.BookExampleId
                        select ex;
            foreach (BookExample bExample in query)
            {
                bExample.IsAccess = true;
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Issues/bookOut
        [HttpPost("bookOut")]
        public async Task<ActionResult<IssueDTO>> PostBookOut(IssueDTO issue)
        {
            var isu = new Issue
            {
                ID = issue.ID,
                Date_start = issue.Date_start,
                Date_end = issue.Date_end,
                BookExampleId = issue.BookExampleId,
                ReaderId = issue.ReaderId
            };
            _context.Issues.Add(isu);
            await _context.SaveChangesAsync();

            var query = from ex in _context.BookExamples
                        where ex.ID == isu.BookExampleId
                        select ex;
            foreach (BookExample bExample in query)
            {
                bExample.IsAccess = false;
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIssue), new { id = isu.ID }, isuToDTO(isu));
        }
        

        private static IssueDTO isuToDTO(Issue isu) =>
            new IssueDTO
            {
                ID = isu.ID,
                Date_start = isu.Date_start,
                Date_end = isu.Date_end,
                BookExampleId = isu.BookExampleId,
                ReaderId = isu.ReaderId
            };

        private bool IssueExists(int id)
        {
            return _context.Issues.Any(e => e.ID == id);
        }
    }
}
