using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Domain
{
    public interface IQueryableUnitOfWork : IDisposable
    {
        DbSet<TEntity> GetSet<TEntity, TId>() where TId : struct where TEntity : Domain.Entities.EntityBase<TId>;
        void Commit();
        Task CommitAsync();
        DbContext GetContext();
    }
}
