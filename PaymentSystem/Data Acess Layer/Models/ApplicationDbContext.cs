using Microsoft.EntityFrameworkCore;

namespace PaymentSystem.Data_Acess_Layer.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferStatus> TransferStatuses { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

}
