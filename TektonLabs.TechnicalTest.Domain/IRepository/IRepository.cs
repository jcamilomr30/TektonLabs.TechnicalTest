using TektonLabs.TechnicalTest.Domain;
using TektonLabs.TechnicalTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Domain.IRepository
{
    public interface IRepository<TId, TEntity> : IDisposable where TId : struct where TEntity : EntityBase<TId>
    {
        IQueryableUnitOfWork UnitOfWork { get; }

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool track = false);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties, bool track);

        TEntity FindById(TId id);

        Task UpdateAsync(TEntity entity);

        TEntity Add(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        TEntity FindByCompositeKey(params object[] parameters);
    }
}
