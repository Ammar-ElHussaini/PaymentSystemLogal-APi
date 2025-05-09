using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Data_Acess_Layer.Models
{
    public class TransferStatus
    {
        [Key]
        public int TransferStatusId { get; set; }
        public string StatusName { get; set; }
    }

}
