using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitang.Api2.Models
{
    public class Phone
    {
        public long Id { get; set; }
        public int number { get; set; }
        public int area_code { get; set; }
        public string country_code { get; set; }    
    }
}
