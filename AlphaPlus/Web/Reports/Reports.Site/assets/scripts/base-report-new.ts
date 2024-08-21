class BaseReportNew {
    $pageTitle: JQuery;
    $reportForm: JQuery;

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

            const $iconInformation = $('#icon-information').click(() => this.showLegend($iconInformation));
            $.ajax({
                method: 'GET',
                url: reportLegendUrl,
                cache: false,
                success: (html) => this.renderLegend($iconInformation, html),
                complete: $.unblockUI
            });
        }
    }

    private showLegend($iconInformation: JQuery) {
        const $reportLegend = $('#report-legend').modal({ show: true, backdrop: false });
        const iconHelpPosition = $iconInformation.position();
        $reportLegend.css({
            bottom: 'auto',
            top: $('.entered-non-entered').length > 0 ? iconHelpPosition.top + 90 : iconHelpPosition.top + 60
        });
    }

    private renderLegend($iconInformation: JQuery, html: string) {
        $('body').append(html);

        if ($.trim(html) === '') {
            $iconInformation.parent().remove();
        } else {
            $iconInformation.parent().removeClass('hide');
        }
    }

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
