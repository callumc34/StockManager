namespace StockManager.UI
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using StockManager.Manager;

    /// <summary>
    /// Interaction logic for SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        private Action<Stock, double, int> callback;
        private Stock stock;
        private double discount = 1;
        private int quantity = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="SellWindow"/> class.
        /// </summary>
        /// <param name="callback">Sell callback.</param>
        /// <param name="stock">Stock to sell.</param>
        public SellWindow(Action<Stock, double, int> callback, Stock stock)
        {
            this.InitializeComponent();
            this.callback = callback;
            this.stock = stock;

            this.CancelButton.Click += (_, _) => this.Close();
            this.SellButton.Click += (_, _) => callback(stock, this.discount, this.quantity);

            this.QuantityButton.Click += (_, _) => this.UpdateTotal();

            this.DiscountSelection.SelectionChanged += (_, e) =>
            {
                ComboBoxItem selected = (ComboBoxItem)this.DiscountSelection.SelectedItem;
                if (selected != null)
                {
                    if (double.TryParse(selected.Tag.ToString(), out double reduction))
                    {
                        this.discount = 1 - (reduction / 100);
                    }

                    this.UpdateTotal();
                }
            };
        }

        private void UpdateTotal()
        {
            int.TryParse(this.QuantityText.Text, out this.quantity);

            if (this.quantity < 1)
            {
                this.QuantityText.Text = "1";
                this.quantity = 1;
            } else if (this.quantity > this.stock.Quantity)
            {
                this.QuantityText.Text = this.stock.Quantity.ToString();
                this.quantity = this.stock.Quantity;
            }

            this.TotalText.Text = string.Format("{0:0.00}", this.discount * this.quantity);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
