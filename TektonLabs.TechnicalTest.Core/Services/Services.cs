using AutoMapper;
using Serilog;
using TektonLabs.TechnicalTest.Core.Interfaces;
using TektonLabs.TechnicalTest.Domain.Entities;
using TektonLabs.TechnicalTest.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

[assembly: CLSCompliant(false)]
[assembly: System.Runtime.InteropServices.ComVisible(true)]
namespace TektonLabs.TechnicalTest.Core.Services
{
    public class Service<TId, TEntity, TEntityDto> : IService<TId, TEntity, TEntityDto>
        where TId : struct
        where TEntityDto : Dtos.EntityBase<TId>
        where TEntity : EntityBase<TId>
    {
        protected readonly IRepository<TId, TEntity> Repository;
        protected readonly IMapper Mapper;

        public Service(IRepository<TId, TEntity> Repository, IMapper Mapper)
        {
            this.Repository = Repository;
            this.Mapper = Mapper;
        }

        public virtual TEntityDto Add(TEntityDto entity)
        {
            TEntity newEntity = Repository.Add(Mapper.Map<TEntity>(entity));
            return newEntity != null ? Mapper.Map<TEntityDto>(newEntity) : null;
        }
        public void Delete(TEntityDto entity)
        {
            Repository.Delete(Mapper.Map<TEntity>(entity));
        }

        public void DeleteById(TId id)
        {
            Repository.Delete(Repository.FindById(id));
        }

        public TEntityDto FindById(TId id)
        {
            var result = Repository.FindById(id);
            return Mapper.Map<TEntityDto>(result);
        }

        public IEnumerable<TEntityDto> GetAll(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties,
            bool track)
        {
            try
            {
                var result = from item in Repository.GetAll(filter, orderBy, includeProperties, track)
                             select Mapper.Map<TEntityDto>(item);
                return (result.Any()) ? result : null;
            }
            catch (FormatException ex)
            {
                throw;
            }
            catch (ArgumentNullException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntityDto>> GetAllAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            bool track = false)
        {
            try
            {
                return Mapper.Map<IEnumerable<TEntityDto>>(await Repository.GetAllAsync(filter, orderBy, includeProperties, track).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual void Update(TEntityDto entity)
        {
            Repository.Update(Mapper.Map<TEntity>(entity));
        }

        public async Task UpdateAsync(TEntityDto entity)
        {
            await Repository.UpdateAsync(Mapper.Map<TEntity>(entity)).ConfigureAwait(false);
        }

        public virtual TEntityDto FindByCompositeKey(params object[] parameters)
        {
            TEntity book = Repository.FindByCompositeKey(parameters);
            return book != null ? Mapper.Map<TEntityDto>(book) : null;
        }
    }

}
