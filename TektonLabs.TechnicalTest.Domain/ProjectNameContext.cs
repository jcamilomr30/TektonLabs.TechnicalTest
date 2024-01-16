using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TektonLabs.TechnicalTest.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Domain
{
    public class ProjectNameContext : DbContext, IQueryableUnitOfWork
    {
        private readonly IConfiguration Config;
        public ProjectNameContext(DbContextOptions<ProjectNameContext> options, IConfiguration config) : base(options)
        {
            Config = config;
        }

        public DbSet<TEntity> GetSet<TEntity, TId>() where TId : struct where TEntity : Domain.Entities.EntityBase<TId>
        {
            return Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                return;
            }
            modelBuilder.Entity<Product>().ToTable("Products");
            base.OnModelCreating(modelBuilder);
        }

        public void Commit()
        {
            try
            {
                SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
            }
        }

        public async Task CommitAsync()
        {
            try
            {
                await SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await ex.Entries.Single().ReloadAsync().ConfigureAwait(false);
            }
        }

        public DbContext GetContext()
        {
            return this;
        }

    }
}
