namespace StockManager.Tests.DatabaseManager
{
    using System.Collections.Generic;
    using System.Linq;
    using Bogus;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StockManager.Database;
    using StockManager.Manager;
    using StockManager.UI;

    /// <summary>
    /// Testing class for <see cref="DatabaseStockManager"/>.
    /// </summary>
    [TestClass]
    public class ManagerTests
    {
        private static readonly DatabaseStockManager Manager = App.GetStockManager();

        private static Faker<Stock> testStockGenerator = new Faker<Stock>()
            .CustomInstantiator(f => new Stock(f.Random.Int(0, 1000000), f.Random.Utf16String(5, 100), 0,  0, 0))
            .RuleFor(s => s.Price, f => f.Random.Double(0.5, 50))
            .RuleFor(s => s.SafeStockAmount, f => f.Random.Int(5, 50))
            .RuleFor(s => s.Quantity, f => f.Random.Int(10, 100));

        private static Stock testStock = testStockGenerator.Generate();

        /// <summary>
        /// Initialise the class tests.
        /// </summary>
        /// <param name="testContext">The test context.</param>
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Manager.TestMode = true;
        }

        /// <summary>
        /// All stocks should appear on the database and be retrieved.
        /// </summary>
        [TestMethod]
        public void AllStocksShouldAppear()
        {
            Manager.RemoveAllStock();
            Assert.AreEqual(0, Manager.GetAllStocks().Count);
            List<Stock> stocks = testStockGenerator.Generate(10);
            foreach (Stock stock in stocks)
            {
                Manager.AddNewStock(stock);
            }

            Assert.AreEqual(10, Manager.GetAllStocks().Count);
        }

        /// <summary>
        /// Should be able to remove all stock from the stock manager.
        /// </summary>
        [TestMethod]
        public void DatabaseShouldClear()
        {
            Manager.RemoveAllStock();
            Assert.AreEqual(0, Manager.GetAllStocks().Count);
            List<Stock> stocks = testStockGenerator.Generate(10);
            foreach (Stock stock in stocks)
            {
                Manager.AddNewStock(stock);
            }

            Assert.AreEqual(10, Manager.GetAllStocks().Count);
            Manager.RemoveAllStock();
            Assert.IsFalse(Manager.GetAllStocks().Any());
        }

        /// <summary>
        /// Database quantity should update with many.
        /// </summary>
        [TestMethod]
        public void MultiQuantityShouldUpdate()
        {
            const int extraStock = 10;
            this.NextTestStock();
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Manager.AddStock(testStock.ProductID, extraStock);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(testStock.Quantity + extraStock, stock.Quantity);
        }

