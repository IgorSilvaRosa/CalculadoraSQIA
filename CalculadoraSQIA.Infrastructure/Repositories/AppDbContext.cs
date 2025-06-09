using CalculadoraSQIA.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculadoraSQIA.Infrastructure.Repositories
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Cotacao> Cotacao { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cotacao>().ToTable("Cotacao");

        }

    }
}
