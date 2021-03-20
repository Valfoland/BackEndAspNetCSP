using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCSP.Models
{
    public class ChildResult
    {
        public int Id { get; set; }
        public string TypeONR { get; set; }
        public string TypeSection { get; set; }
        public string TypeMission { get; set; }
        public int PercentResult { get; set; }
        public Child Child { get; set; }
    }
}
