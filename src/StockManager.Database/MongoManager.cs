namespace StockManager.Database
{
    using MongoDB.Bson.Serialization;
    using MongoDB.Driver;
    using StockManager.Manager;

    /// <summary>
    /// Mongo database helper class.
    /// </summary>
    public class MongoManager
    {
        private const string DATABASE = "StockManager";

        private MongoClient client;

        /// <summary>
        /// Gets or sets a value indicating whether the mongo manager is in test mode or production mode.
        /// </summary>
        public bool TestMode { get; set; }

        /// <summary>
        /// Gets the collection name being used.
        /// </summary>
        private string CollectionName { get { return this.TestMode ? "StocksTest" : "Stocks"; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoManager"/> class.
        /// </summary>
        /// <param name="uri">Mongo database URI.</param>
        /// <param name="testMode">Set whether the database is in test mode.</param>
        public MongoManager(string uri, bool testMode = false)
        {
            this.TestMode = testMode;

            MongoClientSettings settings = MongoClientSettings.FromConnectionString(uri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            this.client = new MongoClient(settings);
            this.CreateCollection();
        }

        /// <summary>
        /// Check if a stock exists in the database.
        /// </summary>
        /// <param name="productID">Product ID to search for.</param>
        /// <returns>True if stock exists, false otherwise.</returns>
        public bool StockExists(int productID)
        {
            return this.GetCollection().Find(x => x.ProductID == productID).Any();
        }

        /// <summary>
        /// Get the default collection for the database.
        /// </summary>
        /// <returns>Returns the mongo collection.</returns>
        public IMongoCollection<DatabaseStock> GetCollection()
        {
            return this.client.GetDatabase(DATABASE).GetCollection<DatabaseStock>(this.CollectionName);
        }

        /// <summary>
        /// Create the collection if it doesn't exist.
        /// </summary>
        /// <returns>Returns the current MongoManager.</returns>
        private MongoManager CreateCollection()
        {
            const string testCollection = "StocksTest";
            const string collection = "Stocks";

            List<string> collectionNames = this.client.GetDatabase(DATABASE).ListCollectionNames().ToList();

            if (!collectionNames.Contains(testCollection))
            {
                this.client.GetDatabase(DATABASE).CreateCollection(testCollection);
            }

            if (!collectionNames.Contains(collection))
            {
                this.client.GetDatabase(DATABASE).CreateCollection(collection);
            }

            return this;
        }
    }
}
