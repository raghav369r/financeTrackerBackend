using System.Security.Claims;
using System.Threading.Tasks;
using financeTrackerBackned.Data;
using financeTrackerBackned.Domain;
using financeTrackerBackned.Dtos;
using financeTrackerBackned.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace financeTrackerBackned.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TransactionService _transactionService;
        public TransactionController(DataContext dataContext, IHttpContextAccessor httpContextAccessor, TransactionService transactionService)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] TransactionDto transaction)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var Transaction = await _transactionService.AddOne(transaction, Convert.ToInt32(userId));
                return CreatedAtAction(nameof(GetTransaction), new { Id = transaction.Id }, transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactions()
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var transactions = await _transactionService.GetAll(Convert.ToInt32(userId));
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransaction(int id)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var transaction = await _transactionService.GetOne(id);
                if (transaction == null)
                    return NotFound(new { error = "transaction with given Id not found!!" });
                if (transaction.UserId != Convert.ToInt32(userId))
                    return Unauthorized(new { error = "You donot have access to this transaction!!" });
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult UpdateTransaction([FromBody] TransactionDto trasaction)
        {
            var trans = _dataContext.Transactions.Find(trasaction.Id);
            if (trans == null) return NotFound();
            trans.Amount = trasaction.Amount;
            trans.Date = trasaction.Date;
            trans.Category = trasaction.Category.ToString();
            trans.Date = trasaction.Date;
            trans.Description = trasaction.Description;

            _dataContext.Transactions.Update(trans);
            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(int id)
        {
            var transaction = _dataContext.Transactions.FirstOrDefault(t => t.Id == id);
            if (transaction == null) return NotFound();
            _dataContext.Transactions.Remove(transaction);
            _dataContext.SaveChanges();
            return Ok();
        }

        [HttpGet("type/{type}")]
        public IActionResult GetTransactionByType(string type)
        {
            var transactions = _dataContext.Transactions.Where(t => t.Type == type).ToList();
            return Ok(transactions);
        }

        [HttpGet("catageory/{cat}")]
        public IActionResult GetTransactionByCategory(string cat)
        {
            var transactions = _dataContext.Transactions.Where(t => t.Category == cat).ToList();
            return Ok(transactions);
        }

        [HttpGet("date/{date}")]
        public IActionResult GetTransactionByDate(DateOnly date)
        {
            var transactions = _dataContext.Transactions.Where(t => t.Date == date).ToList();

            return Ok(transactions);
        }
    }
}
