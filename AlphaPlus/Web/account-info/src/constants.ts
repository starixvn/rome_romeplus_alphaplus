export const UserLevel = {
    Super: 4,
    Master: 3,
    Agent: 2,
    Member: 1
};

interface Params {
    actorRoleId: number;
    mobileView: boolean;
}

declare global {
    const params: Params;
    interface Window {
        alpha: {
            urls: { [key: string]: string };
        };
        $: JQueryStatic;
        ageWnd: any;
    }
}

export enum BlockIds {
    ContactInfo = 'contactInfo',
    BalanceInfo = 'balanceInfo',
    DownlineList = 'downlineList',
    Status = 'status',
    Setting = 'setting',
    LoginInfo = 'loginInfo',
    OnlineList = 'onlineList'
}
