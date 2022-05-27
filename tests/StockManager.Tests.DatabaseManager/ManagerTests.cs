namespace StockManager.Tests.DatabaseManager
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Bogus;
    using Bogus.DataSets;
    using Bogus.Extensions;
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
            .CustomInstantiator(f => new Stock(0, f.Random.Utf16String(5, 100), f.Random.Int(0, 100000), 0, 0))
            .RuleFor(s => s.Price, f => f.Random.Double(0.5, 50))
            .RuleFor(s => s.Description, f => f.Random.String(5, 100))
            .RuleFor(s => s.ProductID, f => f.Random.Int(0))
            .RuleFor(s => s.SafeStockAmount, f => f.Random.Int(0, 50))
            .RuleFor(s => s.Quantity, f => f.Random.Int(0, 50));

        private static Stock testStock = testStockGenerator.Generate();

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
            Manager.SellStock(testStock.ProductID, testStock.Price, testStock.Quantity - 1);
            Stock? stock = Manager.GetStockFromProductID(testStock.ProductID);
            Assert.IsNotNull(stock);
            Assert.AreEqual(stock.TotalFromSales, testStock.Price * (testStock.Quantity - 1));
            Assert.AreEqual(stock.Quantity + testStock.Quantity - 1, testStock.Quantity);
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
