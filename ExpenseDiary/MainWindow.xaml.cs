using System.Windows;
using ExpenseDiary.Interfaces;
using ExpenseDiary.Views;
using Transaction = ExpenseDiary.Modules.Transaction;

namespace ExpenseDiary
{
    public partial class MainWindow : Window
    {
        private MainViewModel MainVM => (MainViewModel)DataContext;
        private ITransaction TransactionsVM => MainVM.TransactionsVM;
        public void AddItem_Click(object sender, RoutedEventArgs e)
        {
            AddWindow dialog = new()
            {
                Owner = this
            };
            if (dialog.ShowDialog() == true) 
            {
                var newtransaction = dialog.ResultTransaction;
                if (newtransaction != null)
                {
                    TransactionsVM.AddTransaction(newtransaction);
                }
            }
        }

        public void EditItem_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid.SelectedItem is not Transaction selected) return;

            EditWindow dialog = new(selected)
            {
                Owner = this
            };
            if (dialog.ShowDialog() == true)
            {
                var newtransaction = dialog.ResultTransaction;
                if (newtransaction != null)
                    TransactionsVM.UpdateTransaction(newtransaction);
            }
        }

        public void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            if (MyDataGrid.SelectedItem is Transaction selected)
            {
                TransactionsVM.DeleteTransaction(selected);
            }
        }


        public MainWindow()
        {
            DataContext = new MainViewModel();
            InitializeComponent();
        }
    }

}