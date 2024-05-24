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
        bootboxConfirm: any;
        confirmPopupPopup: any;
        popupManager: any;
        bootboxAlert: any;
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
