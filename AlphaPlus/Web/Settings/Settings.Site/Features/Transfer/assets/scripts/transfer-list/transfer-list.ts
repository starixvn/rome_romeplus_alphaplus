class TransferListView {
    alert: AlertComponent;
    usernameSuggestionComponent: UsernameSuggestionComponent;
    statusDropdownComponent: SingleDropdownTransferComponent;
    autoTransferDropdownComponent: SingleDropdownTransferComponent;
    levelTypeComponent: SingleDropdownTransferComponent;
    visibleColumnContextMenu: VisibleColumnContextMenuComponent;
    transferListPaging: TransferListPaging;

    constructor(
        alert: AlertComponent,
        usernameSuggestionComponent: UsernameSuggestionComponent,
        statusDropdownComponent: SingleDropdownTransferComponent,
        autoTransferDropdownComponent: SingleDropdownTransferComponent,
        levelTypeComponent: SingleDropdownTransferComponent,
        visibleColumnContextMenu: VisibleColumnContextMenuComponent,
        transferListPaging: TransferListPaging
    ) {
        this.alert = alert;
        this.usernameSuggestionComponent = usernameSuggestionComponent;
        this.statusDropdownComponent = statusDropdownComponent;
        this.autoTransferDropdownComponent = autoTransferDropdownComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.visibleColumnContextMenu = visibleColumnContextMenu;
        this.transferListPaging = transferListPaging;
    }

    init() {
        var isReloadTopPage = $('#hidden-isReloadTopPage').val();
        if(isReloadTopPage === '1') {
            top.indexManager.updateCurrentUrl("adbt=" + ($('#hidden-isAllDayBalanceTransfer').val() === '1' ? 'True' : 'False'));
            top.indexManager.addCustomCookies('messageForReloadSite', $('#hidden-messageForReloadSite').val());
            top.indexManager.isReloadTopPage = true;
            return;
        }

        this.initPrint();
        $('#icon-information').click(() => this.openPopup($('#hidden-transferHintUrl').attr('href'), 560, 320));

        const $formTransfer = $('#form-transfer').submit($.blockUI);
        const $tableTransfer = $('#tbl-transfer');
        this.initComponents($formTransfer, $tableTransfer);

        $('#AllYesterdayBalance').change(function () {
            $('#PageIndex').val(1);
            $formTransfer.submit();
        });

        $('#btn-submit').click(function () {
            $('#PageIndex').val(1);
        });

        this.initCheckboxAndButtonTransfer($tableTransfer);

        $('.switch-auto-transfer, .icon-calendar').click((event) => {
            event.preventDefault();
            this.openPopup($(event.target).attr('href'), 600);
        });

        $('.transfer-link').click((event) => {
            event.preventDefault();
            const $this = $(event.target);
            const width = $this.data('w') || 400;
            const height = $this.data('h') || 360;
            this.openPopup($this.attr('href'), width, height);
        });

        $tableTransfer.removeClass('hide').stickyTableHeaders();
    }

    private initPrint() {
        $('#icon-print').click(function () {
            const printContent = document.getElementById('block-tbl-transfer');
            const printWindow = window.open('', '', 'left=500,top=400,width=200,height=5');
            printWindow.document.write('<html>');
            printWindow.document.write('<head>');
            printWindow.document.write('</head>');
            printWindow.document.write("<body style='margin-top: 100px'>");
            printWindow.document.write(printContent.innerHTML);
            printWindow.document.write('</body></html>');
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        });
    }

    private initComponents($formTransfer: JQuery, $tableTransfer: JQuery) {
        this.transferListPaging.init(() => $formTransfer.submit());
        this.usernameSuggestionComponent.init({
            selectorId: 'Username',
            url: $('#hidden-usernameSuggestionUrl').val(),
            directDownlines: true,
            isAccInfo: false
        });
        this.statusDropdownComponent.render({ dropdownId: 'Status', buttonWidth: 130 });
        this.autoTransferDropdownComponent.render({ dropdownId: 'AutoTransfer', buttonWidth: 110 });
        this.levelTypeComponent.render({ dropdownId: 'dropdown-level-type', buttonWidth: 140 });
        this.visibleColumnContextMenu.init(`transfer_${$('#ViewerId').val()}`, $tableTransfer, 'level');
    }

    private initCheckboxAndButtonTransfer($tableTransfer: JQuery) {
        const $btnTransfer = $('#btn-transfer');
        const $btnAutoTransfer = $('#btn-autoTransfer');
        const $checkboxAll = $('#checkbox-all');
        const $checkboxList = $tableTransfer.find('.checkbox');

        $checkboxAll.change(function () {
            const checked = this.checked;
            $btnTransfer.prop('disabled', !checked);
            $btnAutoTransfer.prop('disabled', !checked);
            $checkboxList.prop('checked', checked);
        });

        $checkboxList.change(function () {
            const checkedCount = $checkboxList.filter(':checked').length;
            if (checkedCount === 0) {
                $checkboxAll.prop({
                    indeterminate: false,
                    checked: false
                });
                $btnTransfer.prop('disabled', true);
                $btnAutoTransfer.prop('disabled', true);
            } else {
                if (checkedCount === $checkboxList.length) {
                    $checkboxAll.prop({
                        indeterminate: false,
                        checked: true
                    });
                } else {
                    $checkboxAll.prop({
                        indeterminate: true,
                        checked: false
                    });
                }
                $btnTransfer.prop('disabled', false);
                $btnAutoTransfer.prop('disabled', false);
            }
        });

        $btnAutoTransfer.click(() => this.openPopupMultipleAutoTransfer($checkboxList));

        $('#menu-transfer').click(() => {
            const custIds = [];
            const custIdsDirectMember = [];
            $checkboxList.filter(':checked').each(function () {
                const $this = $(this);
                if ($this.data('value') !== 0) {
                    if ($this.data('isdirectmember') === 1) {
                        custIdsDirectMember.push($this.data('custid'));
                    } else {
                        custIds.push($this.data('custid'));
                    }
                }
            });

            const hasCustIds = custIds.length > 0;
            const hashCustIdsDirectMember = custIdsDirectMember.length > 0;
            if (!hasCustIds && !hashCustIdsDirectMember) {
                window.top.bootboxAlert($('#hidden-noTransferMade').val());
                return;
            }

            window.top.bootboxConfirm($('#hidden-confirmTransfer').val(), null, () => {
                let formData;
                if (hasCustIds && hashCustIdsDirectMember) {
                    formData = { custIds: custIds.join(',') };
                    const formDataDirectMember = { custIds: custIdsDirectMember.join(',') };
                    this.handleMultipleTransferYesterdayBalance(formData, false, () =>
                        this.handleMultipleTransferYesterdayBalance(formDataDirectMember, true, undefined)
                    );
                    return;
                }

                const $levelType = $('#hidden-queryLevelType');
                if ($levelType.length) {
                    formData = { custIds: hashCustIdsDirectMember ? custIdsDirectMember.join(',') : custIds.join(',') };
                    this.handleMultipleTransferYesterdayBalance(formData, hashCustIdsDirectMember, undefined);
                } else {
                    // Agent transfer Member
                    formData = { custIds: custIds.join(',') };
                    this.handleMultipleTransferYesterdayBalance(formData, true, undefined);
                }
            });
        });

        $('#menu-transfer-allpages').click((e) => {
            if ($(e.target).parent().hasClass('disabled')) {
                return;
            }

            window.top.bootboxConfirm($('#hidden-confirmTransfer').val(), null, () => {
                const formData = this.getTransferAllPagesParams();

                if (typeof formData.levelType === 'undefined') {
                    // Agent transfer Member
                    this.handleMultipleTransferYesterdayBalance(formData, true, undefined);
                } else if (formData.levelType === '2') {
                    this.handleMultipleTransferYesterdayBalance(formData, true, undefined);
                } else if (formData.levelType === '1') {
                    this.handleMultipleTransferYesterdayBalance(formData, false, undefined);
                } else {
                    this.handleMultipleTransferYesterdayBalance(formData, false, () =>
                        this.handleMultipleTransferYesterdayBalance(formData, true, undefined)
                    );
                }
            });
        });

        const isEnableAutoTransfer = $btnAutoTransfer.length > 0;
        isEnableAutoTransfer &&
            setTimeout(function () {
                try {
                    window.top.indexManager.initFeedbackFunction(5, undefined, 470);
                } catch {}
            }, 1000);
    }

    private openPopupMultipleAutoTransfer($checkboxList) {
        const onIds = [];
        const onNames = [];
        const offIds = [];
        const offNames = [];
        $checkboxList.filter(':checked').each(function () {
            const $this = $(this);
            const custId = $this.data('custid');
            const custName = $this.data('custname');
            if ($this.data('autotransfer') === 'on') {
                onIds.push(custId);
                onNames.push(custName);
            } else {
                offIds.push(custId);
                offNames.push(custName);
            }
        });

        if (onIds.length || offIds.length) {
            const url = [
                $('#hidden-updateMultipleAutoTransferUrl').val(),
                '&onIds=',
                onIds.join(','),
                '&onNames=',
                onNames.join(','),
                '&offIds=',
                offIds.join(','),
                '&offNames=',
                offNames.join(',')
            ].join('');
            this.openPopup(url, 600, 500);
        }
    }

    private getTransferAllPagesParams() {
        let formData: any = {
            custIds: '0',
            username: $('#hidden-queryUsername').val(),
            status: $('#hidden-queryStatus').val(),
            allYesterdayBalance: $('#AllYesterdayBalance').is(':checked'),
            isAllPages: true
        };

        const $autoTransfer = $('#hidden-queryAutoTransfer');
        if ($autoTransfer.length) {
            formData.autoTransfer = $autoTransfer.val();
        }

        const $levelType = $('#hidden-queryLevelType');
        if ($levelType.length) {
            formData.levelType = $levelType.val();
        }

        return formData;
    }

    private handleMultipleTransferYesterdayBalance(formData, isMember: boolean, callback: Function) {
        $.blockUI();
        const url = isMember
            ? $('#hidden-transferYesterdayBalanceMultipleMemberUrl').attr('href')
            : $('#hidden-transferYesterdayBalanceMultipleUrl').attr('href');
        $.ajax({
            headers: {
                RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: 'POST',
            url: url,
            data: formData,
            dataType: 'json',
            success: (result) => {
                if (result.code === 999991) {
                    window.top[url] = () => this.handleMultipleTransferYesterdayBalance(formData, isMember, callback);
                    window.top.ShowSecCodePopup(url);
                    return;
                }

                if (result.Code === 0) {
                    if (callback) {
                        callback();
                    } else {
                        window.top.bootboxAlert(result.Message);
                        window.top.closePopupAndReloadMainFrameAfterUpdateSuccess &&
                            window.top.closePopupAndReloadMainFrameAfterUpdateSuccess(3000);
                    }
                } else {
                    this.alert.showError(result.Message);
                }
            },
            complete: $.unblockUI
        });
    }

    private openPopup(url: string, width: number, height = 360) {
        if (window.top.popupManager) {
            window.top.popupManager.open(url, undefined, width, height, 'modal-mobile-friendly');
        } else {
            window.open(url, '_blank').focus();
        }
    }
}

$(function () {
    new TransferListView(
        new AlertComponent(),
        new UsernameSuggestionComponent(),
        new SingleDropdownTransferComponent(),
        new SingleDropdownTransferComponent(),
        new SingleDropdownTransferComponent(),
        new VisibleColumnContextMenuComponent(),
        new TransferListPaging()
    ).init();
});
