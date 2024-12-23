import Button from './button';

export interface BalanceInfo {
    TotalBalance: number;
    YesterdayBalance: number;
    TotalBalanceTransferLink: string;
    YesterdayBalanceTransferLink: string;
    GivenCredit: number;
    GivenCreditMin: number;
    GivenCreditMax: number;
    PendingTransfer3rd: number;
    Outstanding: number;
    ShowOutstandingLink: boolean;
    ReportLink: Button;
    ShowTotalBalanceTransferLink: boolean;
    ShowYesterdayBalanceTransferLink: boolean;
    CanUpdateAccount: boolean;
    TotalCash: number;
    YesterdayCash: number;
    UsedCredit: number;
    NetPosition: number;
    BetCredit: number;
    MemberMaxCredit: number;
    ShowMemberMaxCreditLimit: boolean;
    MemberMaxCreditLimit: string;
    RewardReceivable: number;
    RewardPayable: number;
    RewardCurrency: string;
}
