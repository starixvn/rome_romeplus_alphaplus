using System.Text.Json.Serialization;

namespace AlphaPlus.Reports.Features.Shared.Models;

public abstract class NetWinLoss
{
    public decimal Turnover { get; set; }

    public decimal MemberWinLoss { get; set; }

    public decimal MemberComm { get; set; }

    public decimal MemberTotal
    {
        get => MemberWinLoss + MemberComm;
        private set => _ = value;
    }

    public decimal AgWinLoss { get; set; }

    public decimal AgComm { get; set; }

    public decimal AgTotal
    {
        get => AgWinLoss + AgComm;
        private set => _ = value;
    }

    public decimal MAgWinLoss { get; set; }

    public decimal MAgComm { get; set; }

    public decimal MAgTotal
    {
        get => MAgWinLoss + MAgComm;
        private set => _ = value;
    }

    public decimal SAgWinLoss { get; set; }

    public decimal SAgComm { get; set; }

    public decimal SAgTotal
    {
        get => SAgWinLoss + SAgComm;
        private set => _ = value;
    }

    public virtual decimal Company
    {
        get => -(MemberTotal + AgTotal + MAgTotal + SAgTotal);
        private set => _ = value;
    }

    public virtual NetWinLoss Add(NetWinLoss other)
    {
        Turnover += other.Turnover;

        MemberWinLoss += other.MemberWinLoss;
        MemberComm += other.MemberComm;
        MemberTotal += other.MemberTotal;

        AgWinLoss += other.AgWinLoss;
        AgComm += other.AgComm;
        AgTotal += other.AgTotal;

        MAgWinLoss += other.MAgWinLoss;
        MAgComm += other.MAgComm;
        MAgTotal += other.MAgTotal;

        SAgWinLoss += other.SAgWinLoss;
        SAgComm += other.SAgComm;
        SAgTotal += other.SAgTotal;

        Company += other.Company;

        return this;
    }
}