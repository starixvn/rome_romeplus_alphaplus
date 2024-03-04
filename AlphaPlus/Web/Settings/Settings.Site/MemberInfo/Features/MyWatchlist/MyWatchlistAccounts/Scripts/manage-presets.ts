"use strict";

class ManagePresets {
    private $: JQueryStatic;
    private topWindow: any;

    private alert: MyWatchlistAlert;
    private $tableBody: JQuery;
    private baseUrl: string;

    private reloadTimeout = 1000;
    private removeReloadTimeout = 500;

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    public init() {
        this.cacheDOMElements();
        this.bindEvents();
    }

    private cacheDOMElements() {
        this.alert = new MyWatchlistAlert(this.$);
        this.$tableBody = this.$("#table-body");
        this.baseUrl = this.$("#member-info-url").val();
    }

    private bindEvents() {
        this.$(".icon-edit").click(this.handleOpenEdit);
        this.$(".icon-check").click(this.handleSaveEdit);
        this.$(".icon-close").click(this.handleCloseEdit);
        this.$(".remove").click(this.handleRemove);
        this.$("#btn-add").click(this.handleAddRow);
        this.$("#btn-save").click(this.handleSaveNewRows);
        this.$tableBody.on("input", ".threshold-input", this.handleInputThresholdChange);
    }

