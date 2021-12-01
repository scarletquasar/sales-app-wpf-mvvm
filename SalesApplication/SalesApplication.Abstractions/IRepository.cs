using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;

namespace SalesApplication.Abstractions
{
    public interface IRepository<T>
    {
        public Task<bool> Save();
        public Task<IEnumerable<T>> Search();
        public Task<IEnumerable<T>> Search(Expression<Func<T, bool>> @where);
        public Task<IActionResponse> Add(T entity);
    }
}
