using System.Transactions;

namespace ExpenseTracker.Services
{
    public interface ICsvParserService
    {
        List<ExpenseTracker.Models.Transaction> GetWFTransactions(IFormFile formData, int accountId);
    }
}
