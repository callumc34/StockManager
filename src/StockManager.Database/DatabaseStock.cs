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
        /// Gets or sets the stock's description for use with MongoDB.
        /// </summary>
        [BsonElement]
        public new string Description { get { return this.description; } set { this.description = value; } }

        /// <summary>
        /// Gets or sets the product ID for use with MongoDB.
        /// </summary>
        [BsonElement]
        public new int ProductID { get { return this.productID; } set { this.productID = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseStock"/> class.
        /// Creates a new Stock.
        /// </summary>
        /// <param name="price">Price of the stock.</param>
        /// <param name="description">Description of the stock.</param>
        /// <param name="productID">Product ID of the stock.</param>
        /// <param name="safeStockAmount">Quantity of stock to stay above without needing to reorder.</param>
        /// <param name="quantity">Amount of stock.</param>
        [BsonConstructor]
        public DatabaseStock(double price, string description, int productID, int safeStockAmount, int quantity)
            : base(productID, description, price, safeStockAmount, quantity)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseStock"/> class using the base <see cref="Stock"/> class.
        /// </summary>
        /// <param name="stock">The stock to generate from.</param>
        public DatabaseStock(Stock stock)
            : base(stock.ProductID, stock.Description, stock.Price, stock.SafeStockAmount, stock.Quantity)
        {
        }
    }
}
