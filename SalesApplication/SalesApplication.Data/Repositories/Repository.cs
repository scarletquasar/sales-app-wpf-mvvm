using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Data.Responses;
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
            return await Task.Run(() => true);
        }
        public async Task<IEnumerable<T>> Search()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> @where)
        {
            return await Task.Run(() => _content.Where(@where.Compile()).ToList());
        }
        public async Task<IActionResponse> Add(T entity)
        {
            IActionResponse result = new ActionResponse();
            return await Task.Run(async () => {
                try
                {
                    _content.Add(entity);
                    await Save();
                    result.Result = (await Search()).LastOrDefault();
                    result.Success = true;
                }
                catch
                {
                    result.Result = null;
                    result.Success = false;
                }
                return result;
            });
        }
    }
}
