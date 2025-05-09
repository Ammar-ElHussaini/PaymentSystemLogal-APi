using Data_Access_Layer.ProjectRoot.Core.Interfaces;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.DTOs.Helper;
using PaymentSystem.DTOs;
using PaymentSystem.Services.Interfaces;

namespace PaymentSystem.Services.Imp
{
    public class UserService : IUser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _config;
        private readonly IJwtService _jwtService;

        public UserService(IUnitOfWork unitOfWork, IConfiguration config, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _jwtService = jwtService;
        }

        public async Task<string?> RegisterAsync(RegisterDto dto)
        {
            var userRepo = _unitOfWork.Repository<Users>();

            var exists = await userRepo.FindAsync(u => u.UserName == dto.Username || u.Email == dto.Email);
            if (exists != null)
                return null;

            var user = MappingHelper.MapRegisterDtoToUser(dto);

            await userRepo.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return _jwtService.GenerateJwtToken(user);
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var userRepo = await _unitOfWork.Repository<Users>().FindAsync(s => s.UserName == dto.Username);

            if (userRepo == null || userRepo.PasswordHash != dto.Password)
                return null;

            return _jwtService.GenerateJwtToken(userRepo);
        }
    }
}
