using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace ExpenseDiary.Modules
{
    public class Transaction() : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _date;
        private string? _commentary;
        private TransactionType _type;
        private decimal _summ;

        public required int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }


        public required DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public decimal Summ
        {
            get => _summ;
            set
            {
                _summ = value;
                OnPropertyChanged();
            }
        }

        public required TransactionType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public string Commentary
        {
            get => _commentary ?? string.Empty;
            set
            {
                _commentary = value;
                OnPropertyChanged();
            }    
        }

        public enum TransactionType
        {
            Expense,
            Income
        }

        public string RussianType => Type == TransactionType.Expense ? "Расход" : "Доход";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
