namespace StockManager.Manager
{
    /// <summary>
    /// Interface for a stock manager.
    /// </summary>
    public interface IStockManager
    {
        /// <summary>
        /// Add a new stock to the stock manager.
        /// </summary>
        /// <param name="stock">The stock to add.</param>
        /// <returns>Returns the StockManager with updated information.</returns>
        public IStockManager AddNewStock(Stock stock);

        /// <summary>
        /// Adds an extra quantity of stock with given product ID to the stock manager.
        /// </summary>
        /// <param name="productID">The product ID to update.</param>
        /// <param name="quantity">Number of stock to add.</param>
        /// <returns>The StockManager with updated information.</returns>
        public IStockManager AddStock(int productID, int quantity = 1);

        /// <summary>
        /// Remove a stock from the stock manager.
        /// </summary>
        /// <param name="productID">The product ID to remove.</param>
        /// <returns>Returns the StockManager with updated information.</returns>
        public IStockManager RemoveStock(int productID);

        /// <summary>
        /// Remove all stock from the stocks manager.
        /// </summary>
        /// <returns>Returns the Stockmanager with updated information.</returns>
        public IStockManager RemoveAllStock();

        /// <summary>
        /// Sells a number of stock with a given price per stock.
        /// </summary>
        /// <param name="productID">The product ID to sell.</param>
        /// <param name="pricePerStock">Price to sell each stock at.</param>
        /// <param name="quantity">Number of stock to sell.</param>
        /// <returns>The StockManager with updated information.</returns>
        public IStockManager SellStock(int productID, double pricePerStock, int quantity = 1);

        /// <summary>
        /// Edits the price of a given stock.
        /// </summary>
        /// <param name="productID">Product ID to change price of.</param>
        /// <param name="pricePerStock">New price per stock.</param>
        /// <returns>The StockManager with updated information.</returns>
        public IStockManager EditStockPrice(int productID, double pricePerStock);

        /// <summary>
        /// Edits the quantity of a given stock.
        /// </summary>
        /// <param name="productID">Product ID to change quantity of.</param>
        /// <param name="quantity">New quantity.</param>
        /// <returns>The StockManager with updated information.</returns>
        public IStockManager EditStockQuantity(int productID, int quantity);

        /// <summary>
        /// Edits the safe stock amount of a stock.
        /// </summary>
        /// <param name="productID">Product ID to change safe stock amount of.</param>
        /// <param name="safeStockAmount">New safe stock amount.</param>
        /// <returns>The StockManager with updated information.</returns>
        public IStockManager EditSafeStockAmount(int productID, int safeStockAmount);

        /// <summary>
        /// Return all of the stocks in the manager.
        /// </summary>
        /// <returns>List of all stocks.</returns>
        public List<Stock> GetAllStocks();

        /// <summary>
        /// Search for all the stocks that match a description.
        /// </summary>
        /// <param name="description">Description to search for</param>
        /// <returns>List of all stocks matching the description.</returns>
        public List<Stock> SearchForStocksFromDescription(string description);

        /// <summary>
        /// Search for all the stocks that match a product ID.
        /// </summary>
        /// <param name="productID">Product ID to search for.</param>
        /// <returns>List of all stocks matching the product ID.</returns>
        public List<Stock> SearchForStocksFromProductID(int productID);

        /// <summary>
        /// Gets the product ID from a stock description.
        /// </summary>
        /// <param name="description">Description to search for.</param>
        /// <returns>The product id of the stock. Returns -1 if no stock exists.</returns>
        public int GetProductIDFromDescription(string description);

        /// <summary>
        /// Get a stock from the description.
        /// </summary>
        /// <param name="description">Description to search for.</param>
        /// <returns>Returns the stock if exists otherwise null.</returns>
        public Stock? GetStockFromDescription(string description);

        /// <summary>
        /// Gets the stock from the product ID.
        /// </summary>
        /// <param name="productID">Product ID to search for.</param>
        /// <returns>Returns the stock if exists otherwise null.</returns>
        public Stock? GetStockFromProductID(int productID);

        /// <summary>
        /// Gets the number of a given stock sold.
        /// </summary>
        /// <param name="productID">Product ID to search for.</param>
        /// <returns>Number of stock sold.</returns>
        public int GetNumberSold(int productID);

        /// <summary>
        /// Gets the total money made from selling a stock.
        /// </summary>
        /// <param name="productID">Product ID to search for.</param>
        /// <returns>Total money made from sales.</returns>
        public double GetTotalMoneyFromStock(int productID);

        /// <summary>
        /// Produces a stock report.
        /// </summary>
        /// <returns>A stock report of all the stocks.</returns>
        public string GetStockReport();

        /// <summary>
        /// Produce a report ordering more stock.
        /// </summary>
        /// <param name="productID">Product ID to order for.</param>
        /// <param name="description">The stock description.</param>
        /// <param name="quantity">The amount to order.</param>
        public void ProduceStockOrder(int productID, string description, int quantity);
    }
}
