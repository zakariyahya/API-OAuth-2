using Microsoft.EntityFrameworkCore;
using OAuth2.Models.User;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace OAuth2.Data
{
    public class OAuthContextClass : DbContext
    {
        public OAuthContextClass(DbContextOptions<OAuthContextClass> opt) : base(opt)
        {
        }
        public DbSet<UserModel> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
