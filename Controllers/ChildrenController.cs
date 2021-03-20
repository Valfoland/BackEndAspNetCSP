using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppCSP.Models;

namespace WebAppCSP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly DataContext _context;
        private UserManager<User> userManager;

        public ChildrenController(UserManager<User> userManager, DataContext context)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/Children
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Child>>> GetChilds()
        {
            var user = await GetUser();
            return user.Childs;
        }

        // GET: api/Children/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Child>> GetChild(int id)
        {
            var user = await GetUser();
            var child = user.Childs.FirstOrDefault(x => x.Id == id);

            if (child == null)
            {
                return NotFound();
            }

            return child;
        }

        // PUT: api/Children/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChild(int id, Child child)
        {
            var user = await GetUser();

            if (!user.Childs.Any(x => x.Id == id) && id != child.Id)
            {
                return BadRequest();
            }

            child.User = user;
            var local = _context.Set<Child>().Local.FirstOrDefault(entry => entry.Id.Equals(id));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }

            _context.Entry(child).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChildExists(id))
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

        // POST: api/Children
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Child>> PostChild(Child child)
        {
            var user = await GetUser();
            child.User = user;

            await _context.Childs.AddAsync(child);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChild", new { id = child.Id }, child);
        }

        // DELETE: api/Children/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Child>> DeleteChild(int id)
        {
            var user = await GetUser();
            var child = user.Childs.FirstOrDefault(x => x.Id == id);

            if (child == null)
            {
                return NotFound();
            }

            _context.ChildResults.RemoveRange(child.ChildResults);
            _context.Childs.Remove(child);
            await _context.SaveChangesAsync();

            return child;
        }

        private async Task<User> GetUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return await _context.Users.Include(x => x.Childs).ThenInclude(x => x.ChildResults).FirstOrDefaultAsync(x => x.Id == userId);
        }

        private bool ChildExists(int id)
        {
            return _context.Childs.Any(e => e.Id == id);
        }
    }
}