        /// <summary>
        /// Database quantity should update with one more.
        /// </summary>
        [TestMethod]
        public void SingleQuantityShouldUpdate()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Manager.AddStock(testStock.ProductID);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(testStock.Quantity + 1, stock.Quantity);
        }

        /// <summary>
        /// Should be able to get stock amount.
        /// </summary>
        [TestMethod]
        public void StockAmountShouldBeValid()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(testStock.NumberSold, Manager.GetNumberSold(testStock.ProductID));
        }

        /// <summary>
        /// Stock price should be changeable.
        /// </summary>
        [TestMethod]
        public void StockPriceShouldEdit()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Assert.IsNotNull(Manager.GetStockFromProductID(testStock.ProductID));
            Manager.EditStockPrice(testStock.ProductID, testStock.Price * 2);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(stock.Price, testStock.Price * 2);
        }

        /// <summary>
        /// Stock quantity should be changeable.
        /// </summary>
        [TestMethod]
        public void StockQuantityShouldEdit()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Assert.IsNotNull(Manager.GetStockFromProductID(testStock.ProductID));
            Manager.EditStockQuantity(testStock.ProductID, testStock.Quantity * 2);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(stock.Quantity, testStock.Quantity * 2);
        }

        /// <summary>
        /// Stock safe amount should be editable.
        /// </summary>
        [TestMethod]
        public void StockSafeAmountShouldEdit()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Assert.IsNotNull(Manager.GetStockFromProductID(testStock.ProductID));
            Manager.EditSafeStockAmount(testStock.ProductID, testStock.SafeStockAmount * 2);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(stock.SafeStockAmount, testStock.SafeStockAmount * 2);
        }

        /// <summary>
        /// Stock should get added to the database.
        /// </summary>
        [TestMethod]
        public void StockShouldBeAdded()
        {
            this.NextTestStock();
            Stock? stock = Manager.AddNewStock(testStock).GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.IsTrue(stock.ProductID == testStock.ProductID);
        }

        /// <summary>
        /// Stock should be removed when requested.
        /// </summary>
        [TestMethod]
        public void StockShouldBeRemoved()
        {
            this.NextTestStock();
            Stock? stock = Manager.AddNewStock(testStock).GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Manager.RemoveStock(testStock.ProductID);
            Assert.AreEqual(Manager.GetStockFromProductID(testStock.ProductID), null);
        }

        /// <summary>
        /// Stock should sell properly.
        /// </summary>
        [TestMethod]
        public void StockShouldBeSold()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Manager.SellStock(testStock.ProductID, testStock.Price);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(stock.TotalFromSales, testStock.Price);
            Assert.AreEqual(testStock.Quantity, stock.Quantity + 1);
        }

        /// <summary>
        /// Multiple stocks should sell properly.
        /// </summary>
        [TestMethod]
        public void StocksShouldBeSold()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Manager.SellStock(testStock.ProductID, testStock.Price, testStock.Quantity - testStock.SafeStockAmount);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            double expected = testStock.Price * (testStock.Quantity - testStock.SafeStockAmount);
            Assert.AreEqual(stock.TotalFromSales, expected);
            Assert.AreEqual(Manager.GetTotalMoneyFromStock(testStock.ProductID), expected);
            Assert.AreEqual(testStock.SafeStockAmount, stock.Quantity);
        }

        /// <summary>
        /// Stocks with descriptions should be found when searched for.
        /// </summary>
        [TestMethod]
        public void StocksShouldBeFoundWithDescription()
        {
            Manager.RemoveAllStock();
            Assert.AreEqual(0, Manager.GetAllStocks().Count);
            Stock firstStock = new Stock(0, "Test Stock", 0, 5, 100);
            Stock secondStock = new Stock(2, "Stock with a test", 2, 5, 100);
            Stock thirdStock = new Stock(5, "random", 5, 10, 1000);
            Manager.AddNewStock(firstStock);
            Manager.AddNewStock(secondStock);
            Manager.AddNewStock(thirdStock);
            Assert.AreEqual(3, Manager.GetAllStocks().Count);
            Assert.AreEqual(2, Manager.SearchForStocksFromDescription("test").Count);
        }

        /// <summary>
        /// Stocks with product ID should be found when searched for,
        /// </summary>
        [TestMethod]
        public void StocksShouldBeFoundWithProductID()
        {
            Manager.RemoveAllStock();
            Assert.AreEqual(0, Manager.GetAllStocks().Count);
            Stock firstStock = new Stock(12345, "Test Stock", 5, 2, 100);
            Stock secondStock = new Stock(312355, "Second Test Stock", 7, 10, 50);
            Stock thirdStock = new Stock(999, "Third Test Stock", 50, 50, 1000);
            Manager.AddNewStock(firstStock);
            Manager.AddNewStock(secondStock);
            Manager.AddNewStock(thirdStock);
            Assert.AreEqual(3, Manager.GetAllStocks().Count);
            Assert.AreEqual(2, Manager.SearchForStocksFromProductID(123).Count);
        }

        /// <summary>
        /// Should be able to get the product id from description.
        /// </summary>
        [TestMethod]
        public void ProductIDFromDescription()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Assert.AreEqual(testStock.ProductID, Manager.GetProductIDFromDescription(testStock.Description));
        }

        /// <summary>
        /// Should be able to get the stock from the description.
        /// </summary>
        [TestMethod]
        public void StockFromDescription()
        {
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            Assert.AreEqual(testStock, Manager.GetStockFromDescription(testStock.Description));
        }

        /// <summary>
        /// Should be able to generate a valid stock report.
        /// </summary>
        [TestMethod]
        public void StockReportShouldBeValid()
        {
            Manager.RemoveAllStock();
            Assert.AreEqual(0, Manager.GetAllStocks().Count);
            this.NextTestStock();
            Manager.AddNewStock(testStock);
            string stockReport = testStock.ToString() + "---\n";
            Assert.AreEqual(Manager.GetStockReport(), stockReport);
        }

        /// <summary>
        /// Generate a new test stock.
        /// </summary>
        private void NextTestStock()
        {
            testStock = testStockGenerator.Generate();
        }
    }
}
