using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Parser.Models
{
    public class Domain
    {
        public Domain()
        {

        }
        public Domain(string domainName)
        {
            DomainName = domainName;
        }

        [Key]
        public string DomainName { get; set; }
        public HashSet<Record> Records { get; set; }
    }
}
