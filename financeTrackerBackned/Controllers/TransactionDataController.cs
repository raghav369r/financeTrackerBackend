using financeTrackerBackned.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace financeTrackerBackned.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionDataController : ControllerBase
    {

        private readonly DataContext _dataContext;
        public TransactionDataController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [HttpGet("monthly/{date}")]
        public ActionResult GetMonthlyReport(String date)
        {
            // get total income, expense for given month
            var res = _dataContext.Transactions
                .FromSqlRaw("select [type], SUM([amount]) as total FROM Transactions WHERE [date] LIKE {0} group by [type]", date + "%").ToList();
            return Ok(res);
        }

        [HttpGet("daily/{date}")]
        public ActionResult GetDailyReport(String date)
        {
            // get expense of each day for given month
            var res = _dataContext.Transactions
                .FromSqlRaw("SELECT [date], SUM([amount]) AS expense FROM Transactions WHERE [date] LIKE {0} AND [type] = {1} GROUP BY [date]", date + "%", "expense")
    .ToList();
            return Ok(res);

        }

    }
}