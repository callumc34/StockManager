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
        private const string COLLECTIONNAME = "Stocks";

        private MongoClient client;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoManager"/> class.
        /// </summary>
        /// <param name="uri">Mongo database URI.</param>
        public MongoManager(string uri)
        {
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
            return this.client.GetDatabase(DATABASE).GetCollection<DatabaseStock>(COLLECTIONNAME);
        }

        /// <summary>
        /// Create the collection if it doesn't exist.
        /// </summary>
        /// <returns>Returns the current MongoManager.</returns>
        private MongoManager CreateCollection()
        {
            if (this.client.GetDatabase(DATABASE).ListCollectionNames().ToList().Any(name => name == COLLECTIONNAME))
            {
                return this;
            }

            this.client.GetDatabase(DATABASE).CreateCollection(COLLECTIONNAME);
            return this;
        }
    }
}
