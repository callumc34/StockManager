namespace StockManager.UI
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using StockManager.Manager;

    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private Action<Stock> callback;
        private Stock? selected;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchWindow"/> class.
        /// </summary>
        /// <param name="callback">Callback for selected stock.</param>
        public SearchWindow(Action<Stock> callback)
        {
            this.InitializeComponent();
            this.callback = callback;

            this.CancelButton.Click += (_, _) => this.Close();
            this.OkButton.Click += (_, _) => this.callback(this.selected);
            this.ResultsGrid.SelectionChanged += this.DG_SelectionChanged;
            this.SearchButton.Click += this.Search;

            // Initialise with all stocks
            this.Search(null, null);
        }

        private void SetData(List<Stock> stocks)
        {
            this.ResultsGrid.Items.Clear();

            foreach (Stock stock in stocks)
            {
                this.ResultsGrid.Items.Add(stock);
            }
        }

        private void Search(object sender, EventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)this.TypeSelection.SelectedItem;
            string text = (string)this.SearchTextBox.Text;
            string type = (string)selectedItem.Tag;

            if (text == string.Empty)
            {
                this.SetData(App.GetStockManager().GetAllStocks());
                return;
            }

            if (type == "ProductID")
            {
                if (!int.TryParse(text, out int value))
                {
                    this.SetData(new List<Stock>());
                    return;
                }

                this.SetData(App.GetStockManager().SearchForStocksFromProductID(value));
            }
            else
            {
                this.SetData(App.GetStockManager().SearchForStocksFromDescription(text));
            }
        }

        private void DG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.selected = (Stock)this.ResultsGrid.SelectedItem;
        }
    }
}
