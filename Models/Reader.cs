using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Reader
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public DateTime Birthday { get; set; }
        public bool Lock { get; set; }
    }

    public class ReaderDTO
    {
        public int ID { get; set; }
        public string FIO { get; set; }
        public DateTime Birthday { get; set; }
    }
}
