"use strict";

class EditSettingPopup {
    private $: JQueryStatic;
    private topWindow: any;

    private $ckbSelectAll: JQuery;
    private $editPopupContainer: JQuery;
    private editText: string;
    private editMultipleText: string;
    private baseUrl: string;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public init(tableReport: JQuery, baseUrl: string) {
        this.$ckbSelectAll = this.$("#select-all");
        this.$editPopupContainer = this.$('#edit-popup');
        this.editText = this.$('#edit-text').val();
        this.editMultipleText = this.$('#edit-multiple-text').val();
        this.baseUrl = baseUrl;

        this.$ckbSelectAll.click(_ => tableReport.find(".checkbox").prop("checked", this.$ckbSelectAll.is(":checked")));
        this.$("#edit-multiple").click(event => this.openMultiplePopup(event.currentTarget));
        this.$editPopupContainer.on("mouseleave", _ => this.$editPopupContainer.modal("hide"));
    }

    public bindEvents(tableReport: JQuery) {
        this.$ckbSelectAll.prop("checked", false);
        tableReport.find(".checkbox").off().on("click", _ =>
            this.$ckbSelectAll.prop("checked", tableReport.find(".checkbox").length === tableReport.find(".checkbox:checked").length));

        this.$(".edit").off().click(event => this.openSinglePopup(this.$(event.currentTarget)));
    }

    private openSinglePopup(element) {
        var content =
            `<ul>
                <li class="edit-popup icon-edit">${this.$("#edit-personal-threshold").val()}</li>
                <li class="remove-popup icon-bin">${this.$("#remove-account").val()}</li>
             </ul>`;
        this.openEditSettingPopup(
            element,
            content,
            this.editText,
            {
                usernames: [this.$(element).data("username")],
                custIds: [this.$(element).data("custid")],
                winningThreshold: this.$(element).data("winningthreshold"),
                losingThreshold: this.$(element).data("losingthreshold"),
                isEdit: true
            }
        );
    }

    private openMultiplePopup(element) {
        var content =
            `<ul>
                <li class="edit-popup icon-edit">${this.$("#edit-multiple-personal-threshold").val()}</li>
                <li class="remove-popup icon-bin">${this.$("#remove-multiple-accounts").val()}</li>
            </ul>`;

        const checkboxs = this.$('input.checkbox:checkbox:checked');
        const custIds = [];
        const usernames = [];
        checkboxs.each((_, checkbox) => {
            usernames.push(this.$(checkbox).data("username"));
            custIds.push(this.$(checkbox).data("custid"));
        });

        if (custIds.length > 0 && usernames.length > 0) {
            this.openEditSettingPopup(
                element,
                content,
                this.editMultipleText,
                {
                    custIds,
                    usernames,
                    winningThreshold: checkboxs.first().data("winningthreshold"),
                    losingThreshold: checkboxs.first().data("losingthreshold"),
                    isEdit: true
                }
            );
        }
    }

    private openEditSettingPopup(element, content, title, data) {
        this.$editPopupContainer.find('.modal-body').empty().append(content);

        const $modalDialog = this.$editPopupContainer.find('.modal-dialog');

        if (this.topWindow.alpha.isMobile) {
            this.$editPopupContainer.find('.modal-title').empty().append(title);
            $modalDialog.css({ height: '100%', width: '100%' });
            this.$editPopupContainer
                .css({ top: 0, left: 0, height: '100%', width: '100%', paddingRight: '0px' })
                .modal({ show: true, backdrop: false });
        }
        else {
            this.$editPopupContainer
                .css({ height: '70px', width: 'fit-content' })
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

        this.$('.edit-popup').click(() => {
            const title = data.usernames.length === 1
                ? data.usernames + " - " + this.$("#edit-account-popup-title").val()
                : this.$("#edit-account-popup-title").val();

            data.usernames = data.usernames.join(", ");
            data.custIds = data.custIds.join(",");

            const url = `${this.baseUrl}MyWatchlistAccounts/ManageWatchlistAccounts?${this.$.param(data)}`;
            this.topWindow.popupManager.open(
                url,
                title,
                820,
                520,
                "modal-mobile-friendly");
        });

        this.$('.remove-popup').click(() => this.openRemoveWatchlistAccountPopup(data));
    }

    private openRemoveWatchlistAccountPopup(data) {
        const usernames = data.usernames.join(", ");
        const custIds = data.custIds.join(",");

        const divUsernames = `<div style="max-height: 245px; overflow-y: auto;">${usernames}</div><br/>`;

        this.topWindow.bootboxConfirm(
            divUsernames + this.$("#are-you-sure-to-remove-accounts").val(),
            null,
            () => {
                AJAXHelper.postForm(
                    `${this.baseUrl}MyWatchlistAccounts/DeleteMultiplePersonalThreshold`,
                    { usernames, custIds },
                    result => {
                        if (result && result.Code === 1) {
                            this.topWindow.bootboxAlert(result.Message);
                            return;
                        }

                        if (result && result.length > 0) {
                            this.topWindow.reloadMyWatchlistDataTable();
                        }
                    });
            });
    }
}