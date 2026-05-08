using System.ComponentModel;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace ExpenseDiary.Interfaces
{
    public interface IStatisticsViewModel
    {
        string[]? DatesLabels { get; set; }
        int ExpenseCount { get; set; }
        decimal ExpenseSum { get; set; }
        int IncomeCount { get; set; }
        IEnumerable<ISeries>? IncomeExpensePieSeries { get; set; }
        IEnumerable<ISeries>? IncomeExpenseSeries { get; set; }
        decimal IncomeSum { get; set; }
        decimal TotalSum { get; set; }
        int TransactionCount { get; set; }
        Axis[]? XAxes { get; set; }
        Axis[]? YAxes { get; set; }

        event PropertyChangedEventHandler? PropertyChanged;

        void Recalculate();
    }
}