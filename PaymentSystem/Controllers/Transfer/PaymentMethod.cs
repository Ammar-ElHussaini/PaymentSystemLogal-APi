using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentSystem.Data_Acess_Layer.Models;
using PaymentSystem.Services.Imp.TransferMangent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentSystem.Controllers.Transfer
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class PaymentMethodMn : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentMethodMn(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAll()
        {
            var methods = await _paymentService.GetAllPaymentMethodsAsync();
            return Ok(methods);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetById(int id)
        {
            var method = await _paymentService.GetPaymentMethodByIdAsync(id);
            if (method == null)
                return NotFound();

            return Ok(method);
        }

        // POST: api/PaymentMethod
        [HttpPost]
        public async Task<ActionResult> Create(PaymentMethod method)
        {
            await _paymentService.AddPaymentMethodAsync(method);
            return CreatedAtAction(nameof(GetById), new { id = method.PaymentMethodId }, method);
        }

        // PUT: api/PaymentMethod/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, PaymentMethod method)
        {
            if (id != method.PaymentMethodId)
                return BadRequest("ID mismatch");

            await _paymentService.UpdatePaymentMethodAsync(method);
            return NoContent();
        }

        // DELETE: api/PaymentMethod/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _paymentService.DeletePaymentMethodAsync(id);
            return NoContent();
        }
    }
}
