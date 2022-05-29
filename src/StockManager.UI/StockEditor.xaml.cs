namespace StockManager.UI
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using StockManager.Manager;

    /// <summary>
    /// Interaction logic for StockEditor.xaml
    /// </summary>
    public partial class StockEditor : Window
    {
        private Action<Stock> callback;
        private bool edit;

        private int productID = -1;
        private string description;
        private double price = -1;
        private int quantity = -1;
        private int safeStockAmount = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockEditor"/> class.
        /// </summary>
        /// <param name="callback">Callback on completion.</param>
        /// <param name="stock">Active stock..</param>
        /// <param name="edit">Whether the window is in edit or add mode.</param>
        public StockEditor(Action<Stock> callback, Stock? stock, bool edit = false)
        {
            this.InitializeComponent();
            this.callback = callback;

            this.SetEdit(edit);

            if (stock != null)
            {
                this.SetStock(stock);
            }

            this.OkButton.Click += (_, _) => callback(new Stock(this.productID, this.description, this.price, this.safeStockAmount, this.quantity));
            this.CancelButton.Click += (_, _) => this.Close();
        }

        /// <summary>
        /// Sets the active stock.
        /// </summary>
        /// <param name="stock">Active stock.</param>
        public void SetStock(Stock stock)
        {
            this.ProductID.Text = stock.ProductID.ToString();
            this.productID = stock.ProductID;
            this.Description.Text = stock.Description;
            this.description = stock.Description;
            this.Price.Text = string.Format("{0:0.00}", stock.Price);
            this.price = stock.Price;
            this.Quantity.Text = stock.Quantity.ToString();
            this.quantity = stock.Quantity;
            this.SafeStockAmount.Text = stock.SafeStockAmount.ToString();
            this.safeStockAmount = stock.SafeStockAmount;
        }

        /// <summary>
        /// Sets whether the window is in edit or add mode.
        /// </summary>
        /// <param name="edit">Edit mode enabled or not.</param>
        public void SetEdit(bool edit)
        {
            this.edit = edit;
            if (this.edit)
            {
                this.ProductID.IsReadOnly = true;
                this.Description.IsReadOnly = true;
            } else
            {
                this.ProductID.IsReadOnly = false;
                this.Description.IsReadOnly = false;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void DoubleValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            if (this.Price.Text.Contains(".") && e.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            Regex regex = new Regex("[^0-9\\.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ProductID_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(this.ProductID.Text, out int id))
            {
                this.productID = id;
            }
        }

        private void Description_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.description = this.Description.Text;
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.Price.Text, out double p))
            {
                this.price = p;
            }
        }

        private void Quantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(this.Quantity.Text, out int q))
            {
                this.quantity = q;
            }
        }

        private void SafeStockAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(this.SafeStockAmount.Text, out int s))
            {
                this.safeStockAmount = s;
            }
        }
    }
}
