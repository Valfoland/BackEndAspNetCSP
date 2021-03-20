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
    public class ChildrenResultController : ControllerBase
    {
        private readonly DataContext _context;
        private UserManager<User> userManager;

        public ChildrenResultController(UserManager<User> userManager, DataContext context)
        {
            _context = context;
            this.userManager = userManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ChildResult>>> GetChildResults(int id)
        {
            var user = await GetUser();
            var child = user.Childs.FirstOrDefault(x => x.Id == id);

            if(child == null)
            { 
                return NotFound();
            }
            return child.ChildResults;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<ChildResult>>> PostChildResults(int id, ChildResult childResult)
        {
            var user = await GetUser();
            var child = user.Childs.FirstOrDefault(x => x.Id == id);

            if (child == null)
            {
                return NotFound();
            }

            childResult.Child = child;
            await _context.ChildResults.AddAsync(childResult);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChildResult", new { id = childResult.Id }, childResult);
        }

        private async Task<User> GetUser()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            return await _context.Users.Include(x => x.Childs).ThenInclude(x => x.ChildResults).FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}
