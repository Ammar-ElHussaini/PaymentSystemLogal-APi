using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Data_Acess_Layer.Models
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; }
        public int PaymentMethodId { get; set; }
        public int Phone { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransferImage { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }
        public int TransferStatusId { get; set; }
        public TransferStatus TransferStatus { get; set; }
    }

}
