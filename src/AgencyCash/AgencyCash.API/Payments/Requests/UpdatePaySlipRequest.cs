using System.ComponentModel.DataAnnotations;
using AgencyCash.API.Shared.DataAnnotations;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Requests;

public class UpdatePaySlipRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [Required(ErrorMessage = "{0} is required.")]
    public string PaySlip { get; init; }

    internal byte[] PaySlipBuffer { get; set; }

    public string IP { get; init; }

    public ModelValidationResult ValidatePaySlip()
    {
        if (!Base64StringHelper.TryConvertToByte(PaySlip, out var paySlipBuffer))
        {
            return new ModelValidationResult(nameof(PaySlip), $"{nameof(PaySlip)} is not a valid Base64 string.");
        }

        const int MaxPaySlipSize = 1024 * 1024; // 1MB
        if (paySlipBuffer.Length > MaxPaySlipSize)
        {
            return new ModelValidationResult(nameof(PaySlip), $"{nameof(PaySlip)} cannot exceed 1MB.");
        }

        PaySlipBuffer = paySlipBuffer;
        return null;
    }
}