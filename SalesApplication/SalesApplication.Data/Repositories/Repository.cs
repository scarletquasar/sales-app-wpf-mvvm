using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Database;
using SalesApplication.Domain.Business;

namespace SalesApplication.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GeneralContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(GeneralContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<T>> Search()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> @where)
        {
            return await _dbSet.Where(@where).ToListAsync();
        }
        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            await Save();
        }

        public async Task Update(T entity)
        {
            _dbSet.Update(entity);
            await Save();
        }
    }
}
