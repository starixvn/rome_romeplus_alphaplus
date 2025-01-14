namespace AlphaPlus.Settings.Transfer.API.Models.Reward;

public class RewardPaymentEntity
{
    public long MaxTransId { get; init; }

    public int ProgramId { get; init; }

    public string ProgramName { get; set; }

    public int CustId { get; init; }

    public string Username { get; init; }

    public decimal Amount { get; set; }

    public bool PaymentStatus { get; init; }

    public DateTime? PaymentDate { get; init; }

    public bool IsDirectMember { get; init; }

    public static RewardPaymentEntity operator +(RewardPaymentEntity model1, RewardPaymentEntity model2)
    {
        model1.Amount += model2.Amount;

        return model1;
    }
}