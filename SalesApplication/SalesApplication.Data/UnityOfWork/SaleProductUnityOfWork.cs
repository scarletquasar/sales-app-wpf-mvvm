using SalesApplication.Abstractions;
using SalesApplication.Data.Repositories;
using SalesApplication.Database;
using SalesApplication.Domain.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Data.UnityOfWork
{
    public class SaleProductUnityOfWork : IUnitOfWork<Sale, Product>, IDisposable
    {
        private GeneralContext _context;
        private Repository<Sale> saleRepository;
        private Repository<Product> productRepository;

        public SaleProductUnityOfWork(GeneralContext context)
        {
            _context = context;
        }

        public IRepository<Sale> Type1Repository
        {
            get
            {
                if (saleRepository == null)
                {
                    saleRepository = new Repository<Sale>(_context);
                }
                return saleRepository;
            }
        }

        public IRepository<Product> Type2Repository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new Repository<Product>(_context);
                }
                return productRepository;
            }
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
