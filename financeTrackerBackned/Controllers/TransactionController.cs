using System.Security.Claims;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TransactionService _transactionService;
        public TransactionController(IHttpContextAccessor httpContextAccessor, TransactionService transactionService)
        {
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
                var addedTransaction = await _transactionService.AddOne(transaction, Convert.ToInt32(userId));
                return CreatedAtAction(nameof(GetTransaction), new { Id = transaction.Id }, addedTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] GetTrasactionsQueryParams queryParams)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var transactions = await _transactionService.GetAll(Convert.ToInt32(userId), queryParams);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
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
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionDto trasaction)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var transactiontoUpdate = await _transactionService.GetOne(trasaction.Id);
                if (transactiontoUpdate == null)
                    return NotFound(new { error = "transaction with given Id not found!!" });
                if (transactiontoUpdate.UserId != Convert.ToInt32(userId))
                    return Unauthorized(new { error = "You donot have access to update this transaction!!" });
                var updatedTransaction = await _transactionService.UpdateOne(transactiontoUpdate, trasaction);
                return Ok(updatedTransaction);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{transactionId}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            string? userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized(new { error = "Erorr decoding token!, Login and try again!!" });
            try
            {
                var transactionToDelete = await _transactionService.GetOne(transactionId);
                if (transactionToDelete == null)
                    return NotFound(new { error = "transaction with given Id not found!!" });
                if (transactionToDelete.UserId != Convert.ToInt32(userId))
                    return Unauthorized(new { error = "You donot have access to Delete this transaction!!" });

                await _transactionService.DeleteOne(transactionToDelete);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
