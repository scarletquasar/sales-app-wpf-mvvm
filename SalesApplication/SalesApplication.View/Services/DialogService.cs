using System.Windows.Forms;
using SalesApplication.View.Abstractions;

namespace SalesApplication.View.Services
{
    public class DialogService : IDialogService
    {
        public DialogResult Show(string messageText)
        {
            MessageBox.Show(messageText);
            return DialogResult.OK;
        }
        public DialogResult Show(string messageText, string caption)
        {
            MessageBox.Show(messageText, caption);
            return DialogResult.OK;
        }
    }
}
