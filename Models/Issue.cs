using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Issue
    {
        public int ID { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime? Date_end { get; set; }

        [ForeignKey("BookExample")]
        public int BookExampleId { get; set; }
        public virtual BookExample BookExample { get; set; }

        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
        public virtual Reader Reader { get; set; }
    }

    public class IssueDTO
    {
        public int ID { get; set; }
        public DateTime Date_start { get; set; }
        public DateTime? Date_end { get; set; }

        [ForeignKey("BookExample")]
        public int BookExampleId { get; set; }

        [ForeignKey("Reader")]
        public int ReaderId { get; set; }
    }
}
