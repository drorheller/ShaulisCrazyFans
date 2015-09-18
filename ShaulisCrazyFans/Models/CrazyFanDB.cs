using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ShaulisCrazyFans.Models
{
    public class CrazyFanDB : DbContext
    {
        public CrazyFanDB() : base("DefaultConnection") { }

        public DbSet<CrazyFan> CrazyFans { get; set; }

        public System.Data.Entity.DbSet<ShaulisCrazyFans.Models.Post> Posts { get; set; }

        public System.Data.Entity.DbSet<ShaulisCrazyFans.Models.Comment> Comments { get; set; }
    }
}