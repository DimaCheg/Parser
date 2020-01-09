using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class Record
    {
        public Record()
        {
        }

        public Record(string url, string body)
        {
            Url = url;
            Body = body;
        }

        [Key]
        public string Url { get; set; }
        public string Body { get; set; }
        public Domain Domain { get; set; }
    }
}
