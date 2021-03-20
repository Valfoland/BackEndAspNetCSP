using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCSP.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }
        public string NameChild { get; set; }
        public int AgeChild { get; set; }
        public string GroupeChild { get; set; }
        public User User { get; set; }

        public List<ChildResult> ChildResults { get; set; }

        public Child()
        {
            ChildResults = new List<ChildResult>();
        }

    }
}
