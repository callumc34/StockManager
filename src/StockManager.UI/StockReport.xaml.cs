namespace StockManager.UI
{
    using System.Windows;
    using System.Windows.Documents;

    /// <summary>
    /// Interaction logic for StockReport.xaml
    /// </summary>
    public partial class StockReport : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockReport"/> class.
        /// </summary>
        public StockReport()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Set the RichTextBox value.
        /// </summary>
        /// <param name="text">Text to change to.</param>
        public void SetText(string text)
        {
            this.ReportText.Document.Blocks.Clear();
            this.ReportText.Document.Blocks.Add(new Paragraph(new Run(text)));
        }
    }
}
