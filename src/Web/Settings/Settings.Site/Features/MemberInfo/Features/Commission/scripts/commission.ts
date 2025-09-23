class CommissionView {
    private readonly compactFeedback = new CompactFeedbackComponent();

    init() {
        const $ddlProduct = $('#ddl-product');
        const $ddlMaster = $('#ddl-master');
        const $ddlAgent = $('#ddl-agent');
        const $iconLoadingAgent = $('#icon-loadingAgent');
        const $tblContainer = $('#tblRpt-container');

        $ddlProduct.change(() => {
            $.blockUI();

            const queryString = [`productId=${$ddlProduct.val()}`];

            if ($ddlMaster.length) {
                queryString.push(`masterId=${$ddlMaster.val()}`);
            }

            if ($ddlAgent.length) {
                const recommendId = $ddlAgent.val();
                if (recommendId !== 'undefined') {
                    queryString.push(`recommendId=${recommendId}`);
                }
            }

            window.location.href = this.buildUrl(`index?${queryString.join('&')}`);
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
                if (custId === 'undefined' || custId === '0') {
                    this.getMasterComm($ddlProduct.val(), $ddlMaster.val(), $tblContainer);
                } else {
                    $.get(this.buildUrl(`GetData?productId=${$ddlProduct.val()}&recommendId=${custId}`), (html) => {
                        $tblContainer.html(html);
                        this.setStickyTableHeadersAndCompactFeedback({
                            productId: $ddlProduct.val(),
                            recommendId: custId
                        });
                    }).always($.unblockUI);
                }
            });

        this.setStickyTableHeadersAndCompactFeedback(this.getQueryString($ddlProduct, $ddlMaster, $ddlAgent));
    }

    private buildUrl(action: string): string {
        return `${$('#hidden-commissionUrl').val()}/${action}`;
    }

    private getMasterComm(productId, masterId, $tblContainer: JQuery) {
        $.get(this.buildUrl(`GetData?productId=${productId}&masterId=${masterId}`), (html) => {
            $tblContainer.html(html);
            this.setStickyTableHeadersAndCompactFeedback({ productId, masterId });
        }).always($.unblockUI);
    }

    private setStickyTableHeadersAndCompactFeedback(formData) {
        const $table = $('#tbl-comm');
        const firstCell = $table.find('tbody tr:first td:first');
        const hasData = firstCell.length > 0 && !firstCell.is('[colspan]');
        hasData && $table.stickyTableHeaders();
        this.compactFeedback.init(formData, hasData);
    }

    private getQueryString($ddlProduct, $ddlMaster, $ddlAgent) {
        const formData = { productId: $ddlProduct.val() };

        if ($ddlMaster.length) {
            formData['masterId'] = $ddlMaster.val();
        }

        if ($ddlAgent.length) {
            const recommendId = $ddlAgent.val();
            if (recommendId !== 'undefined') {
                formData['recommendId'] = recommendId;
            }
        }

        return formData;
    }
}

$(function () {
    new CommissionView().init();
});
