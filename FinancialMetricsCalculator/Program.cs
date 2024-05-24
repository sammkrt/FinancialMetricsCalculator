using System;
using System.IO;
using System.Threading.Tasks;
using YahooFinanceApi;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Org.BouncyCastle.Crypto.Digests;
using iText.Kernel;

namespace FinancialMetricsCalculator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter the stock symbol (e.g., AAPL, MSFT, GOOG):");
            string symbol = Console.ReadLine().ToUpper();

            var securities = await Yahoo.Symbols(symbol).Fields(Field.Symbol, Field.RegularMarketPrice, Field.EpsTrailingTwelveMonths, Field.TrailingAnnualDividendYield, Field.MarketCap, Field.TrailingPE, Field.TrailingAnnualDividendRate).QueryAsync();
            var security = securities[symbol];

            double sharePrice = (double)security[Field.RegularMarketPrice];
            double eps = (double)security[Field.EpsTrailingTwelveMonths];
            double dividendRate = (double)security[Field.TrailingAnnualDividendRate];
            double marketCap = (double)security[Field.MarketCap];
            double peRatio = (double)security[Field.TrailingPE];
            double dividendYield = (double)security[Field.TrailingAnnualDividendYield];
            double totalEquity = marketCap / peRatio; // Approximation
            double netIncome = totalEquity * (eps / sharePrice); // Approximation
            double debt = totalEquity * 0.5; // Approximation for demonstration
            double freeCashFlow = netIncome * 0.75; // Approximation for demonstration
            double growthRate = 0.05; // Placeholder for demonstration

            double pegRatio = peRatio / growthRate;
            double debtEquityRatio = debt / totalEquity;
            double roe = netIncome / totalEquity;

            // Sonuçları yazdır
            Console.WriteLine($"\nP/E Ratio: {peRatio}");
            Console.WriteLine($"PEG Ratio: {pegRatio}");
            Console.WriteLine($"Dividend Yield: {dividendYield:P2}");
            Console.WriteLine($"Debt/Equity Ratio: {debtEquityRatio}");
            Console.WriteLine($"Free Cash Flow (EUR): {freeCashFlow}");
            Console.WriteLine($"ROE: {roe:P2}");

            // Sonuçları yorumla ve PDF olarak kaydet
            string yorum = GenerateCommentary(peRatio, pegRatio, dividendYield, debtEquityRatio, roe);

            // PDF dosyasını belirli bir dizine kaydet ve dosya adında şirket ismini kullan
            string fileName = $"FinancialMetricsReport_{symbol}.pdf";
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            CreatePdf(filePath, peRatio, pegRatio, dividendYield, debtEquityRatio, freeCashFlow, roe, yorum);
            Console.WriteLine($"PDF report has been saved to {filePath}");
        }

        static string GenerateCommentary(double peRatio, double pegRatio, double dividendYield, double debtEquityRatio, double roe)
        {
            string commentary = "Financial Metrics Commentary:\n\n";

            commentary += $"P/E Ratio: {peRatio}\n";
            if (peRatio < 15)
                commentary += "The P/E ratio is relatively low, indicating that the stock might be undervalued.\n";
            else if (peRatio > 25)
                commentary += "The P/E ratio is relatively high, indicating that the stock might be overvalued.\n";
            else
                commentary += "The P/E ratio is within a normal range.\n";

            commentary += $"PEG Ratio: {pegRatio}\n";
            if (pegRatio < 1)
                commentary += "The PEG ratio is less than 1, indicating that the stock might be undervalued given its growth rate.\n";
            else if (pegRatio > 2)
                commentary += "The PEG ratio is greater than 2, indicating that the stock might be overvalued given its growth rate.\n";
            else
                commentary += "The PEG ratio is within a normal range.\n";

            commentary += $"Dividend Yield: {dividendYield:P2}\n";
            if (dividendYield > 0.05)
                commentary += "The dividend yield is relatively high, providing good income for investors.\n";
            else
                commentary += "The dividend yield is relatively low.\n";

            commentary += $"Debt/Equity Ratio: {debtEquityRatio}\n";
            if (debtEquityRatio < 1)
                commentary += "The debt/equity ratio is low, indicating that the company is not heavily reliant on debt.\n";
            else if (debtEquityRatio > 2)
                commentary += "The debt/equity ratio is high, indicating that the company is heavily reliant on debt.\n";
            else
                commentary += "The debt/equity ratio is within a normal range.\n";

            commentary += $"ROE: {roe:P2}\n";
            if (roe > 0.15)
                commentary += "The return on equity is high, indicating efficient use of equity.\n";
            else
                commentary += "The return on equity is low, indicating less efficient use of equity.\n";

            return commentary;
        }

        static void CreatePdf(string filePath, double peRatio, double pegRatio, double dividendYield, double debtEquityRatio, double freeCashFlow, double roe, string commentary)
        {
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    document.Add(new Paragraph("Financial Metrics Report")
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                        .SetFontSize(20));

                    document.Add(new Paragraph($"P/E Ratio: {peRatio}"));
                    document.Add(new Paragraph($"PEG Ratio: {pegRatio}"));
                    document.Add(new Paragraph($"Dividend Yield: {dividendYield:P2}"));
                    document.Add(new Paragraph($"Debt/Equity Ratio: {debtEquityRatio}"));
                    document.Add(new Paragraph($"Free Cash Flow (EUR): {freeCashFlow}"));
                    document.Add(new Paragraph($"ROE: {roe:P2}"));

                    document.Add(new Paragraph("\n" + commentary));
                }
            }
        }
    }
}
