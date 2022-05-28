namespace StockManager.Database
{
    using System.Globalization;
    using System.Text;
    using CsvHelper;
    using CsvHelper.Configuration;
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
        public IStockManager AddStock(int productID, int quantity = 1)
        {
            Stock? stock = this.GetStockFromProductID(productID);
            if (stock == null)
            {
                return this;
            }

            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            var update = Builders<DatabaseStock>.Update.Set("Quantity", stock.Quantity + quantity);
            this.database.GetCollection().UpdateOne(filter, update);
            return this;
        }

        /// <inheritdoc />
        public IStockManager RemoveStock(int productID)
        {
            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            this.database.GetCollection().DeleteMany(filter);
            return this;
        }

        /// <inheritdoc />
        public IStockManager RemoveAllStock()
        {
            this.database.GetCollection().DeleteMany(_ => true);
            return this;
        }

        /// <inheritdoc />
        public IStockManager SellStock(int productID, double pricePerStock, int quantity = 1)
        {
            Stock? stock = this.GetStockFromProductID(productID);

            if (stock == null)
            {
                return this;
            }

            if (quantity <= 0 || quantity > stock.Quantity)
            {
                return this;
            }

            stock.Quantity -= quantity;
            stock.NumberSold += quantity;
            stock.TotalFromSales += quantity * pricePerStock;

            if (stock.Quantity < stock.SafeStockAmount)
            {
                int oneHundredDollarsWorth = (int)Math.Floor(100 / stock.Price);
                int order = (oneHundredDollarsWorth == 0) ? 1 : oneHundredDollarsWorth;
                // Always order at least one.
                stock.Quantity += order;
                this.ProduceStockOrder(stock.ProductID, stock.Description, order);
            }

            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            var update = Builders<DatabaseStock>.Update.Set("Quantity", stock.Quantity)
                .Set("NumberSold", stock.NumberSold)
                .Set("TotalFromSales", stock.TotalFromSales);
            this.database.GetCollection().UpdateOne(filter, update);
            return this;
        }

        /// <inheritdoc />
        public IStockManager EditStockPrice(int productID, double pricePerStock)
        {
            Stock? stock = this.GetStockFromProductID(productID);

            if (stock == null)
            {
                return this;
            }

            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            var update = Builders<DatabaseStock>.Update.Set("Price", pricePerStock);
            this.database.GetCollection().UpdateOne(filter, update);
            return this;
        }

        /// <inheritdoc/>
        public IStockManager EditSafeStockAmount(int productID, int safeStockAmount)
        {
            Stock? stock = this.GetStockFromProductID(productID);

            if (stock == null)
            {
                return this;
            }

            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            var update = Builders<DatabaseStock>.Update.Set("SafeStockAmount", safeStockAmount);
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

            Stock? stock = this.GetStockFromProductID(productID);
            if (stock != null)
            {
                return stock.NumberSold;
            }

            return -1;
        }

        /// <inheritdoc />
        public List<Stock> GetAllStocks()
        {
            IEnumerable<Stock> stocks = this.database.GetCollection().Find(_ => true).ToList();
            return stocks.ToList();
        }

        /// <inheritdoc />
        public List<Stock> SearchForStockFromDescription(string description)
        {
            IEnumerable<Stock> stocks = this.database.GetCollection()
                .AsQueryable()
                .Where(s => s.Description.ToLower().Contains(description.ToLower()))
                .ToList();
            return stocks.ToList();
        }

        /// <inheritdoc />
        public int GetProductIDFromDescription(string description)
        {
            Stock? found = this.SearchForStockFromDescription(description).FirstOrDefault();
            if (found != null)
            {
                return found.ProductID;
            }

            return -1;
        }

        /// <inheritdoc />
        public Stock? GetStockFromDescription(string description)
        {
            return this.SearchForStockFromDescription(description).FirstOrDefault();
        }

        /// <inheritdoc />
        public Stock? GetStockFromProductID(int productID)
        {
            var filter = Builders<DatabaseStock>.Filter.Eq("ProductID", productID);
            return this.database.GetCollection().Find(filter).FirstOrDefault();
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
            Stock? stock = this.GetStockFromProductID(productID);

            if (stock != null)
            {
                return stock.TotalFromSales;
            }

            return -1;
        }

        /// <inheritdoc />
        public void ProduceStockOrder(int productID, string description, int amount)
        {
            if (!File.Exists("StockOrder.csv"))
            {
                File.Create("StockOrder.csv");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var stream = File.Open("StockOfer.csv", FileMode.Append))
            using (var csv = new CsvWriter(new StreamWriter(stream), config))
            {
                var info = new[] { new { productID, description, amount } };
                csv.WriteRecords(info);
            }
        }
    }
}
