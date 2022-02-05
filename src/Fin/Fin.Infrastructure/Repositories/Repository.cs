using Fin.Domain.Entities;
using Fin.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fin.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly FinDbContext context;

        public Repository(FinDbContext context) => this.context = context;

        public async Task<IEnumerable<T>> GetAllAsync() => await context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetWhereAsync(Expression<Func<T, bool>> filter) => await context.Set<T>().Where(filter).ToListAsync();

        public async Task<T> GetByIdAsync(Guid id) => await context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdAsync(Guid id, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = context.Set<T>();

            foreach (Expression<Func<T, object>> include in includes)
            {
                query = query.Include(include);
            }

            return await query.FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
        }

        public void Add(T entity) => context.Set<T>().Add(entity);

        public void Update(T entity) => context.Set<T>().Update(entity);

        public void Delete(T entity) => context.Set<T>().Remove(entity);
    }
}
