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
        private GeneralContext _context;
        public Repository(GeneralContext context)
        {
            _context = context;
        }
        public async Task<bool> Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<T>> Search()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> @where)
        {
            return await _context.Set<T>().Where(@where).ToListAsync();
        }
        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await Save();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
