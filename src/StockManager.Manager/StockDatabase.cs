namespace StockManager.Manager
{
    using StockManager.Database;

    /// <summary>
    /// Implementation of <see cref="IStockManager"/> using a Mongo database.
    /// </summary>
    public class StockDatabase : IStockManager
    {
        private MongoManager database;


        /// <summary>
        /// Initializes a new instance of the <see cref="StockDatabase"/> class.
        /// </summary>
        /// <param name="uri">Mongo database URI.</param>
        public StockDatabase(string uri)
        {
            this.database = new MongoManager(uri);
        }
    }
}
