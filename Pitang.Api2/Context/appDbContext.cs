using Microsoft.EntityFrameworkCore;
using Pitang.Api2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pitang.Api2.Context
{
    public class appDbContext  :DbContext
    {
        public appDbContext(DbContextOptions<appDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Phone> Phones { get; set; }
    }
}
