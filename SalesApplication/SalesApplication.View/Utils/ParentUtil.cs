using System.Windows;
using System.Windows.Media;

namespace SalesApplication.View.Utils
{
    public static class ParentUtil
    {
        public static MainWindow GetWindow(DependencyObject baseObject)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(baseObject);
            while (parent is not MainWindow)
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return (MainWindow)parent;
        }
    }
}
