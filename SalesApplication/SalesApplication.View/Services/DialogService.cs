using System.Windows.Forms;
using SalesApplication.View.Abstractions;

namespace SalesApplication.View.Services
{
    public class DialogService : IDialogService
    {
        public DialogResult Show(string messageText)
        {
            return MessageBox.Show(messageText);
        }
        public DialogResult Show(string messageText, string caption)
        {
            return MessageBox.Show(messageText, caption);
        }
    }
}
