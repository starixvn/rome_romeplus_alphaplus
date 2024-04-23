class BaseReportNew {
    $pageTitle: JQuery;
    $reportForm: JQuery;

    $reportLegend: JQuery;
    $iconInformation: JQuery;

    constructor() {
        this.initBaseReport();
    }

    private initBaseReport() {
        // cache elements
        this.$pageTitle = $('.page-title');
        this.$reportForm = $('#report-form');

        // register event
        this.initBaseReportLegend();
        $('.icon-filter')
            .click(() => this.onClickIconFilter())
            .tooltip({ placement: 'left' });
    }

    private initBaseReportLegend() {
        const $loadLegendURL = $('#LoadLegendURL');
        if ($loadLegendURL.length) {
            const reportLegendUrl = $loadLegendURL.attr('href');
            $loadLegendURL.remove();

            this.$iconInformation = $('#icon-information').click(this.showLegend);
            $.ajax({
                method: 'GET',
                url: reportLegendUrl,
                cache: false,
                success: this.renderLegend,
                complete: $.unblockUI
            });
        }
    }

    private showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });
        const iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: $('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    };

    private renderLegend = (html: string) => {
        this.$reportLegend && this.$reportLegend.remove();
        $('body').append(html);

        if ($.trim(html) === '') {
            this.$iconInformation.parent().remove();
        } else {
            this.$iconInformation.parent().removeClass('hide');
        }

        this.$reportLegend = $('#report-legend');
    };

    protected onClickIconFilter() {
        const isHideFilter = this.$reportForm.hasClass('hide');
        isHideFilter ? this.showFilter() : this.hideFilter();
    }

    private showFilter() {
        this.$pageTitle.removeClass('hide-filter');
        this.$reportForm.removeClass('hide');
    }

    protected hideFilter() {
        this.$pageTitle.addClass('hide-filter');
        this.$reportForm.addClass('hide');
    }
}
