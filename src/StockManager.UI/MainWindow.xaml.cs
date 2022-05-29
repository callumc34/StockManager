namespace StockManager.UI
{
    using System;
    using System.Windows;
    using StockManager.Manager;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private StockEditor? editorWindow;
        private SellWindow? sellWindow;
        private SearchWindow? searchWindow;
        private StockReport? stockReportWindow;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.AddStock.Click += (_, _) =>
            {
                this.editorWindow?.Close();
                Action<Stock> callback = (stock) =>
                {
                    if (stock == null)
                    {
                        return;
                    }

                    if (stock.Price < 0 || stock.Quantity < 0 || stock.SafeStockAmount < 0)
                    {
                        return;
                    }

                    App.GetStockManager().AddNewStock(stock);
                    this.editorWindow?.Close();
                };

                this.editorWindow = new StockEditor(callback, null);
                this.editorWindow.Show();
            };

            this.EditStock.Click += (_, _) =>
            {
                this.editorWindow?.Close();
                this.searchWindow?.Close();

                Action<Stock> searchCallback = (stock) =>
                {
                    this.searchWindow?.Close();
                    Action<Stock> callback = (edited) =>
                    {
                        if (stock.Price != edited.Price)
                        {
                            App.GetStockManager().EditStockPrice(stock.ProductID, edited.Price);
                        }

                        if (stock.Quantity != edited.Quantity)
                        {
                            App.GetStockManager().EditStockQuantity(stock.ProductID, edited.Quantity);
                        }

                        if (stock.SafeStockAmount != edited.SafeStockAmount)
                        {
                            App.GetStockManager().EditSafeStockAmount(stock.ProductID, edited.SafeStockAmount);
                        }

                        this.editorWindow?.Close();
                    };

                    this.editorWindow = new StockEditor(callback, stock, true);
                    this.editorWindow.Show();
                };

                this.searchWindow = new SearchWindow(searchCallback);
                this.searchWindow.Show();
            };

            this.SellStock.Click += (_, _) =>
            {
                this.searchWindow?.Close();

                Action<Stock> searchCallback = (stock) =>
                {
                    this.searchWindow?.Close();
                    Action<Stock, double, int> callback = (edited, discount, quantity) =>
                    {
                        this.sellWindow?.Close();
                        if (discount < 0.85)
                        {
                            return;
                        }

                        if (quantity < 1 || quantity > stock.Quantity)
                        {
                            return;
                        }

                        App.GetStockManager().SellStock(stock.ProductID, stock.Price * discount, quantity);
                    };

                    this.sellWindow = new SellWindow(callback, stock);
                    this.sellWindow.Show();
                };

                this.searchWindow = new SearchWindow(searchCallback);
                this.searchWindow.Show();
            };

            this.SearchStock.Click += (_, _) =>
            {
                this.searchWindow?.Close();
                this.searchWindow = new SearchWindow((_) => this.searchWindow?.Close());
                this.searchWindow.Show();
            };

            this.StockReport.Click += (_, _) =>
            {
                this.stockReportWindow = new StockReport();
                this.stockReportWindow.SetText(App.GetStockManager().GetStockReport());
                this.stockReportWindow.Show();
            };
        }
    }
}
