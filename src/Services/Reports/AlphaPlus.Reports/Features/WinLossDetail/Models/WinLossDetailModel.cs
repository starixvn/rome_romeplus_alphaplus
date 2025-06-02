using System.Text.Json.Serialization;
using AlphaPlus.Reports.Features.Shared.Models;

namespace AlphaPlus.Reports.Features.WinLossDetail;

public class WinLossDetailModel : NetWinLoss
{
    public int CustId { get; init; }

    public string CustName { get; init; }

    [JsonIgnore]
    public string FirstName { get; init; }

    [JsonIgnore]
    public string LastName { get; init; }

    public string FullName
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FirstName) && string.IsNullOrWhiteSpace(LastName))
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName))
            {
                return $"{FirstName}/ {LastName}";
            }

            return $"{FirstName}{LastName}";
        }
    }

    public bool IsExistedWatchlist { get; init; }

    public decimal RewardAmount { get; init; }

    public string RewardCurrency { get; init; }

    public decimal NetTurnover { get; set; }

    public decimal GrossComm { get; set; }

    public int NumberOfTickets { get; set; }

    public bool IsDirectMember { get; init; }

    public static WinLossDetailModel operator +(WinLossDetailModel model1, WinLossDetailModel model2)
    {
        model1.NetTurnover += model2.NetTurnover;
        model1.GrossComm += model2.GrossComm;
        model1.NumberOfTickets += model2.NumberOfTickets;
        model1.Add(model2);

        return model1;
    }
}