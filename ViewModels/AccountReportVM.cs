using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.ViewModels
{
    public class AccountReportVM
    {
        public Account? Account { get; set; }
        public List<Transaction>? Transactions { get; set; }
        public required List<Category> Categories { get; set; }
        public JsonResult? ChartData { get; set; }
    }
}
