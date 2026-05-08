using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpenseDiary.DBservice;
using ExpenseDiary.Interfaces;
using ExpenseDiary.Modules;

namespace ExpenseDiary.Views
{
    public class TransactionsViewModel : INotifyPropertyChanged, ITransaction
    {
        private ObservableCollection<Transaction>? _transactions;

        public ObservableCollection<Transaction> Transactions
        {
            get => _transactions ?? Transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged();
            }
        }

        private Transaction? _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get => _selectedTransaction ?? SelectedTransaction;
            set
            {
                _selectedTransaction = value;
                OnPropertyChanged();
            }
        }

        public TransactionsViewModel()
        {
            DBService.Connection();
            LoadTransactions();
        }

        public void AddTransaction(Transaction transaction)
        {
            if (transaction == null) return;
            DBService.Add(transaction.Date, transaction.Commentary, transaction.Type, transaction.Summ);
            if (Transactions.Count > 0)
            {
                transaction.Id = Transactions.Last().Id + 1;
            }
            else
            {
                transaction.Id = 1;
            }
            Transactions.Add(transaction);
            
        }

        public void UpdateTransaction(Transaction transaction)
        {
            if (transaction == null) return;
            DBService.Update(transaction.Id, transaction.Date, transaction.Commentary, transaction.Type, transaction.Summ);
            var existing = Transactions.FirstOrDefault(t => t.Id == transaction.Id);
            if (existing != null)
            {
                var index = Transactions.IndexOf(existing);
                Transactions[index] = transaction;
            }
        }

        public void DeleteTransaction(Transaction transaction)
        {
            if (transaction == null) return;
            DBService.Delete(transaction.Id);
            Transactions.Remove(transaction);

        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void LoadTransactions()
        {
            IEnumerable<Transaction> data = DBService.GetAll();
            if (data != null)
            {
                Transactions = new ObservableCollection<Transaction>(data);
            }    

        }

        void ITransaction.LoadTransactions()
        {
            LoadTransactions();
        }

        int ITransaction.GetHashCode()
        {
            return base.GetHashCode();
        }

        string ITransaction.ToString()
        {
            return base.ToString() ?? nameof(TransactionsViewModel);
        }
    }
}
