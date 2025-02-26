/// <reference path="../shared/top-windows-interface.ts" />

class BaseList {
    $: JQueryStatic;
    topWindow: any;
    reviewStatusComponent: SingleSelectionDropdownComponent;
    problemAccountPopup: ProblemAccountPopup;
    dateRangePickerComponent: DateRangePickerComponent;

    $fromDate: JQuery;
    $toDate: JQuery;

    $dropdownStatus: JQuery;
    $username: JQuery;
    $uplineName: JQuery;
    $tblReport: JQuery;
    getProblemAccountUrl: string;
    $usernameAutoSuggestionPath: JQuery;
    $viewedNameDirectUpline: JQuery;

    $downlineObject: BaseList;
    reportParam: ReportParam;

    indexManager: ProblemAccount;

    levelName: string;
    levelNameLowerCase: string;
    levelId: number;
    isIndirectDownlineOfActor: boolean;
    directDownlineRoleId: number;

    customerListHelper: CustomerListHelper;
    displayNoneClass = "display-none";

    $btnMarkReviewed: JQuery;
    $ckbSelectAll: JQuery;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        levelId: number,
        levelName: string,
        dateRangePickerComponent: DateRangePickerComponent,
        reviewStatusComponent: SingleSelectionDropdownComponent = null,
        problemAccountPopup: ProblemAccountPopup = null
    ) {
        this.$ = $;
        this.topWindow = topWindow;
        this.reviewStatusComponent = reviewStatusComponent === null ? new SingleSelectionDropdownComponent($) : reviewStatusComponent;
        this.levelName = levelName;
        this.levelNameLowerCase = levelName.toLowerCase();
        this.levelId = levelId;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.problemAccountPopup = problemAccountPopup;
        this.topWindow.customerListNew = true;
    }

    public init(indexManager: ProblemAccount, downlineObject: BaseList) {
        this.indexManager = indexManager;

        this.isIndirectDownlineOfActor = this.levelId < this.indexManager.ActorLevel - 1;
        this.directDownlineRoleId = this.levelId - 1;
        this.cacheDOMElements();
        this.problemAccountPopup && this.problemAccountPopup.init(this.$tblReport, indexManager);

        this.customerListHelper = new CustomerListHelper(this.$, indexManager.viewResources);
        this.$downlineObject = downlineObject;
        this.reviewStatusComponent.render(
            new SingleSelectionDropdownOption(`dropdown-reviewstatus-${this.levelNameLowerCase}`, `UserSelectedReviewedStatusId-${this.levelName}`));
        this.dateRangePickerComponent.render(
            new DateRangePickerOption(`daterange-picker-${this.levelNameLowerCase}`, ""));

        this.$tblReport.stickyTableHeaders();

        this.$btnMarkReviewed.click(() => this.markReviewedMultiple());
        this.$ckbSelectAll.off().change(() => this.handleCheckboxAllChanged());

        this.$(`#btn-submit-${this.levelNameLowerCase}`).off("click").on("click", () => {
            this.viewList();
        });

        this.$(`#icon-excel-${this.levelNameLowerCase}`).off("click").on("click", () => {
            this.export();
        });

        this.$username.keypress((e) => {
            if (e.which == 13) {
                this.viewList();
                e.preventDefault();
            }
        });

        this.$viewedNameDirectUpline.keypress((e) => {
            if (e.which == 13) {
                this.viewList();
                e.preventDefault();
            }
        });
    }

    public cacheDOMElements() {
        this.$usernameAutoSuggestionPath = this.$("#usernameAutoSuggestionPath");
        this.$username = this.$(`#txt-username-${this.levelNameLowerCase}`);
        this.$uplineName = this.$(`#txt-uplinename-${this.levelNameLowerCase}`);
        this.$dropdownStatus = this.$(`#dropdown-reviewstatus-${this.levelNameLowerCase}`);
        this.getProblemAccountUrl = this.$("#getProblemAccountUrl").val();
        this.$tblReport = this.$(`#tbl-${this.levelNameLowerCase}`);
        this.$fromDate = this.$(`#from-date-${this.levelNameLowerCase}`);
        this.$toDate = this.$(`#to-date-${this.levelNameLowerCase}`);
        this.$viewedNameDirectUpline = this.$(`#txt-uplinename-${this.levelId === siteInfrastructure.Role.Agent.Id ? 'master' : 'agent'}`);
        if (this.isIndirectDownlineOfActor) {
            this.reportParam = new ReportParam();
        }
        else {
            this.reportParam = {
                UserName: this.$username.val(),
                SearchRoleId: this.levelId,
                UplineName: "",
                UserSelectedReviewedStatusId: parseInt(this.$("#UserSelectedReviewedStatusId").val()),
                FromDate: new Date(),
                ToDate: new Date()
            };
        }

        this.$btnMarkReviewed = this.$(`#btn-markReviewed-${this.levelNameLowerCase}`).tooltip();
        this.$ckbSelectAll = this.$tblReport.find(`#select-all-${this.levelNameLowerCase}`);
    }

    export = () => {
        const downlineUsername = this.isIndirectDownlineOfActor ? this.$viewedNameDirectUpline.val() : null;

        this.$("#FromDate").val(this.$fromDate.val());
        this.$("#ToDate").val(this.$toDate.val());
        this.$("#UserSelectedReviewedStatusId").val(parseInt(this.$dropdownStatus.find("option:selected").val()));
        this.$("#UplineName").val(downlineUsername);
        this.$("#SearchRoleId").val(this.levelId);
        this.$("#UserName").val(this.$username.val());

        this.$("#export-excel-form").submit();
    }

    loadData = () => {
        this.$btnMarkReviewed.attr("disabled", "disabled");
        this.$ckbSelectAll.prop("checked", false);
        const downlineUsername = this.isIndirectDownlineOfActor ? this.$viewedNameDirectUpline.val() : null;

        this.reportParam.UplineName = downlineUsername;
        this.reportParam.SearchRoleId = this.levelId;
        this.reportParam.UserSelectedReviewedStatusId = parseInt(this.$dropdownStatus.find("option:selected").val());
        this.reportParam.UserName = this.$.trim(this.$username.val());
        this.reportParam.FromDate = this.$fromDate.val();
        this.reportParam.ToDate = this.$toDate.val();

        this.$.ajax({
            method: "GET",
            data: this.reportParam,
            url: this.getProblemAccountUrl,
            cache: false,
            success: (rs) => {
                this.renderCustomerList(rs);
            },
            complete: this.$.unblockUI
        });
    }

    viewDownlines(event) {
        const $this = this.$(event.currentTarget),
            username = $this.attr('username');

        this.$(`#tab${this.directDownlineRoleId}`).click();
        this.$downlineObject.$fromDate.val(this.$fromDate.val());
        this.$downlineObject.$toDate.val(this.$toDate.val());
        this.$downlineObject.dateRangePickerComponent.render();
        this.$downlineObject.$viewedNameDirectUpline.val(username);
        this.$downlineObject.$username.val('');

        this.$downlineObject.reviewStatusComponent.changeSelection(this.reportParam.UserSelectedReviewedStatusId);
        this.$downlineObject.viewList();
    }

    bindReportEvents() {
        this.$tblReport.find(".view-downline").click(event => this.viewDownlines(event));
        const checkboxList = this.$tblReport.find(".checkbox");
        checkboxList.off().change(e => this.handleCheckboxChanged(e, checkboxList));
        this.$tblReport.find(".not-reviewed").off().click((e) => {
            const $checkbox = this.$(e.target);
            this.openPopup([$checkbox.data("custid")], [$checkbox.data("username")], $checkbox.data("custrole"));
        });
    }

    markReviewedMultiple() {
        const checkboxList = this.$tblReport.find(".checkbox:checked");
        const checkboxListLength = checkboxList.length;
        if (!checkboxListLength) {
            return;
        }

        let ids = [], usernames = [], roleId = 0;
        for (let i = 0; i < checkboxListLength; i++) {
            const $checkbox = this.$(checkboxList[i]);
            ids.push($checkbox.data("custid"));
            usernames.push($checkbox.data("username"));
            roleId = $checkbox.data("custrole");
        }

        this.openPopup(ids, usernames, roleId);
    }

    openPopup(ids, usernames, roleId: number) {
        this.$("#Ids").val(ids);
        this.$("#UserNames").val(usernames);
        this.$("#RoleId").val(roleId);

        const popupWidth = 500, popupHeight = roleId === siteInfrastructure.Role.Member.Id ? 250 : 300;
        const action = this.$("target").attr("action");

        this.topWindow["indexManager"].reloadProblemAccount = () => this.viewList();
        this.topWindow["popupManager"].open('', this.indexManager.viewResources.UpdatePopupTitle, popupWidth, popupHeight, 'modal-mobile-friendly', null, null, null, null, {
            preUrl: action
        });
    }

    renderCustomerList(rs) {
        if (!rs) {
            return;
        }

        this.$tblReport.removeClass(this.displayNoneClass);

        this.$tblReport.find("> tbody").empty().html(
            this.customerListHelper.buildBody(rs, this.levelId, this.indexManager.ActorLevel, this.reportParam)
        );
        this.$(window).trigger('resize.stickyTableHeaders');

        this.bindReportEvents();
        this.problemAccountPopup && this.problemAccountPopup.bindEvents();
    }

    reloadPage() {
        const { UserName } = this.reportParam;
        this.$username.val(UserName);
        if (this.isIndirectDownlineOfActor) {
            this.$viewedNameDirectUpline.val(UserName);
        }

        this.viewList();
    }

    viewList() {
        this.$.blockUI();

        this.loadData();
    }

    handleCheckboxChanged(e, checkboxList) {
        const checked = this.$(e.target).is(":checked");
        this.$ckbSelectAll.prop("checked", checked && checkboxList.not(":checked").length === 0);
        if (checkboxList.filter(":checked").length > 1) {
            this.$btnMarkReviewed.removeAttr("disabled");
        } else {
            this.$btnMarkReviewed.attr("disabled", "disabled");
        }
    }

    handleCheckboxAllChanged() {
        const checked = this.$ckbSelectAll.is(":checked");
        const checkboxList = this.$tblReport.find(".checkbox");
        checkboxList.prop("checked", checked);
        if (checked && checkboxList.filter(":checked").length > 1) {
            this.$btnMarkReviewed.removeAttr("disabled");
        } else {
            this.$btnMarkReviewed.attr("disabled", "disabled");
        }
    }
}

