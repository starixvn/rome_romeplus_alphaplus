export const UserLevel = {
    Super: 4,
    Master: 3,
    Agent: 2,
    Member: 1
};

interface Params {
    actorName: string;
    actorRoleId: number;
    mobileView: boolean;
    showLoginInfo: boolean;
}

interface GivenCreditParam {
    searchAccountName: string;
    searchAccountId: number;
    searchAccountRoleId: number;
    currentGivenCredit: number;
    givenCreditMin: number;
    givenCreditMax: number;
    memberMaxCredit: number;
    availableCredit: number;
}

declare global {
    const params: Params;
    const givenCreditParams: GivenCreditParam;
    interface Window {
        alpha: {
            urls: { [key: string]: string };
        };
        $: JQueryStatic;
        ageWnd: any;
        bootboxConfirm: any;
        confirmPopupPopup: any;
        popupManager: any;
        bootboxAlert: any;
        updateProductStatusPopup: any;
        updateGivenCreditPopup: any;
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

export const LoadCustomerAllStatus = {
    ByUpdateStatus: 'reload_customer_all_status_by_update_status',
    ByClick: 'load_all_status_by_click'
};

export const LoadCustomerInfo = {
    ByUpdateStatus: 'reload_customer_info_by_status',
    ByUpdateGivenCredit: 'reload_customer_info_by_given_credit',
    ByClick: 'load_customer_info_by_click'
};

export const ProductConstants = {
    Sportsbook: 1,
    RNGLottery: 44,
    VoidBridge: 22,
    LiveCasino: 41,
    ProgressiveBonus: 10,
    RNGCasino: 6,
    ESport: 11,
    Keno: 8,
    VirtualSports: 5,
    NumberGame: 3
};
