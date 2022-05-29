namespace StockManager.Manager
{
    /// <summary>
    /// Stock product.
    /// </summary>
    public class Stock
    {
        private double price;
        private int quantity;
        private int safeStockAmount;

        /// <summary>
        /// Internal description.
        /// </summary>
        protected string description;

        /// <summary>
        /// Internal product ID.
        /// </summary>
        protected int productID;

        /// <summary>
        /// Gets the description of a stock.
        /// </summary>
        public string Description { get { return this.description; } }

        /// <summary>
        /// Gets the product ID.
        /// </summary>
        public int ProductID { get { return this.productID;  } }

        /// <summary>
        /// Gets or sets the price field.
        /// </summary>
        public double Price
        {
            get
            {
                return this.price;
            }

            set
            {
                if (value < 0)
                {
                    return;
                }

                this.price = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Gets or sets the safe stock amount.
        /// </summary>
        public int SafeStockAmount
        {
            get
            {
                return this.safeStockAmount;
            }

            set
            {
                if (value < 0)
                {
                    return;
                }

                this.safeStockAmount = value;
            }
        }

        /// <summary>
        /// Gets or sets the quantity amount.
        /// </summary>
        public int Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                if (value < 0)
                {
                    return;
                }

                this.quantity = value;
            }
        }

        /// <summary>
        /// Gets or sets the total from sales.
        /// </summary>
        public double TotalFromSales { get; set; }

        /// <summary>
        /// Gets or sets the number of this stock sold.
        /// </summary>
        public int NumberSold { get; set; }

        /// <summary>
        /// Checks whether two stocks are equal.
        /// </summary>
        /// <param name="obj">The other object to test against.</param>
        /// <returns>True if stocks are equal.</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || this.GetType() != obj.GetType().BaseType)
            {
                return false;
            }
            else
            {
                Stock other = (Stock)obj;
                return (this.ProductID == other.ProductID) && (this.Description == other.Description);
            }

        }

        /// <summary>
        /// Creates a hash code for a stock.
        /// </summary>
        /// <returns>The generated hashcode.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(this.ProductID, this.Description);
        }

        /// <summary>
        /// Creates a stock report for the stock.
        /// </summary>
        /// <returns>String containing the relevant stock information.</returns>
        public override string ToString()
        {
            return string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n", this.ProductID, this.Description, this.Quantity, this.NumberSold, this.TotalFromSales);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Stock"/> class.
        /// Creates a new Stock.
        /// </summary>
        /// <param name="price">Price of the stock.</param>
        /// <param name="description">Description of the stock.</param>
        /// <param name="productID">Product ID of the stock.</param>
        /// <param name="safeStockAmount">Quantity of stock to stay above without needing to reorder.</param>
        /// <param name="quantity">Amount of stock.</param>
        public Stock(int productID, string description, double price, int safeStockAmount, int quantity)
        {
            this.productID = productID;
            this.description = description;
            this.Price = price;
            this.SafeStockAmount = safeStockAmount;
            this.Quantity = quantity;
            this.TotalFromSales = 0;
            this.NumberSold = 0;
        }
    }
}
