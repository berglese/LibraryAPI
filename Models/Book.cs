using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Article { get; set; }
        public uint YearPublic { get; set; }
        public bool Lock { get; set; }
        /*public ICollection<BookExample> BookExamples { get; set; }
        public Book()
        {
            BookExamples = new List<BookExample>();
        }*/
    }

    public class BookDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Article { get; set; }
        public uint YearPublic { get; set; }
    }
}
