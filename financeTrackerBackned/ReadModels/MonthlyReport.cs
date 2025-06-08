namespace financeTrackerBackned.ReadModels
{
    public class MonthlyReport
    {
        public float Income {  get; set; }  
        public float Expense { get; set; }  
        public float NetIncome { get; set; }    
        public MonthlyReport(float income, float expense, float netIncome)
        {
            Income = income;
            Expense = expense;
            NetIncome = netIncome;
        }
    }
}
