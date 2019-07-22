using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitang.Api2.Models
{
    public class UsuarioResponse
    {
        public long Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public List<Phone> phones { get; set; }
        public string accesskey { get; set; }
    }
}
