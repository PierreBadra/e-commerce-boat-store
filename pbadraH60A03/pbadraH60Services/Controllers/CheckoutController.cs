using Microsoft.AspNetCore.Mvc;
using pbadraH60Services.CalculateTaxes;
using pbadraH60Services.CheckCreditCard;
using pbadraH60Services.Models;

namespace pbadraH60Services.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckoutController : Controller
{
    private readonly CheckCreditCardSoapClient _creditCardClient;
    private readonly CalculateTaxesSoapClient _calculateTaxesClient;

    public CheckoutController(CheckCreditCardSoapClient creditCardClient, CalculateTaxesSoapClient calculateTaxesClient)
    {
        _creditCardClient = creditCardClient;
        _calculateTaxesClient = calculateTaxesClient;
    }
    
    [HttpPost("CheckCreditCard")]
    public async Task<ActionResult> ValidateCreditCard(CreditCard creditCard)
    {
        var response = await _creditCardClient.CreditCardCheckAsync(creditCard.CardNumber);

        switch (response)
        {
            case -1: return BadRequest("Invalid card character length");
            case -2: return BadRequest("Card number contains non-numerical characters");
            case -3: return BadRequest("Card number groups sum are greater than 30");
            case -4: return BadRequest("Product of the last two digits is not a multiple of 2");
            case -5: return BadRequest("Insufficient card balance");
            default: return Ok("Valid card number");
        }
    }

    [HttpGet("CalculateTax/{provinceCode}/{amount}")]
    public async Task<ActionResult> ProvinceTaxes(string provinceCode, double amount)
    {
        if (amount < 0)
            return BadRequest("Amount cannot be negative");
        
        if (provinceCode.Equals("MB", StringComparison.OrdinalIgnoreCase))
            return Ok(amount * 0.12);
        
        var response = await _calculateTaxesClient.CalculateTaxAsync(amount, provinceCode);

        if (response == -1)
            return BadRequest("Invalid province code");
        
        return Ok(response);
    }
}