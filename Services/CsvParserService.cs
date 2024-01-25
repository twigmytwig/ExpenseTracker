using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.SqlServer.Server;
using System.Globalization;
using System.Transactions;

namespace ExpenseTracker.Services
{
    public class CsvParserService : ICsvParserService
    {
        public CsvParserService()
        {
        }

        public List<ExpenseTracker.Models.Transaction> GetWFTransactions(IFormFile formData, int accountId)
        {
            List<ExpenseTracker.Models.Transaction> transactions = new();

            using var reader = new StreamReader(formData.OpenReadStream());
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", HasHeaderRecord = false };
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<Utility.CsvData>();

            foreach (var record in records)
            {
                ExpenseTracker.Models.Transaction tempTransaction = new()
                {
                    Amount = record.Amount,
                    Notes = record.Description,
                    Date = record.Date,
                    AccountId = accountId,
                    isReoccuring = false,
                    CategoryId = 8

                };

                transactions.Add(tempTransaction);
            }
            return transactions;
        }
    }
}