class ViewResources {
    $: JQueryStatic;
    public NoInformationIsAvailable: string;
    public UpdatePopupTitle: string;
    public TotalColumnMaster: number;
    public TotalColumnAgent: number;
    public TotalColumnMember: number;
    public EditSettingText: string;
    public LockedText: string;
    public MonitoredMemberText: string;

    constructor($: JQueryStatic) {
        this.$ = $;

        this.NoInformationIsAvailable = this.$("#NoInformationIsAvailable").val();
        this.UpdatePopupTitle = this.$("#markAsReviewedText").val();
        this.TotalColumnMaster = parseInt(this.$("#TotalColumnMaster").val());
        this.TotalColumnAgent = parseInt(this.$("#TotalColumnAgent").val());
        this.TotalColumnMember = parseInt(this.$("#TotalColumnMember").val());
        this.EditSettingText = this.$('#editSettingText').val();
        this.LockedText = this.$("#text-locked").val();
        this.MonitoredMemberText = this.$("#MonitoredMemberText").val();
    }
}

class ReportParam {
    public UserName: string = '';
    public SearchRoleId: number = 0;
    public UplineName: string = '';
    public UserSelectedReviewedStatusId: number = 0;
    public FromDate: Date;
    public ToDate: Date;
}

class SiteInfrastructure {
    Role = {
        Super: { Id: 4, Name: "Super" },
        Master: { Id: 3, Name: "Master" },
        Agent: { Id: 2, Name: "Agent" },
        Member: { Id: 1, Name: "Member" },
    };
}

const siteInfrastructure = new SiteInfrastructure();
