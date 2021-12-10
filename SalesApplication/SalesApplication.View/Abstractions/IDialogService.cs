using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesApplication.View.Abstractions
{
    public interface IDialogService
    {
        DialogResult Show(string messageText);
        DialogResult Show(string messageText, string caption);
    }
}
