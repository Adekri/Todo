using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace todo
{

    public class Task
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public string? Priority { get; set; }
        public DateTime Date { get; set; }
    }
    

}
