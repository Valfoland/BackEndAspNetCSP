using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCSP.Models
{
    public class User : IdentityUser
    {
        public List<Child> Childs { get; set; }

        public User()
        {
            Childs = new List<Child>();
        }
    }
}
