using Microsoft.EntityFrameworkCore;
using SalesApplication.Abstractions;
using SalesApplication.Database;
using SalesApplication.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SalesApplication.Domain.Abstractions;

namespace SalesApplication.Data.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        private GeneralContext _context;
        public SaleRepository(GeneralContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Sale>> SearchWithProducts()
        {
            return await _context.Sales.Include(x => x.Products).ToListAsync();
        }
        public async Task<IEnumerable<Sale>> SearchWithProducts(Expression<Func<Sale, bool>> @where)
        {
            return await _context.Sales.Include(x => x.Products).Where(where).ToListAsync();
        }
    }
}
