using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyPaymentSystem.DTOs;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.DTOs.Helper;
using System.Threading.Tasks;

namespace PaymentSystem.Controllers.Transfer
{
    [ApiController]
    [Route("api/user/transfers")]
    [Authorize]
    public class TransferUserController : ControllerBase
    {
        private readonly TransferService _transferService;

        public TransferUserController(TransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTransfer([FromForm] TransferDTO dto, IFormFile image)
        {
            int userId = int.Parse(User.FindFirst("id")?.Value!);

            var result = await _transferService.AddTransferAsync(dto, image, userId);

            if (!result)
                return BadRequest("Valid");

            return Ok("Done");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyTransfers()
        {
            int userId = int.Parse(User.FindFirst("id")?.Value!);

            var transfers = await _transferService.GetUserTransfersAsync(userId);
            var result = transfers.Select(t => MappingHelper.MapTransferToDto(t));

            return Ok(result);
        }
    }

}
