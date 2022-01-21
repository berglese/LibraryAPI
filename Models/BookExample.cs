using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class BookExample
    {
        public int ID { get; set; }
        public bool IsAccess { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }
    }

    public class BookExampleDTO
    {
        public int ID { get; set; }
        public bool IsAccess { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
    }
}
