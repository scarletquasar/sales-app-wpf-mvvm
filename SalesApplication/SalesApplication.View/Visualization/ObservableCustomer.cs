using SalesApplication.Abstractions;
using SalesApplication.Domain.Business;
using SalesApplication.Domain.Exceptions;
using SalesApplication.Domain.Hardcodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesApplication.View.Visualization
{
    public class ObservableCustomer
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
}
