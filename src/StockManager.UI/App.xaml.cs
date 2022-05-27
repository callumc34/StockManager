namespace StockManager.UI
{
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using StockManager.Manager;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// App settings configuration.
        /// </summary>
        public static readonly IConfiguration Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        /// <summary>
        ///  Stock database for the app.
        /// </summary>
        private static StockDatabase stockManager = new StockDatabase(Configuration["uri"]);

        /// <summary>
        /// Gets the stock manager.
        /// </summary>
        /// <returns>The stock manager.</returns>
        public StockDatabase GetStockManager()
        {
            return stockManager;
        }
    }
}
