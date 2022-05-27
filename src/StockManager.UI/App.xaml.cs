namespace StockManager.UI
{
    using System.Windows;
    using Microsoft.Extensions.Configuration;
    using StockManager.Database;

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
        private static DatabaseStockManager stockManager = new DatabaseStockManager(Configuration["uri"]);

        /// <summary>
        /// Gets the stock manager.
        /// </summary>
        /// <returns>The stock manager.</returns>
        public static DatabaseStockManager GetStockManager()
        {
            return stockManager;
        }
    }
}
