using System.ComponentModel.DataAnnotations;

namespace PaymentSystem.Data_Acess_Layer.Models
{
    public class PaymentMethod
    {

        [Key]
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; }
        public string Description { get; set; }
    }

}
