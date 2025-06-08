namespace financeTrackerBackned.Dtos
{
  public class MonthlySummaryDto
  {
    public DateOnly Date { set; get; }
    public Double Expense { set; get; }
    public Double Income { set; get; }
  }
}
