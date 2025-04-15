declare var initAutoComplete: Function;

class TransferToDownlineHistory extends TransferReport {
    $: JQueryStatic;
    $username: JQuery;
    $usernameAutoSuggestionPath: JQuery;
    $output: JQuery;
    $exportExcel: JQuery;
    $exportExcelForm: JQuery;
    $fromDate: JQuery;
    $toDate: JQuery;

    dateRangePickerComponent: DateRangePickerComponentTransfer;
    levelTypeComponent: SingleDropdownTransferComponent;
    showLevelType: boolean;
    directMemberText: string;
    masterAgentText: string;

    constructor(
        $: JQueryStatic,
        dateRangePickerComponent: DateRangePickerComponentTransfer,
        levelTypeComponent: SingleDropdownTransferComponent
    ) {
        super($);

        this.$ = $;
        this.dateRangePickerComponent = dateRangePickerComponent;
        this.levelTypeComponent = levelTypeComponent;
    }

    public init(): void {
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type' });
        this.showLevelType = this.levelTypeComponent.$dropdown.length > 0;
        if (this.showLevelType) {
            this.directMemberText = this.$('#DirectMemberText').val();
            this.masterAgentText = this.$('#MasterAgentText').val();
        }

        super.init();
        this.initUsernameSuggestion();
        this.dateRangePickerComponent.render();
    }

    public initDataTable(): void {
        super.initDataTable({
            serverSide: true,
            ajax: (data, callback) => {
                const params: any = {
                    UserName: this.$username.val(),
                    FromDate: this.$fromDate.val(),
                    ToDate: this.$toDate.val(),
                    UserSelectedDateRangeOption: this.$('#hidden-range-option').val(),
                    PageIndex: data.start / data.length + 1,
                    PageSize: data.length
                };
                if (this.showLevelType) {
                    params.LevelType = this.levelTypeComponent.getSelected();
                }

                this.getTransferDownlineHistoryData(params, callback);
            },
            columns: this.buildDataTableColumns(),
            columnDefs: [],
            responsive: false,
            isSubmit: false
        });
    }

    private getTransferDownlineHistoryData(params, callback): void {
        this.$.ajax({
            type: 'POST',
            url: this.$('#GetTransferToDownlineHistoryDataUrl').val(),
            data: params,
            dataType: 'json',
            beforeSend: this.$.blockUI,
            success: function (data) {
                if (data.IsFailed && data.Code === 6001) {
                    window.top['bootboxAlert'](data.Message, null, window.top['headerNav'].reloadPage);
                    window.location.href = window.top['alpha'].urls.mainRootPath + 'Dashboard/Index';
                } else {
                    callback({
                        data: data.TransferToDownlines,
                        recordsTotal: data.TotalRecords,
                        recordsFiltered: data.TotalRecords
                    });
                }
            },
            complete: this.$.unblockUI
        });
    }

    private buildDataTableColumns = () => {
        const columns: any = [
            { className: 'col-date', data: 'FormattedActionDate' },
            { className: 'col-text', data: 'DownlineName' }
        ];

        if (this.showLevelType) {
            columns.push({
                className: 'col-text',
                data: null,
                render: (item) => (item.IsDirectMember ? this.directMemberText : this.masterAgentText)
            });
        }

        columns.push(
            { className: 'col-text', data: 'TransferTypeName' },
            { className: 'col-number', data: 'FormattedTransferAmount' },
            { className: 'col-number', data: 'FormattedOldBalance' },
            { className: 'col-number', data: 'FormattedNewBalance' },
            { data: 'Description' }
        );

        return columns;
    };

    public initUsernameSuggestion(): void {
        const queryUsernameURL = `${this.$usernameAutoSuggestionPath.val()}?${this.$.param({
            directdownlines: true,
            isaccinfo: false
        })}`;
        initAutoComplete(this.$username[0], queryUsernameURL);

        let left = 1,
            top = this.$username.offset().top + this.$username.outerHeight(true) + 1,
            width = utilsInstance.isMobileDevice()
                ? utilsInstance.isLandscapeMode()
                    ? `calc(50% - ${left}px)`
                    : `calc(100% - ${left}px)`
                : this.$username.outerWidth() - left;

        this.$output.css({ left, top, width });
    }

    public cacheDOMElements(): void {
        super.cacheDOMElements();

        this.$username = this.$('#txt-username');
        this.$usernameAutoSuggestionPath = this.$('#usernameAutoSuggestionPath');
        this.$output = this.$('#output');
        this.$exportExcel = this.$('#icon-excel');
        this.$exportExcelForm = this.$('#export-excel-form');
        this.$fromDate = this.$('#from-date');
        this.$toDate = this.$('#to-date');
    }

    public registerEvents(): void {
        super.registerEvents();
        this.$exportExcel.click(() => this.$exportExcelForm.submit());
    }
}

$(() => {
    new TransferToDownlineHistory(
        $,
        new DateRangePickerComponentTransfer($),
        new SingleDropdownTransferComponent()
    ).init();
});
