using System.Security.Claims;
using financeTrackerBackned.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financeTrackerBackned.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionReportController : ControllerBase
    {

        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TransactionReportController(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult> GetReports()
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });

            var overallSummary = await _dataContext.TransactionsSummary.FromSqlRaw("Select [Type],sum(Amount) as Sum from Transactions where Transactions.UserId={0} GROUP BY [Type]", 5)
                    .ToListAsync();
            var monthlySummary = await _dataContext.MonthlySummary.FromSqlRaw(@"SELECT [Date],
                    SUM(CASE WHEN [Type] = {0} THEN Amount ELSE 0 END) AS Income,
                    SUM(CASE WHEN [Type] = {1} THEN Amount ELSE 0 END) AS Expense 
                        FROM Transactions
                        WHERE UserId={2} AND [Date] >= CAST(DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1) AS DATE)
                        AND [Date] < CAST(DATEADD(MONTH, 1, DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), 1)) AS DATE)
                        GROUP BY [Date]", "Income", "Expense", Convert.ToInt32(userId))
                    .ToListAsync();
            var ExpenseSummary = await _dataContext.ExpenseSummary.FromSqlRaw("SELECT Category,SUM(Amount) AS Expense FROM Transactions WHERE [UserId]={0} AND [Type]={1} GROUP BY [Category]", Convert.ToInt32(userId), "Expense")
                    .ToListAsync();
            return Ok(new { overallSummary, monthlySummary, ExpenseSummary });
        }
    }
}