using financeTrackerBackned.Domain;
using financeTrackerBackned.Dtos;
using Microsoft.EntityFrameworkCore;

namespace financeTrackerBackned.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionSummaryDto> TransactionsSummary { get; set; }
        public DbSet<MonthlySummaryDto> MonthlySummary { get; set; }
        public DbSet<ExpenseSummaryDto> ExpenseSummary { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.user)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TransactionSummaryDto>().HasNoKey();
            modelBuilder.Entity<MonthlySummaryDto>().HasNoKey();
            modelBuilder.Entity<ExpenseSummaryDto>().HasNoKey();
            base.OnModelCreating(modelBuilder);
        }
    }
}