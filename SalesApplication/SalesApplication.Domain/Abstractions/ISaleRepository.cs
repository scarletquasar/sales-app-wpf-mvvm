using SalesApplication.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Abstractions
{
    public interface ISaleRepository : IRepository<Sale>
    {
        public Task<IEnumerable<Sale>> SearchWithProducts(Expression<Func<Sale, bool>> @where);
    }
}
