class CommissionView {
    init() {
        const $ddlProduct = $('#ddl-product');
        const $ddlMaster = $('#ddl-master');
        const $ddlAgent = $('#ddl-agent');
        const $iconLoadingAgent = $('#icon-loadingAgent');
        const $tblContainer = $('#tblRpt-container');

        $ddlProduct.change(() => {
            $.blockUI();
            window.location.href = this.buildUrl(`index?productId=${$ddlProduct.val()}`);
        });

        $ddlMaster.length &&
            $ddlMaster.change(() => {
                $.blockUI();
                const masterId = $ddlMaster.val();
                $iconLoadingAgent.show();
                $.get(this.buildUrl(`GetAgents?masterId=${masterId}`), function (agents) {
                    let html;
                    if (agents && agents.length) {
                        html = agents.map(function (item) {
                            return `<option value="${item.CustId}">${item.CustName}</option>`;
                        });
                    } else {
                        html = ['<option value="0">', $('#hidden-noInfoText').val(), '</option>'];
                    }
                    $ddlAgent.html(html.join('')).prop('disabled', masterId === '0');
                }).always(() => $iconLoadingAgent.hide());
                this.getMasterComm($ddlProduct.val(), masterId, $tblContainer);
            });

        $ddlAgent.length &&
            $ddlAgent.change(() => {
                $.blockUI();
                const custId = $ddlAgent.val();
                if (custId === 'undefined') {
                    this.getMasterComm($ddlProduct.val(), $ddlMaster.val(), $tblContainer);
                } else {
                    $.get(this.buildUrl(`GetData?productId=${$ddlProduct.val()}&recommendId=${custId}`), (html) => {
                        $tblContainer.html(html);
                        this.setStickyTableHeaders();
                    }).always($.unblockUI);
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
