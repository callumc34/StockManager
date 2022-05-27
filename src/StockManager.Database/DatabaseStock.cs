namespace StockManager.Database
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using StockManager.Manager;

    /// <summary>
    /// Stock class to work with Mongo database.
    /// </summary>
    public class DatabaseStock : Stock
    {
        /// <summary>
        /// MongoDB object id.
        /// </summary>
        [BsonId]
        public ObjectId _id { get; set; }

        /// <summary>
        /// Gets or sets the description of the stock.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the stock product ID.
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseStock"/> class.
        /// Creates a new Stock.
        /// </summary>
        /// <param name="price">Price of the stock.</param>
        /// <param name="description">Description of the stock.</param>
        /// <param name="productID">Product ID of the stock.</param>
        /// <param name="safeStockAmount">Quantity of stock to stay above without needing to reorder.</param>
        /// <param name="quantity">Amount of stock.</param>
        public DatabaseStock(double price, string description, int productID, int safeStockAmount, int quantity)
            : base(price, description, productID, safeStockAmount, quantity)
        {
            this.Description = description;
            this.ProductID = productID;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseStock"/> class using the base <see cref="Stock"/> class.
        /// </summary>
        /// <param name="stock">The stock to generate from.</param>
        public DatabaseStock(Stock stock)
            : base(stock.Price, stock.Description, stock.ProductID, stock.SafeStockAmount, stock.Quantity)
        {
            this.Description = stock.Description;
            this.ProductID = stock.ProductID;
        }
    }
}
