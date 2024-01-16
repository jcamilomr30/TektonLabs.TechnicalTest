using TektonLabs.TechnicalTest.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TektonLabs.TechnicalTest.Core.Interfaces
{
    public interface IService<TId, TEntity, TEntityDto>
        where TId : struct
        where TEntityDto : Dtos.EntityBase<TId>
        where TEntity : Domain.Entities.EntityBase<TId>
    {
        Task<IEnumerable<TEntityDto>> GetAllAsync(
             Expression<Func<TEntity, bool>> filter = null,
             Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
             string includeProperties = "",
             bool track = false);

        IEnumerable<TEntityDto> GetAll(Expression<Func<TEntity, bool>> filter=null, Func<IQueryable<TEntity>,
                IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = null, bool track = false);

        TEntityDto FindById(TId id);
        Task UpdateAsync(TEntityDto entity);
        TEntityDto Add(TEntityDto entity);
        void Update(TEntityDto entity);
        void Delete(TEntityDto entity);
        void DeleteById(TId id);
        TEntityDto FindByCompositeKey(params object[] parameters);
    }
}
