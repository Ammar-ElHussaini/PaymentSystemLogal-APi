using System.ComponentModel.DataAnnotations;

namespace MyPaymentSystem.DTOs
{
    public class TransferDTO
    {
        public int TransferId { get; set; }

        [Required(ErrorMessage = "Sender phone number is required.")]
        public string SenderPhoneNumber { get; set; }

        [Required(ErrorMessage = "Receiver phone number is required.")]
        public string ReceiverPhoneNumber { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public string TransferDate { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public string PaymentMethod { get; set; }
    }
}
