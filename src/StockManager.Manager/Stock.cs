namespace StockManager.Manager
{
    /// <summary>
    /// Stock product.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// Gets or sets the price field.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets the description of a stock.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the product ID.
        /// </summary>
        public int ProductID { get; }

        /// <summary>
        /// Gets or sets the safe stock amount.
        /// </summary>
        public int SafeStockAmount { get; set; }

        /// <summary>
        /// Gets or sets the quantity amount.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the total from sales.
        /// </summary>
        public double TotalFromSales { get; set; }

        /// <summary>
        /// Gets or sets the number of this stock sold.
        /// </summary>
        public int NumberSold { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// Creates a new Stock.
        /// </summary>
        /// <param name="price">Price of the stock.</param>
        /// <param name="description">Description of the stock.</param>
        /// <param name="productID">Product ID of the stock.</param>
        /// <param name="safeStockAmount">Quantity of stock to stay above without needing to reorder.</param>
        /// <param name="quantity">Amount of stock.</param>
        public Stock(double price, string description, int productID, int safeStockAmount, int quantity)
        {
            this.Price = price;
            this.Description = description;
            this.ProductID = productID;
            this.SafeStockAmount = safeStockAmount;
            this.Quantity = quantity;
            this.TotalFromSales = 0;
            this.NumberSold = 0;
        }
    }
}
