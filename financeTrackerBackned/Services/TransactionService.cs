using financeTrackerBackned.Data;
using financeTrackerBackned.Domain;
using financeTrackerBackned.Dtos;
using Microsoft.EntityFrameworkCore;

namespace financeTrackerBackned.Services
{
  public class TransactionService
  {
    private readonly DataContext _dataContext;
    public TransactionService(DataContext dataContext)
    {
      _dataContext = dataContext;
    }

    public async Task<Transaction> AddOne(TransactionDto transactionDto, int userId)
    {
      var createdTransaction = await _dataContext.Transactions.AddAsync(transactionDto.DtoToEntity(userId));
      await _dataContext.SaveChangesAsync();
      return createdTransaction.Entity;
    }

    public async Task<List<Transaction>> GetAll(int userId)
    {
      var transactions = await _dataContext.Transactions.Where(t => t.UserId == userId).ToListAsync();
      return transactions;
    }

    public async Task<Transaction?> GetOne(int transactionId)
    {
      var transaction = await _dataContext.Transactions.FindAsync(transactionId);
      return transaction;
    }
  }
}