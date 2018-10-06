
namespace Shared.Queue
{
    public class BalanceImportMessage
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
        public int PeriodId { get; set; }
    }
}
