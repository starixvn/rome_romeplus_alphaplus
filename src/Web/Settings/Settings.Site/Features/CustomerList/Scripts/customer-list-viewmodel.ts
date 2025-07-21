'use strict';

class CustomerListViewModel {
    UplineRoleId: number;
    SearchAccountId: number;
    SearchAccountName: string;
    SearchAccountRoleId: number;
    DownlineLevelName: string;
    ShowEditMultiple: boolean;
    IsMobileView: boolean;
    BreadcrumbLevelJson: string;
    OldMainUrl: string;
    ReportsUrl: string;
    MemberInfoUrl: string;
    AccountInfoUrl: string;
    LimitedDateToShowInfo: boolean;
    CanViewReport: boolean;
    CanUpdateAccount: boolean;
    CustomerListData: any;
    IsDirectMember: boolean;
    ServiceType: number;
    OfferedCashService: boolean;

    constructor(
        uplineRoleId: number,
        searchAccountId: number,
        searchAccountName: string,
        searchAccountRoleId: number,
        downlineLevelName: string,
        showEditMultiple: boolean,
        isMobileView: boolean,
        breadcrumbLevelJson: string,
        oldMainUrl: string,
        reportsUrl: string,
        memberInfoUrl: string,
        accountInfoUrl: string,
        limitedDateToShowInfo: boolean,
        canViewReport: boolean,
        canUpdateAccount: boolean,
        customerListData: any,
        isDirectMember: boolean,
        serviceType: number,
        offeredCashService: boolean
    ) {
        this.UplineRoleId = uplineRoleId;
        this.SearchAccountId = searchAccountId;
        this.SearchAccountRoleId = searchAccountRoleId;
        this.SearchAccountName = searchAccountName;
        this.DownlineLevelName = downlineLevelName;
        this.ShowEditMultiple = showEditMultiple;
        this.IsMobileView = isMobileView;
        this.BreadcrumbLevelJson = breadcrumbLevelJson;
        this.OldMainUrl = oldMainUrl;
        this.ReportsUrl = reportsUrl;
        this.MemberInfoUrl = memberInfoUrl;
        this.AccountInfoUrl = accountInfoUrl;
        this.LimitedDateToShowInfo = limitedDateToShowInfo;
        this.CanViewReport = canViewReport;
        this.CanUpdateAccount = canUpdateAccount;
        this.CustomerListData = customerListData;
        this.IsDirectMember = isDirectMember;
        this.ServiceType = serviceType;
        this.OfferedCashService = offeredCashService;
    }
}

enum ServiceType {
    Credit = 0,
    Cash = 1,
    All = 2
}

enum CustomerLevel {
    Member = 1,
    Agent,
    Master,
    Super
}

enum LevelType {
    DirectMember = '2'
}

const ClosedReason = {
    ByAdmin: 1,
    ByUpline: 2,
    ByArchive: 4
};

const LockedAccess = {
    FalseOTPSC: 1,
    FalsePassword: 2
};

const statusClasses = {
    Closed: [
        {
            Id: ClosedReason.ByAdmin,
            Css: 'closed-byAdmin'
        },
        {
            Id: ClosedReason.ByUpline,
            Css: 'closed-byUpline'
        },
        {
            Id: ClosedReason.ByArchive,
            Css: 'closed-byArchive'
        }
    ],
    Suspended: {
        Css: 'RowBgSuspended'
    },
    Disabled: {
        Css: 'RowBgDisabled'
    },
    Enabled: {
        Css: 'enabled'
    },
    Locked: [
        {
            Id: LockedAccess.FalseOTPSC,
            Css: 'locked-falseOTPSC'
        },
        {
            Id: LockedAccess.FalsePassword,
            Css: 'locked-falsePassword'
        }
    ]
};

const buildAttributes = (attrs) => this.$.map(attrs, (value, key) => `${key}='${value}'`).join(' ');
