namespace StockManager.Manager
{
    /// <summary>
    /// Stock product.
    /// </summary>
    public class Stock
    {
        /// <summary>
        /// The price of the stock.
        /// </summary>
        private double price;

        /// <summary>
        /// The description of the stock.
        /// </summary>
        private string description;

        /// <summary>
        /// Product ID of the stock.
        /// </summary>
        private int productID;

        /// <summary>
        /// Stock amount to not fall under without ordering more.
        /// </summary>
        private int safeStockAmount;

        /// <summary>
        /// Number of stock on hand.
        /// </summary>
        private int quantity;

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
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// Creates a new Stock.
        /// </summary>
        /// <param name="price">Price of the stock.</param>
        /// <param name="description">Description of the stock.</param>
        /// <param name="productID">Product ID of the stock.</param>
        /// <param name="safeStockAmount">Quantity of stock to stay above without needing to reorder.</param>
        /// <param name="amount">Amount of stock.</param>
        public Stock(double price, string description, int productID, int safeStockAmount, int amount)
        {
            this.price = price;
            this.description = description;
            this.productID = productID;
            this.safeStockAmount = safeStockAmount;
            this.quantity = amount;
        }

    }
}
