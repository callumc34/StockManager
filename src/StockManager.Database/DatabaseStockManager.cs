namespace StockManager.Database
{
    using System.Text;
    using MongoDB.Driver;
    using StockManager.Manager;

    /// <summary>
    /// Implementation of <see cref="IStockManager"/> using a Mongo database.
    /// </summary>
    public class DatabaseStockManager : IStockManager
    {
        private MongoManager database;


        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseStockManager"/> class.
        /// </summary>
        /// <param name="uri">Mongo database URI.</param>
        public DatabaseStockManager(string uri)
        {
            this.database = new MongoManager(uri);
        }


        /// <inheritdoc />
        public IStockManager AddNewStock(Stock stock)
        {
            if (this.database.StockExists(stock.ProductID))
            {
                return this;
            }

            this.database.GetCollection().InsertOne(new DatabaseStock(stock));
            return this;
        }

        /// <inheritdoc />
        public IStockManager AddStock(int productID, int quantity)
        {
            if (!this.database.StockExists(productID))
            {
                return this;
            }

            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            var update = Builders<DatabaseStock>.Update.Set("Quantity", this.GetStockFromProductID(productID).Quantity);
            this.database.GetCollection().UpdateOne(filter, update);
            return this;
        }

        /// <inheritdoc />
        public int GetNumberSold(int productID)
        {
            if (!this.database.StockExists(productID))
            {
                return -1;
            }

            return this.GetStockFromProductID(productID).NumberSold;
        }

        /// <inheritdoc />
        public List<Stock> GetAllStocks()
        {
            IEnumerable<Stock> stocks = this.database.GetCollection().Find(_ => true).ToList();
            return stocks.ToList();
        }

        /// <inheritdoc />
        public int GetProductIDFromDescription(string description)
        {
            var filter = Builders<DatabaseStock>.Filter.Eq("Description", description);
            return this.database.GetCollection().Find(filter).First().ProductID;
        }

        /// <inheritdoc />
        public Stock GetStockFromDescription(string description)
        {
            return this.GetStockFromProductID(this.GetProductIDFromDescription(description));
        }

        /// <inheritdoc />
        public Stock GetStockFromProductID(int productID)
        {
            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            return this.database.GetCollection().Find(filter).First();
        }

        /// <inheritdoc />
        public string GetStockReport()
        {
            StringBuilder report = new StringBuilder();
            foreach (Stock stock in this.GetAllStocks())
            {
                report.Append("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n---\n");
            }
            return report.ToString();
        }

        /// <inheritdoc />
        public double GetTotalMoneyFromStock(int productID)
        {
            return this.GetStockFromProductID(productID).TotalFromSales;
        }

        /// <inheritdoc />
        public IStockManager RemoveStock(int productID)
        {
            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            this.database.GetCollection().DeleteMany(filter);
            return this;
        }

        /// <inheritdoc />
        public IStockManager SellStock(int productID, int quantity, double pricePerStock)
        {
            Stock stock = this.GetStockFromProductID(productID);

            stock.Quantity -= quantity;
            stock.NumberSold += quantity;
            stock.TotalFromSales += quantity * pricePerStock;

            this.RemoveStock(productID);
            this.AddNewStock(stock);

            return this;
        }
    }
}
