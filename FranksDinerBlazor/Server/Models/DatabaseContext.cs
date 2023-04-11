using FranksDinerBlazor.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace FranksDinerBlazor.Server.Models
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Order> Orders { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders");
                entity.Property(e => e.Items)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
                entity.Property(e => e.Message)
                    .HasMaxLength(1000)
                    .IsUnicode(false);
            });
            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
