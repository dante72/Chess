using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.DB
{
    public class InfoContext : DbContext
    {
        public InfoContext() : base("DbConnectionString")
        {
        }

        public DbSet<Exercise> Exercises { get; set; }
    }
}
