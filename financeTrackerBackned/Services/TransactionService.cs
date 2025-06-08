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

    public async Task<List<Transaction>> GetAll(int userId, GetTrasactionsQueryParams queryParams)
    {
      Console.WriteLine(queryParams.Date);
      Console.WriteLine(queryParams.Category);
      Console.WriteLine(queryParams.Type);
      var transactions = await _dataContext.Transactions
                            .Where(t => t.UserId == userId)
                            .Where(t => queryParams.Date == DateOnly.Parse("0001/01/01") || t.Date == queryParams.Date)
                            .Where(t => queryParams.Category == "" || t.Category == queryParams.Category)
                            .Where(t => queryParams.Type == "" || t.Type == queryParams.Type)
                            .ToListAsync();
      return transactions;
    }

    public async Task<Transaction?> GetOne(int transactionId)
    {
      var transaction = await _dataContext.Transactions.FindAsync(transactionId);
      return transaction;
    }

    public async Task<Transaction> UpdateOne(Transaction transactionToUpdate, TransactionDto transaction)
    {
      transactionToUpdate.Amount = transaction.Amount;
      transactionToUpdate.Description = transaction.Description;
      transactionToUpdate.Category = transaction.Category.ToString();
      transactionToUpdate.Type = transaction.Type.ToString();
      transactionToUpdate.Date = transaction.Date;
      var updatedTransaction = _dataContext.Transactions.Update(transactionToUpdate);
      await _dataContext.SaveChangesAsync();
      return updatedTransaction.Entity;
    }

    public async Task DeleteOne(Transaction transaction)
    {
      _dataContext.Transactions.Remove(transaction);
      await _dataContext.SaveChangesAsync();
    }

  }
}