using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.Abstractions
{
    public interface IUnitOfWork<T1, T2>
    {
        public IRepository<T1> Type1Repository { get; }
        public IRepository<T2> Type2Repository { get; }

        Task Commit();
        Task Rollback();
        Task BeginTransaction();
    }
}
