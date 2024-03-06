"use strict";

class ProblemAccountPopup {
    private $: JQueryStatic;
    private topWindow: any;
    private indexManager: ProblemAccount
    private $editPopupContainer: JQuery;
    private $tblReport: JQuery;

    private editText: string;
    private editMultipleText: string;
    private memberInfoUrl: string;
    private roleId: string;
    private needReviewUsernames: string[];
    private needReviewUsernameIds: string[];
    private needUpdateSuspendUsernames: string[];
    private needUpdateSuspendUsernameIds: string[];

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public init($tblReport: JQuery, indexManager: ProblemAccount) {
        this.$tblReport = $tblReport;
        this.indexManager = indexManager;
        this.cacheDOMElements();

        this.$("#edit-multiple").click(event => this.openMultiplePopup(event.currentTarget));
        this.$editPopupContainer.on("mouseleave", _ => this.$editPopupContainer.modal("hide"));
    }

    private cacheDOMElements() {
        this.$editPopupContainer = this.$('#edit-popup');
        this.editText = this.$('#editSettingText').val();
        this.editMultipleText = this.$('#editMultipleSettingText').val();
        this.memberInfoUrl = this.$("#memberInfoUrl").val();
        this.roleId = this.$("#RoleId").val();

        this.topWindow["indexManager"].reloadProblemAccount = () => {
            this.indexManager.memberList.reloadPage();
        };

        window.top.successfulUpdatingCallbackAndReloadMainFrame = (reloadAfterThreeSecond = false) => {
            $.blockUI();
            setTimeout(() => {
                $.unblockUI();
                this.indexManager.memberList.reloadPage();
                window.top["popupManager"].close();
            }, reloadAfterThreeSecond ? 3000 : 1500);
        }
    }

    public bindEvents() {
        this.$(".edit").off().on("click", event => this.openSinglePopup(this.$(event.currentTarget)));
        this.$(".open-button").off().on("click", event => this.changeStatusToSuspend(this.$(event.currentTarget)));
    }

    private changeStatusToSuspend(element) {
        const $this = this.$(event.target);

        const SearchAccountId = this.$(element).data("custid");
        const SearchAccountName = $this.data("username");

        const message = this.$("#changeStatusToSuspendText").val();
        this.topWindow.bootboxConfirm(
            message,
            null,
            () => {
                AJAXHelper.postForm(
                    `${this.memberInfoUrl}CustomerList/UpdateCustomerStatus`,
                    {
                        SearchAccountId,
                        SearchAccountName,
                        SearchAccountRoleId: 1,
                        StatusName: "Suspended",
                        StatusValue: true
                    },
                    result => {
                        if (result && result.IsSuccessful) {
                            this.indexManager.memberList.reloadPage();
                            return;
                        }
                        this.topWindow.bootboxAlert(result.message);
                    });
            }
        );
    }

    private openSinglePopup(element) {
        const isReviewed = this.hasAttribute(element, "is-reviewed")
        const markReviewMenu = `<li class="edit-popup icon-edit" id="mark-as-reviewed-selected">${this.$("#markAsReviewedText").val()}</li>`;

        const disableMarkReviewMenu = `<li class="edit-popup icon-edit grey-color Disable_edit">${this.$("#markAsReviewedText").val()}</li>`;
        const content =
            `<ul>
              ${!isReviewed ? markReviewMenu : disableMarkReviewMenu}

                <li class="edit-popup icon-edit" id="add-to-my-watchlist-selected">${this.$("#addToMyWatchlistText").val()}</li>
             </ul>`;
        this.openEditSettingPopup(
            element,
            content,
            this.editText,
            {
                usernames: [this.$(element).data("username")],
                ids: [this.$(element).data("custid")],
            }
        );
    }

    private hasAttribute(element, attName) {
        return this.$(element).closest('tr').attr(attName);
    }

    private openMultiplePopup(element) {
        const checkboxs = this.$tblReport.find(".checkbox:checked")
        if (!checkboxs.length) {
            return
        }
        this.needReviewUsernames = [];
        this.needReviewUsernameIds = [];
        this.needUpdateSuspendUsernames = [];
        this.needUpdateSuspendUsernameIds = [];
        let isEnableMarkReview = false;
        let isEnableUpdateSuspend = false;

        const ids = [];
        const usernames = [];
        checkboxs.each((_, checkbox) => {
            if (!this.hasAttribute(checkbox, "is-reviewed")) {
                this.needReviewUsernames.push(this.$(checkbox).data("username"));
                this.needReviewUsernameIds.push(this.$(checkbox).data("custid"));

                isEnableMarkReview = true;
            }
            if (this.hasAttribute(checkbox, "is-open")) {
                this.needUpdateSuspendUsernames.push(this.$(checkbox).data("username"));
                this.needUpdateSuspendUsernameIds.push(this.$(checkbox).data("custid"));

                isEnableUpdateSuspend = true;
            }
            usernames.push(this.$(checkbox).data("username"));
            ids.push(this.$(checkbox).data("custid"));

        });
        const markReviewMenu = `<li class="edit-popup icon-edit" id="mark-multiple-as-reviewed-selected">${this.$("#markMultipleAsReviewedText").val()}</li>`;
        const disableMarkReviewMenu = `<li class="edit-popup icon-edit grey-color Disable_edit"> ${this.$("#markMultipleAsReviewedText").val()} </li>`;

        const updateSuspendMenu = `<li class="edit-popup icon-edit" id="suspend-multiple-accounts-selected">${this.$("#suspendMultipleAccountsText").val()}</li>`;
        const disableUpdateSuspendMenu = `<li class="edit-popup icon-edit grey-color Disable_edit"> ${this.$("#suspendMultipleAccountsText").val()} </li>`;

        const content =
            `<ul>
                ${isEnableMarkReview ? markReviewMenu : disableMarkReviewMenu}
                <li class="edit-popup icon-edit" id="add-multiple-to-watchlist-selected">${this.$("#addMultipleToWatchlistText").val()}</li>
                ${isEnableUpdateSuspend ? updateSuspendMenu : disableUpdateSuspendMenu}
            </ul>`;
        this.openEditSettingPopup(
            element,
            content,
            this.editMultipleText,
            {
                ids,
                usernames
            }
        );
    }

