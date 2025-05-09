using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Data_Acess_Layer.Models
{
    public class TransactionLog
    {
        [Key]
        public int LogId { get; set; }
        public int TransferId { get; set; }
        public Transfer Transfer { get; set; }
        public string LogMessage { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
