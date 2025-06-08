namespace financeTrackerBackned.Dtos
{
  public class GetTrasactionsQueryParams
  {
    public DateOnly Date { set; get; }
    public string Type { set; get; } = "";
    public string Category { set; get; } = "";

  }
}
