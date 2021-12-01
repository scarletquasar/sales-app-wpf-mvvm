using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SalesApplication.Data.Responses;

namespace SalesApplication.Data.Repositories
{
    class TestRepository<T> : IRepository<T>
    {
        private readonly List<T> _content = new();
        public async Task<bool> Save()
        {
            return await Task.Run(() => true);
        }
        public async Task<IEnumerable<T>> Search() 
        {
            return await Task.Run(() => _content);
        }
        public async Task<IEnumerable<T>> Search(Expression<Func<T, bool>> @where)
        {
            return await Task.Run(() => _content.Where(@where.Compile()).ToList());
        }
        public async Task<ActionResponse> Add(T entity)
        {
            ActionResponse result = new();
            return await Task.Run(() => {
                _content.Add(entity);
                result.Success = true;
                return result;
            });
        }
    }
}
