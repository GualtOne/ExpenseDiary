using System.Collections.ObjectModel;
using ExpenseDiary.Modules;

namespace ExpenseDiary.Interfaces
{
    public interface ITransaction
    {
        ObservableCollection<Transaction> Transactions { get; }
        Transaction SelectedTransaction { get; set; }

        void LoadTransactions();
        void AddTransaction(Transaction transaction);
        void UpdateTransaction(Transaction transaction);
        void DeleteTransaction(Transaction transaction);
        int GetHashCode();
        string ToString();
    }
}