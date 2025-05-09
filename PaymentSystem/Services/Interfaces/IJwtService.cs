using PaymentSystem.Data_Acess_Layer.Models;

namespace PaymentSystem.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(Users user);
    }


}