    private escapeHtml = (str) => str.toString().replace(/&/g, '&amp;')
        .replace(/>/g, '&gt;')
        .replace(/</g, '&lt;')
        .replace(/\//g, '&#x2F')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&apos;');

    private handleOpenEdit = event => {
        const $presetHtml = this.$(event.currentTarget).parents(".preset");

        $presetHtml.find(".preset-title").html(`<input type="text" name="presetTitle" value="${this.escapeHtml($presetHtml.data("title"))}" maxlength="50" autocomplete="off" />`);
        $presetHtml.find(".winning-threshold").html(
            `<input type="text" class="threshold-input text-right" name="winningThreshold" value="${$presetHtml.data("winningthresholdstring")}" maxlength="18" autocomplete="off" />`);
        $presetHtml.find(".losing-threshold").html(
            `<input type="text" class="threshold-input text-right" name="losingThreshold" value="${$presetHtml.data("losingthresholdstring")}" maxlength="18" autocomplete="off" />`);

        $presetHtml.find(".edit-group").addClass("hidden");
        $presetHtml.find(".confirm-group").removeClass("hidden");
    }

    private handleInputThresholdChange = event => {
        const el = event.currentTarget;
        el.value = SiteHelper.ToMoneyFormat(el.value);
    }

    private handleSaveEdit = event => {
        const $presetHtml = this.$(event.currentTarget).parent().parent().parent();

        const presetTitle = $presetHtml.find('input[name="presetTitle"]').val();
        const winningThreshold = $presetHtml.find('input[name="winningThreshold"]').val();
        const losingThreshold = $presetHtml.find('input[name="losingThreshold"]').val();

        if (!this.validateThresholdInputs(presetTitle, winningThreshold, losingThreshold)) return false;

        const formData = {
            id: parseInt($presetHtml.data("id")),
            presetTitle,
            winningThreshold: SiteHelper.ToNumber(winningThreshold),
            losingThreshold: SiteHelper.ToNumber(losingThreshold)
        }

        AJAXHelper.postForm(
            `${this.baseUrl}MyWatchlistAccounts/UpdatePersonalPreset`,
            formData,
            result => {
                switch (result.Code) {
                    case 0:
                        this.alert.showSuccess(this.$("#update-successfully").val());
                        this.reloadAfterSuccess(this.reloadTimeout);
                        break;
                    case 1:
                        this.alert.showError(result.message, CancellationToken cancellation);
                        break;
                    case 103:
                        this.alert.showError(this.$("#preset-is-no-longer-exists").val());
                        break;
                    case 104:
                        this.alert.showError(this.$("#preset-is-duplicated").val());
                        break;
                }
            });
    }

    private handleCloseEdit = event => {
        const $presetHtml = this.$(event.currentTarget).parents(".preset");

        $presetHtml.find(".preset-title").html(this.escapeHtml($presetHtml.data("title")));
        $presetHtml.find(".winning-threshold").html($presetHtml.data("winningthresholdstring"));
        $presetHtml.find(".losing-threshold").html($presetHtml.data("losingthresholdstring"));

        $presetHtml.find(".edit-group").removeClass("hidden");
        $presetHtml.find(".confirm-group").addClass("hidden");
    }

    private handleRemove = event => {
        this.topWindow.bootboxConfirm(
            this.$("#are-you-sure-to-remove-preset").val(),
            null,
            () => {
                const $presetHtml = this.$(event.currentTarget).parents(".preset");
                const formData = { id: parseInt($presetHtml.data("id")) };

                AJAXHelper.postForm(
                    `${this.baseUrl}MyWatchlistAccounts/DeletePersonalPreset`,
                    formData,
                    result => {
                        if (result.IsSuccessful) {
                            this.alert.showSuccess(this.$("#remove-successfully").val());
                            this.reloadAfterSuccess(this.removeReloadTimeout);
                        } else if (result.Code === 1) {
                            this.alert.showError(result.message, CancellationToken cancellation);
                        }
                });
            });
    }

    private handleAddRow = () => {
        const dataIndex = parseInt(this.$tableBody.children().last().attr("data-index"));
        const index = isNaN(dataIndex) ? 0 : dataIndex + 1;
        this.$tableBody.append(
            `<tr id="preset-${index}"
                             class="preset newRow"
                             data-index="${index}">
                                <td class="text-center">${index + 1}</td>
                                <td class="preset-title"><input type="text" name="presetTitle" value="" maxlength="50" autocomplete="off" /></td>
                                <td class="winning-threshold"><input type="text" class="threshold-input text-right" name="winningThreshold" value="" maxlength="18" autocomplete="off" /></td>
                                <td class="losing-threshold"><input type="text" class="threshold-input text-right" name="losingThreshold" value="" maxlength="18" autocomplete="off" /></td>
                                <td class="action">
                                    <span class="icon-bin remove-new-row"></span>
                                </td>
                         </tr>`);

        this.$(".remove-new-row").on("click", event => this.$(event.currentTarget).parents(".preset").remove());
    }

    private handleSaveNewRows = () => {
        this.alert.hide();
        const params = { presets: [] };
        const newRows = this.$tableBody.find(".newRow");
        for (var row of newRows.toArray()) {
            const presetTitle = this.$(row).find('input[name="presetTitle"]').val();
            const winningThreshold = this.$(row).find('input[name="winningThreshold"]').val();
            const losingThreshold = this.$(row).find('input[name="losingThreshold"]').val();

            if (!this.validateThresholdInputs(presetTitle, winningThreshold, losingThreshold)) return;

            if (presetTitle !== "" && winningThreshold !== "" && losingThreshold !== "") {
                params.presets.push({
                    presetTitle,
                    winningThreshold: SiteHelper.ToNumber(winningThreshold),
                    losingThreshold: SiteHelper.ToNumber(losingThreshold)
                });
            }
        }

        if (params.presets.length === 0) {
            this.alert.showError(this.$("#no-change-to-update-error").val());
            return;
        }

        AJAXHelper.postForm(
            `${this.baseUrl}MyWatchlistAccounts/AddMultiplePersonalPreset`,
            params,
            result => {
                if (!result) return;

                if (result && result.Code === 1) {
                    this.alert.showError(result.message, CancellationToken cancellation);
                    return;
                }

                if (result.length > 0) {
                    const updateSuccessfullyMsg = this.$("#update-successfully").val();
                    const presetIsDuplicatedMsg = this.$("#preset-is-duplicated").val();
                    let resultAlerts = [];
                    let isFailed = false;

                    result.forEach(r => {
                        switch (r.Code) {
                            case 0:
                                resultAlerts.push({ Code: r.Code, Name: r.Name, Message: updateSuccessfullyMsg });
                                break;
                            case 103:
                                resultAlerts.push({ Code: r.Code, Name: r.Name, Message: presetIsDuplicatedMsg });
                                isFailed = true;
                                break;
                            default:
                                break;
                        }
                    });

                    this.alert.showMultiple(resultAlerts);

                    isFailed || this.reloadAfterSuccess();
                }
            });
    }

    private validateThresholdInputs = (presetTitle, winningThreshold, losingThreshold) => {
        if (presetTitle === "" && (winningThreshold !== "" || losingThreshold !== "")) {
            this.alert.showError(this.$("#must-enter-preset-name-error").val());
            return false;
        }

        if (presetTitle !== "" && (winningThreshold === "" || losingThreshold === "")) {
            this.alert.showError(this.$("#must-enter-threshold-error").val());
            return false;
        }

        return true;
    }

    private reloadAfterSuccess(reloadTimeout = 3000) {
        setTimeout(function () {
            window.location.reload();
        }, reloadTimeout);
    }
}

$(function () {
    new ManagePresets($, window.top).init();
})