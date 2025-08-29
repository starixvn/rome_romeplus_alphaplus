'use strict';
declare var initAutoComplete: Function;

class ProblemAccount {
    $: JQueryStatic;
    topWindow: any;
    $usernameAutoSuggestionPath: JQuery;
    private problemAccountPopup: ProblemAccountPopup;
    masterList: BaseList;
    agentList: BaseList;
    memberList: BaseList;
    viewResources: ViewResources;
    ActorLevel: number;
    constructor(
        $: JQueryStatic,
        topWindow: any,
        problemAccountPopup: ProblemAccountPopup,
        viewResources: ViewResources = null
    ) {
        this.$ = $;
        this.viewResources = viewResources === null ? new ViewResources($) : viewResources;
        this.topWindow = topWindow;
        this.problemAccountPopup = problemAccountPopup;
    }

    init = () => {
        this.ActorLevel = parseInt(this.$('#ActorLevel').val());
        this.render();
    };

    render = () => {
        this.initDownlineLists();
        const searchAccountRoleId = parseInt(this.$('#SearchRoleId').val());
        if (searchAccountRoleId > 0) {
            this.$(`#tab${searchAccountRoleId}`).click();
            switch (searchAccountRoleId) {
                case siteInfrastructure.Role.Master.Id:
                    this.masterList.reloadPage();
                    break;
                case siteInfrastructure.Role.Agent.Id:
                    this.masterList && this.masterList.reloadPage();
                    this.agentList.reloadPage();
                    break;
                case siteInfrastructure.Role.Member.Id:
                    this.masterList && this.masterList.reloadPage();
                    this.agentList && this.agentList.reloadPage();
                    this.memberList.reloadPage();
                    break;
            }
        }
    };

    initDownlineLists(): void {
        this.memberList = new BaseList(
            this.$,
            this.topWindow,
            siteInfrastructure.Role.Member.Id,
            siteInfrastructure.Role.Member.Name,
            new DateRangePickerComponent($),
            new CompactFeedbackComponent('#box-icon-title-member'),
            null,
            this.problemAccountPopup
        );
        this.memberList.init(this, null);

        if (this.ActorLevel >= siteInfrastructure.Role.Master.Id) {
            this.agentList = new BaseList(
                this.$,
                this.topWindow,
                siteInfrastructure.Role.Agent.Id,
                siteInfrastructure.Role.Agent.Name,
                new DateRangePickerComponent($),
                new CompactFeedbackComponent('#box-icon-title-agent')
            );
            this.agentList.init(this, this.memberList);
        }

        if (this.ActorLevel >= siteInfrastructure.Role.Super.Id) {
            this.masterList = new BaseList(
                this.$,
                this.topWindow,
                siteInfrastructure.Role.Master.Id,
                siteInfrastructure.Role.Master.Name,
                new DateRangePickerComponent($),
                new CompactFeedbackComponent('#box-icon-title-master')
            );
            this.masterList.init(this, this.agentList);
        }
    }
}

$(() => {
    const popup = new ProblemAccountPopup($, window.top);
    const problemAccount = new ProblemAccount($, window.top, popup);
    problemAccount.init();
});