    private openEditSettingPopup(element, content, title, data) {

        this.$editPopupContainer.find('.modal-body').empty().append(content);

        const $modalDialog = this.$editPopupContainer.find('.modal-dialog');

        if (this.topWindow.alpha.isMobile) {
            this.$editPopupContainer.find('.modal-title').empty().append(title);
            $modalDialog.css({ height: '100%', width: '100%' });
            this.$editPopupContainer
                .css({ top: 0, left: 0, height: '100%', width: '100%' })
                .modal({ show: true, backdrop: false });
        }
        else {
            this.$editPopupContainer
                .css({ height: 'fit-content', width: 'fit-content' })
                .modal({ show: true, backdrop: false });

            $modalDialog.css({ width: '100%' });
            const width = $modalDialog.outerWidth();

            let top = this.$(element).offset().top - this.$(document).scrollTop();
            let left = this.$(element).offset().left + this.$(element).width();

            if (this.$(window).width() - left < width) {
                left = left - width;
            }

            this.$editPopupContainer.css({ top, left });
        }

        this.$('#mark-as-reviewed-selected').click(() => this.openSingleMarkReviewPopup(data));

        this.$('#mark-multiple-as-reviewed-selected').click(() => this.openMultipleMarkReviewPopup());

        this.$('#suspend-multiple-accounts-selected').click(() => this.openSuspendStatusPopup());

        this.$('#add-to-my-watchlist-selected').click(() => this.openAddToWatchlistPopup(data));

        this.$('#add-multiple-to-watchlist-selected').click(() => this.openAddToWatchlistPopup(data));
    }

    private openSuspendStatusPopup() {

        const isUpdateMultiple = this.needUpdateSuspendUsernames.length > 1;
        const title = isUpdateMultiple ? this.$("#suspendMultipleAccountsText").val() : this.$("#suspendAccountsText").val();

        const usernames = this.needUpdateSuspendUsernames.join(",");
        const ids = this.needUpdateSuspendUsernameIds.join(",");

        const params = `ids=${ids}&usernames=${usernames}&roleId=${this.roleId}`;
        const accountInfoUrl = this.$("#accountInfoUrl").val();
        this.$editPopupContainer.modal("hide");
        const url = `${accountInfoUrl}UnsuspendDownlines/MultipleSuspendConfirm?${params}`;
        this.topWindow.popupManager.open(
            url,
            title,
            534,
            154,
            "modal-mobile-friendly");
    }

    private openSingleMarkReviewPopup(data) {

        const title = this.$("#markAsReviewedText").val();
        const usernames = data.usernames.join(", ");
        const ids = data.ids.join(",");
        const params = `ids=${ids}&usernames=${usernames}&roleId=${this.roleId}`;

        const url = `${this.memberInfoUrl}ProblemAccount/Update?${params}`;
        this.$editPopupContainer.modal("hide");
        this.topWindow.popupManager.open(
            url,
            title,
            534,
            154,
            "modal-mobile-friendly");
    }

    private openMultipleMarkReviewPopup() {
        const isUpdateMultiple = this.needReviewUsernames.length > 1;
        const title = isUpdateMultiple ? this.$("#markMultipleAsReviewedText").val() : this.$("#markAsReviewedText").val();

        const usernames = this.needReviewUsernames.join(",");
        const ids = this.needReviewUsernameIds.join(",");
        const params = `ids=${ids}&usernames=${usernames}&roleId=${this.roleId}`;
        const url = `${this.memberInfoUrl}ProblemAccount/Update?${params}`;
        this.$editPopupContainer.modal("hide");
        this.topWindow.popupManager.open(
            url,
            title,
            534,
            154,
            "modal-mobile-friendly");
    }

    private openAddToWatchlistPopup(data) {
        const isSingleSelect = data.usernames.length === 1;
        const title = isSingleSelect
            ? this.$("#addToMyWatchlistText").val()
            : this.$("#addMultipleToMyWatchlistText").val();
        const custIds = data.ids.join(",");
        this.$editPopupContainer.modal("hide");
        this.topWindow.popupManager.open(
            `${this.memberInfoUrl}ProblemAccount/ConfirmAddToWatchlist?custIds=${custIds}`,
            title,
            534,
            238,
            "modal-mobile-friendly");
    }
}