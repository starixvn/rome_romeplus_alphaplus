/// <reference path="../core.ts" />
/// <reference path="../components/daterangepicker.ts" />
/// <reference path="../components/productmultiselection.ts" />
/// <reference path="../components/single-selection-dropdown.ts" />
/// <reference path="../components/tooltip.ts" />
/// <reference path="../base-report.ts" />

class ConsolidationView extends BaseReport {
    productComponent: ProductMultiSelectionComponent;
    dateRangeComponent: DateRangePickerComponent;
    reportTooltip: ReportTooltip;

    $productPTListPopup: JQuery;
    productPTListHint: string;

    isFirstSwitchMode = true;

    constructor(
        productComponent: ProductMultiSelectionComponent,
        dateRangeComponent: DateRangePickerComponent,
        reportTooltip: ReportTooltip
    ) {
        super();

        this.productComponent = productComponent;
        this.dateRangeComponent = dateRangeComponent;
        this.reportTooltip = reportTooltip;
    }

    init() {
        $.blockUI();

        this.cacheElements();
        this.initComponents();
        this.registerEvents();

        $.unblockUI();
    }

    cacheElements() {
        this.$productPTListPopup = $('#consolidation-product-pt-list-popup');
        this.productPTListHint = $('#product-pt-list-hint').html();
    }

    initComponents() {
        if (
            $('#IsSetDefaultStackMode').val().toString().toLowerCase() === 'true' &&
            alphaReports.screenSmallThan768()
        ) {
            this.setStackMode();
            this.reportTooltip.show(new ReportTooltipOption('icon-normal-column', 'consolidation'));
        }

        this.productComponent.render();
        $('#dropdown-currencies').multiselect({
            maxHeight: 300
        });
        this.dateRangeComponent.render();
    }

    registerEvents() {
        $('.icon-switchmode').on('click', this.onSwitchMode);
        super.registerDrilldownableEvents();

        const self = this;
        this.$reportForm.find('[name="ReportType"]').change(function () {
            self.$reportForm.attr('action', $(`#${$(this).attr('id')}-url`).attr('href'));
            self.$reportForm.submit();
        });

        this.$tableReport.find('.username-row').click(function () {
            self.collapseExpandRows($(this));
        });

        this.$tableReport.find('.downline-link').click(function (event) {
            event.stopPropagation();
        });

        this.$tableReport.find('.view-product-pt-list').click(function (event) {
            event.stopPropagation();
            self.viewProductPTList($(this));
        });
    }

    setStackMode() {
        this.$pageTitle.addClass('stack-mode');
        this.$tableReport.addClass('stack-mode');
        //this.replaceHrefParam('IsStackMode', true);

        alphaReports.combineConsecutiveCells($('td.col-member-winloss'), 3, 'col-member-winloss-combined');
        alphaReports.combineConsecutiveCells($('td.col-owner-turnover'), 4, 'col-member-owner-turnover-combined');
        this.isFirstSwitchMode = false;
    }

    onSwitchMode = () => {
        $.blockUI();

        const isStackMode = this.isStackMode();
        if (isStackMode) {
            this.$pageTitle.removeClass('stack-mode');
            this.$tableReport.removeClass('stack-mode');
        } else {
            this.$pageTitle.addClass('stack-mode');
            this.$tableReport.addClass('stack-mode');
        }

        //this.replaceHrefParam('IsStackMode', !isStackMode);

        if (this.isFirstSwitchMode) {
            if (isStackMode) {
                alphaReports.splitCell('col-member-winloss');
                alphaReports.splitCell('col-owner-turnover');
            } else {
                alphaReports.combineConsecutiveCells($('td.col-member-winloss'), 3, 'col-member-winloss-combined');
                alphaReports.combineConsecutiveCells(
                    $('td.col-owner-turnover'),
                    4,
                    'col-member-owner-turnover-combined'
                );
            }

            this.isFirstSwitchMode = false;
        }

        $.unblockUI();
    };

    onSubmitReport = (event: any) => {
        if (!this.productComponent.validateProductIds()) {
            event.preventDefault();
            return;
        }

        super.onSubmitReport(event, true);
    };

    // replaceHrefParam = (paramName: string, paramValue: any) => {
    //     this.linkBuilderComponent.updateParam(paramName, paramValue);
    // };

    collapseExpandRows = ($usernameRow) => {
        const custId = $usernameRow.data('custid');
        const $collapseExpandIcon = $usernameRow.find('#icon-' + custId);

        if ($collapseExpandIcon.hasClass('icon-arrow-right')) {
            $collapseExpandIcon.removeClass().addClass('icon-arrow-down');
            this.$tableReport.find('.child-row-' + custId).removeClass('hide');
        } else {
            $collapseExpandIcon.removeClass().addClass('icon-arrow-right');
            this.$tableReport.find('.child-row-' + custId).addClass('hide');
        }
    };

    viewProductPTList = ($icon) => {
        $('#product-pt-list-username').html($icon.data('username'));
        $('#product-pt-list-content').html(this.buildProductPTListHtml($icon.data('custid')));
        $('#product-pt-list-hint').html(this.productPTListHint.replace('{0}', $icon.data('userlevel')));

        this.$productPTListPopup.modal({ show: true, backdrop: false });
    };

    buildProductPTListHtml(custId: number) {
        const html = [];
        this.$tableReport.find('.child-row-' + custId).each(function (index, item) {
            const $item = $(item);
            if ($item.hasClass('consolidation-pt-row')) {
                if (index !== 0) {
                    html.splice(-1);
                    html.push('</li>');
                }

                html.push('<li>');
                html.push($item.find('.group-pt').html());
                html.push(': ');
            } else {
                html.push($item.find('.col-product').html());
                html.push(', ');
            }
        });

        html.splice(-1);
        html.push('</li>');

        return html.join('');
    }
}

$(function () {
    new ConsolidationView(
        new ProductMultiSelectionComponent(),
        new DateRangePickerComponent(),
        new ReportTooltip()
    ).init();
});
