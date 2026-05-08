using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ExpenseDiary.Modules;
using static ExpenseDiary.Modules.Transaction;

namespace ExpenseDiary
{
    public partial class EditWindow : Window
    {
        private readonly Transaction _originalTransaction;

        public Transaction? ResultTransaction { get; private set; }

        public EditWindow(Transaction transaction)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            _originalTransaction = transaction;
            LoadTransactionData();
        }

        private void LoadTransactionData()
        {
            DatePicker.SelectedDate = _originalTransaction.Date;
            SumTextBox.Text = _originalTransaction.Summ.ToString();

            if (_originalTransaction.Type == TransactionType.Expense)
                ExpenseRadio.IsChecked = true;
            else
                IncomeRadio.IsChecked = true;

            CommentTextBox.Text = _originalTransaction.Commentary ?? "";
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !decimal.TryParse(e.Text, out _);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime date = DatePicker.SelectedDate ?? DateTime.Today;

            if (!decimal.TryParse(SumTextBox.Text, out decimal sum))
            {
                Error.ShowError("Введите корректную сумму (число).");
                return;
            }
            if (sum < 0)
            {
                Error.ShowError("Сумма не может быть отрицательной.");
                return;
            }
            if (sum > 9999999999)
            {
                Error.ShowError("Сумма не может быть выше 9 999 999 999.");
                return;
            }

            TransactionType type = ExpenseRadio.IsChecked == true ? TransactionType.Expense : TransactionType.Income;
            string comment = CommentTextBox.Text;

            ResultTransaction = new Transaction
            {
                Id = _originalTransaction.Id,
                Date = date,
                Summ = sum,
                Type = type,
                Commentary = comment
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}