using System.ComponentModel;
using System.Runtime.CompilerServices;
using ExpenseDiary.Interfaces;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using static ExpenseDiary.Modules.Transaction;

namespace ExpenseDiary.Views
{
    public class StatisticsViewModel : INotifyPropertyChanged, IStatisticsViewModel
    {
        private readonly ITransaction _transactionsModule;
        private decimal _totalSum;
        private decimal _incomeSum;
        private decimal _expenseSum;
        private int _transactionCount;
        private int _expenseCount;
        private int _incomeCount;
        private DateTime _startDate;
        private DateTime _endDate;
        private decimal _periodSum;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                    RecalculateDate();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                    RecalculateDate();
                }
            }
        }

        public decimal PeriodSum
        {
            get => _periodSum;
            set { _periodSum = value; OnPropertyChanged(); }
        }


        private IEnumerable<ISeries>? _incomeExpenseSeries;
        public IEnumerable<ISeries>? IncomeExpenseSeries
        {
            get => _incomeExpenseSeries;
            set { _incomeExpenseSeries = value; OnPropertyChanged(); }
        }

        private IEnumerable<ISeries>? _incomeExpensePieSeries;
        public IEnumerable<ISeries>? IncomeExpensePieSeries
        {
            get => _incomeExpensePieSeries;
            set { _incomeExpensePieSeries = value; OnPropertyChanged(); }
        }


        private string[]? _datesLabels;
        public string[]? DatesLabels
        {
            get => _datesLabels;
            set { _datesLabels = value; OnPropertyChanged(); }
        }

        private Axis[]? _xAxes;
        public Axis[]? XAxes
        {
            get => _xAxes;
            set { _xAxes = value; OnPropertyChanged(); }
        }

        private Axis[]? _yAxes;
        public Axis[]? YAxes
        {
            get => _yAxes;
            set { _yAxes = value; OnPropertyChanged(); }
        }

        public decimal TotalSum
        {
            get => _totalSum;
            set { _totalSum = value; OnPropertyChanged(); }
        }

        public decimal IncomeSum
        {
            get => _incomeSum;
            set { _incomeSum = value; OnPropertyChanged(); }
        }

        public decimal ExpenseSum
        {
            get => _expenseSum;
            set { _expenseSum = value; OnPropertyChanged(); }
        }

        public int TransactionCount
        {
            get => _transactionCount;
            set { _transactionCount = value; OnPropertyChanged(); }
        }

        public int ExpenseCount
        {
            get => _expenseCount;
            set { _expenseCount = value; OnPropertyChanged(); }
        }

        public int IncomeCount
        {
            get => _incomeCount;
            set { _incomeCount = value; OnPropertyChanged(); }
        }

        public StatisticsViewModel(ITransaction transactionsModule)
        {
            _transactionsModule = transactionsModule;
            _startDate = DateTime.Today.AddDays(-30);
            _endDate = DateTime.Today;

            _transactionsModule.Transactions.CollectionChanged += (s, e) => Recalculate();
            Recalculate();

            XAxes = [new Axis { Name = "Дата", LabelsRotation = 15 }];
            YAxes = [new Axis { Name = "Сумма", Labeler = value => value.ToString("C0") }];
        }

        public void RecalculateDate()
        {
            var filtered = _transactionsModule.Transactions
                .Where(t => t.Date.Date >= StartDate.Date && t.Date.Date <= EndDate.Date)
                .ToList();

            PeriodSum = filtered.Sum(t => t.Type == TransactionType.Income ? t.Summ : -t.Summ);
        }

        public void Recalculate()
        {
            var transactions = _transactionsModule.Transactions;

            TransactionCount = transactions.Count;
            ExpenseCount = transactions.Count(t => t.Type == TransactionType.Expense);
            IncomeCount = transactions.Count(t => t.Type == TransactionType.Income);

            IncomeSum = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Summ);
            ExpenseSum = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Summ);
            TotalSum = IncomeSum - ExpenseSum;

            var grouped = transactions
                 .GroupBy(t => t.Date.Date)
                 .OrderBy(g => g.Key)
                 .ToList();

            if (grouped.Count != 0)
            {
                DatesLabels = [.. grouped.Select(g => g.Key.ToString("dd.MM"))];
                var incomeData = grouped.Select(g => g.Where(t => t.Type == TransactionType.Income).Sum(t => (double)t.Summ)).ToArray();
                var expenseData = grouped.Select(g => g.Where(t => t.Type == TransactionType.Expense).Sum(t => (double)t.Summ)).ToArray();

                IncomeExpenseSeries =
                [
                    new ColumnSeries<double> { Name = "Доходы", Values = incomeData },
                    new ColumnSeries<double> { Name = "Расходы", Values = expenseData }
                ];

                if (XAxes != null && XAxes.Length > 0)
                    XAxes[0].Labels = DatesLabels;
            }
            else
            {
                IncomeExpenseSeries = [];
                if (XAxes != null && XAxes.Length > 0)
                    XAxes[0].Labels = Array.Empty<string>();
            }

            IncomeExpensePieSeries =
            [
                new PieSeries<double> { Name = "Доходы", Values = [(double)IncomeSum], DataLabelsSize = 12 },
                new PieSeries<double> { Name = "Расходы", Values = [(double)ExpenseSum], DataLabelsSize = 12 }
            ];
        }



        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}