namespace StockManager.Database
{
    using MongoDB.Driver;

    /// <summary>
    /// Mongo database helper class.
    /// </summary>
    public class MongoManager
    {
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
        }
    }
}
