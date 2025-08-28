class SearchTicketView {
    $refNo: JQuery;
    $submitButton: JQuery;
    compactFeedback = new CompactFeedbackComponent();

    init() {
        this.$submitButton = $('#btn-submit');

        this.$refNo = $('#ref-no')
            .on('input', this.updateSubmitButtonStateOnTextChange)
            .on('paste', () => setTimeout(this.updateSubmitButtonStateOnTextChange, 100));

        const $betlistForm = $('#betlist-form');
        $betlistForm.submit((event) => {
            event.preventDefault();
            const refno = this.$refNo.val();
            AJAXHelper.get(`${$betlistForm.attr('action')}?refno=${refno}`, (html: string) => {
                $('#tbl-betlist-box').html(html);
                this.expandParlay();
                this.initCompactFeedback(refno);
            });
        });

        setTimeout(() => window.top.indexManager.initFeedbackFunction(3), 1000);
    }

    updateSubmitButtonStateOnTextChange = () => {
        const refno = this.$refNo.val();
        this.$submitButton.prop('disabled', refno.length === 0);
    };

    expandParlay() {
        const parlayClickableId = `#r${this.$refNo.val()} a#hidden0`;
        const parlayItem = $(parlayClickableId);
        parlayItem && parlayItem[0] && parlayItem[0].click();
    }

    initCompactFeedback(refno) {
        const firstCell = $('#tbl-betlist tbody tr:first td:first');
        const hasBetListData = firstCell.length > 0 && !firstCell.data('noinfo');

        if (hasBetListData) {
            this.compactFeedback.init({ refno });
        }

        this.compactFeedback.toggleShowHide(hasBetListData);
    }
}

$(function () {
    new SearchTicketView().init();
});
