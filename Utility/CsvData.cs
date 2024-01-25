using CsvHelper.Configuration.Attributes;

namespace ExpenseTracker.Utility
{
    public class CsvData
    {

        [Index(0)]
        public DateOnly Date { get; set; }
        [Index(1)]
        public float Amount { get; set; }
        [Index(2)]
        public string Asterik { get; set; }
        [Index(3)]
        public string Nothing { get; set; }
        [Index(4)]
        public string Description { get; set; }

    }
}
