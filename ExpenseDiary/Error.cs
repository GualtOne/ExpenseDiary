
using System.Windows;

namespace ExpenseDiary
{
    public class Error
    {
        public static void ShowError(string txt)
        {
            MessageBox.Show(txt, "Ошибка" ,MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
