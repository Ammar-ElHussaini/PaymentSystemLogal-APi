using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.DTOs.Helper;

namespace PaymentSystem.Controllers.Transfer
{
    [ApiController]
    [Route("api/admin/transfers")]
    [Authorize(Roles = "Admin")]
    public class TransferAdminController : ControllerBase
    {
        private readonly TransferService _transferService;

        public TransferAdminController(TransferService transferService)
        {
            _transferService = transferService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transfers = await _transferService.GetAllTransfersAsync();
            var result = transfers.Select(t => MappingHelper.MapTransferToDto(t));

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var transfer = await _transferService.GetTransferByIdAsync(id);
            if (transfer == null)
                return NotFound("Transfer Not Found");

            return Ok(MappingHelper.MapTransferToDto(transfer));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _transferService.UpdateTransferStatusAsync(id, status);
            if (!result)
                return BadRequest("Vaild Update Transfer");

            return Ok("Update Transfer");
        }
    }

}
