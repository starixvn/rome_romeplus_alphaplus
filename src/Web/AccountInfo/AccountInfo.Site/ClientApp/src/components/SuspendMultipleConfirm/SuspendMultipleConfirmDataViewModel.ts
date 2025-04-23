export interface UnsuspendCustomerInfo {
    SearchAccountId: number;
    Username: string;
    Suspended: boolean;
}

export interface SuspendMultipleConfirmDataViewModel {
    RoleId: number;
    UnsuspendList: UnsuspendCustomerInfo[];
    DisplayShowAllAccountOption: boolean;
    TotalSelectedText: string;
    ShortenUsernames: string[];
    MoreDownlineUsernames: string[];
}
