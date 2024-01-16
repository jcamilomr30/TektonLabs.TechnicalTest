using TektonLabs.TechnicalTest.Domain;
using TektonLabs.TechnicalTest.Domain.Entities;
using TektonLabs.TechnicalTest.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]
[assembly: System.Runtime.InteropServices.ComVisible(true)]
namespace Renting.ProjectName.Domain.Repository
{
    public class Repository<TId, TEntity> : IRepository<TId, TEntity> where TId : struct where TEntity : EntityBase<TId>
    {
        public Repository(IQueryableUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public IQueryableUnitOfWork UnitOfWork { get; set; }
        public TEntity Add(TEntity entity)
        {
            if (entity != null)
            {
                DbSet<TEntity> item = UnitOfWork.GetSet<TEntity, TId>();
                item.Add(entity);
                UnitOfWork.Commit();
            }

            return entity;
        }

        public void Delete(TEntity entity)
        {
            UnitOfWork.GetSet<TEntity, TId>().Remove(entity);
            UnitOfWork.Commit();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            UnitOfWork.Dispose();
        }

        public TEntity FindById(TId id)
        {
            TEntity item = UnitOfWork.GetSet<TEntity, TId>().Find(id);
            return item;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties,
            bool track)
        {
            return BuildQuery(filter, orderBy, includeProperties, track).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool track = false)
        {
            IQueryable<TEntity> query = (!track) ? UnitOfWork.GetSet<TEntity, TId>().AsNoTracking() : UnitOfWork.GetSet<TEntity, TId>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split
                (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync().ConfigureAwait(false);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                DbSet<TEntity> item = UnitOfWork.GetSet<TEntity, TId>();
                item.Update(entity);
                UnitOfWork.Commit();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (entity != null)
            {
                DbSet<TEntity> book = UnitOfWork.GetSet<TEntity, TId>();
                book.Update(entity);
                await UnitOfWork.CommitAsync().ConfigureAwait(false);
            }
        }

        public TEntity FindByCompositeKey(params object[] parameters)
        {
            return UnitOfWork.GetSet<TEntity, TId>().Find(parameters);
        }

        private IQueryable<TEntity> BuildQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>,
               IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "", bool track = false)
        {
            IQueryable<TEntity> query = (!track) ? UnitOfWork.GetSet<TEntity, TId>().AsNoTracking() : UnitOfWork.GetSet<TEntity, TId>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != default)
                foreach (string includeProperty in includeProperties.Split
                    (new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }

        public IEnumerable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            return UnitOfWork.GetSet<TEntity, TId>().Where(expression).AsEnumerable();
        }

    }
}
