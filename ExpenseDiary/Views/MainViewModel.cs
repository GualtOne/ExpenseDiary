using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpenseDiary.Interfaces;

namespace ExpenseDiary.Views
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ITransaction TransactionsVM { get; }
        public IStatisticsViewModel StatisticsVM { get; }

        public MainViewModel()
        {
            TransactionsVM = new TransactionsViewModel();
            StatisticsVM = new StatisticsViewModel(TransactionsVM);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}