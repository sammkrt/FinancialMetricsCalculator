# Financial Metrics Calculator

This is a .NET Core console application that calculates financial metrics for a given stock symbol and generates a PDF report. The application fetches financial data from Yahoo Finance, performs various calculations, and saves the results in a PDF file.

## Features

- Fetches financial data from Yahoo Finance.
- Calculates key financial metrics:
  - P/E Ratio
  - PEG Ratio
  - Dividend Yield
  - Debt/Equity Ratio
  - Free Cash Flow
  - ROE (Return on Equity)
- Generates a PDF report with the calculated metrics and commentary.
- Saves the PDF report with the company name in the file name.

## Requirements

- .NET Core SDK
- Internet connection for fetching financial data
- NuGet packages:
  - `YahooFinanceApi`
  - `iText7`
  - `iText7.bouncy-castle-adapter`
  - `BouncyCastle`

## Installation

1. Clone the repository or download the source code.
2. Open the project directory in a terminal or command prompt.
3. Restore the NuGet packages by running:
    ```bash
    dotnet restore
    ```
4. Ensure you have the necessary NuGet packages installed:
    ```bash
    dotnet add package YahooFinanceApi
    dotnet add package itext7
    dotnet add package itext7.bouncy-castle-adapter
    dotnet add package BouncyCastle
    ```

## Usage

1. Open the project directory in a terminal or command prompt.
2. Run the application using:
    ```bash
    dotnet run
    ```
3. Enter the stock symbol when prompted (e.g., AAPL, MSFT, GOOG).
4. The application will fetch the financial data, perform the calculations, and save the PDF report in the current directory.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

