class CommissionView {
    init() {
        const $ddlProduct = $('#ddl-product');
        const $ddlMaster = $('#ddl-master');
        const $ddlAgent = $('#ddl-agent');
        const $iconLoadingAgent = $('#icon-loadingAgent');
        const $tblContainer = $('#tblRpt-container');

        $ddlProduct.multiselect({
            multiple: false,
            maxHeight: 300,
            onChange: ($option: JQuery) => {
                $.blockUI();
                window.location.href = this.buildUrl(`index?productId=${$option.val()}`);
            }
        });

        $ddlMaster.length &&
            $ddlMaster.multiselect({
                multiple: false,
                maxHeight: 300,
                nonSelectedText: $ddlAgent.data('label-nodata'),
                onChange: ($option: JQuery) => {
                    $.blockUI();
                    const masterId = $option.val();
                    $iconLoadingAgent.show();
                    $.get(this.buildUrl(`GetAgents?masterId=${masterId}`), function (agents) {
                        const html = agents.map(function (item) {
                            return `<option value="${item.CustId}">${item.CustName}</option>`;
                        });
                        $ddlAgent.html(html.join(''));
                        $ddlAgent.multiselect('rebuild');
                        agents && agents.length
                            ? $ddlAgent.closest('.form-group').removeClass('commission-nodata')
                            : $ddlAgent.closest('.form-group').addClass('commission-nodata');
                    }).always(() => $iconLoadingAgent.hide());
                    this.getMasterComm($ddlProduct.val(), masterId, $tblContainer);
                }
            });

        $ddlAgent.length &&
            $ddlAgent.multiselect({
                multiple: false,
                maxHeight: 300,
                nonSelectedText: $ddlAgent.data('label-nodata'),
                onChange: ($option: JQuery) => {
                    $.blockUI();
                    const custId = $option.val();
                    if (custId === 'undefined') {
                        this.getMasterComm($ddlProduct.val(), $ddlMaster.val(), $tblContainer);
                    } else {
                        $.get(
                            this.buildUrl(`GetData?productId=${$ddlProduct.val()}&custid=${$option.val()}`),
                            (html) => {
                                $tblContainer.html(html);
                                this.setStickyTableHeaders();
                            }
                        ).always($.unblockUI);
                    }
                }
            });

        this.setStickyTableHeaders();
    }

    private buildUrl(action: string): string {
        return `${$('#hidden-commissionUrl').val()}/${action}`;
    }

    private getMasterComm(productId, masterId, $tblContainer: JQuery) {
        $.get(this.buildUrl(`GetData?productId=${productId}&masterId=${masterId}`), (html) => {
            $tblContainer.html(html);
            this.setStickyTableHeaders();
        }).always($.unblockUI);
    }

    private setStickyTableHeaders() {
        $('#tbl-comm').stickyTableHeaders();
    }
}

$(function () {
    new CommissionView().init();
});
