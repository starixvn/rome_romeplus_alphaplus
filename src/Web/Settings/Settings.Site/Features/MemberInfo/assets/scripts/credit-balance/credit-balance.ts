/// <reference path="../components/pager.ts" />
declare var numeral: any;

interface JQuery {
    stickyTableHeaders(): void;
}

class CreditBalance {
    $: JQueryStatic;
    topWindow: any;
    creditBalanceForm: JQuery;
    $reportLegend: JQuery;
    $iconInformation: JQuery;
    pager: PagerComponent;
    columnsControl: ColumnsControlComponent;
    statusComponent: SingleSelectionDropdownComponent;
    levelTypeComponent: SingleSelectionDropdownComponent;
    compactFeedback: CompactFeedbackComponent;
    submitButton: JQuery;

    constructor(
        $: JQueryStatic,
        topWindow: any,
        pager: PagerComponent,
        columnsControl: ColumnsControlComponent,
        statusComponent: SingleSelectionDropdownComponent,
        levelTypeComponent: SingleSelectionDropdownComponent,
        compactFeedback: CompactFeedbackComponent
    ) {
        this.$ = $;
        this.topWindow = topWindow;
        this.pager = pager;
        this.columnsControl = columnsControl;
        this.statusComponent = statusComponent;
        this.levelTypeComponent = levelTypeComponent;
        this.compactFeedback = compactFeedback;
    }

    init = () => {
        this.cacheDOMElements();

        this.registerEvents();

        this.registerAutoSuggestion();

        this.statusComponent.render(new SingleSelectionDropdownOption('StatusId', 'HiddenStatusId'));
        this.levelTypeComponent.render(new SingleSelectionDropdownOption('dropdown-level-type', 'HiddenLevelType'));

        this.pager.render(
            new PagerOption(
                () => {
                    this.$.blockUI();
                    this.creditBalanceForm.submit();
                },
                'report-pager',
                'creditBalanceForm',
                'PageIndex',
                'PageSize',
                [10, 50, 100, 500]
            )
        );

        this.columnsControl.render(this.$('#CookiePrefix').val());

        const $tblReport = this.$('#tbl-report');
        $tblReport.removeClass('hide').stickyTableHeaders();
        $tblReport.find('.icon-information-outline').tooltip({ html: true, placement: 'right' });

        this.$('.view-downline')
            .unbind()
            .bind('click', (event) => {
                this.viewDataURL(event);
            });
        this.compactFeedback.init(this.creditBalanceForm, true);
    };

    viewDataURL = (event) => {
        event.preventDefault();

        const mainFrame = this.topWindow.$('#main');
        if (!this.topWindow.alpha.core.isIe()) {
            mainFrame.contents().find('head').append(this.$('#AssetCommonCssLink').val());
            mainFrame.contents().find('body').empty().block();
        }

        mainFrame.attr('src', this.$(event.currentTarget).data('url'));
    };

    cacheDOMElements = () => {
        this.creditBalanceForm = this.$('#creditBalanceForm');
        this.submitButton = this.$('#btn-submit');
        this.$reportLegend = this.$('#report-legend');
        this.$iconInformation = this.$('#icon-information');
    };

    givenPopupOnclosing = (username = null, updateSuccessfully = false, credit = 0, custid = 0) => {
        if (updateSuccessfully === true) {
            setTimeout(() => {
                window.location = window.location;
                window.top.updateGivenCreditPopup.close();
            }, 3000);
        } else {
            window.top.updateGivenCreditPopup.close();
        }
    };

    registerEvents = () => {
        const self: any = this;

        this.submitButton.click(function () {
            self.$.blockUI();
            self.$('#PageIndex').val('1');
        });

        this.$('.creditlink').click(function () {
            const $this: JQuery = self.$(this);
            const url: string =
                self.$('#GivenCreditLink').val() +
                '?SearchAccountId=' +
                $this.data('custid') +
                '&SearchAccountName=' +
                $this.data('username') +
                '&SearchAccountRoleId=' +
                $this.data('levelid');

            if (self.topWindow.updateGivenCreditPopup === null || self.topWindow.updateGivenCreditPopup === undefined) {
                self.topWindow.updateGivenCreditPopup = self.topWindow.ageWnd.createInstance();
            }

            const popupTitle = `${self.$('#GivenCreditPopupTitle').val()} -  ${$this.data('username')}`;
            self.topWindow.updateGivenCreditPopup.onClosing = self.givenPopupOnclosing;
            self.topWindow.popupManager.openWithHanlder(
                self.topWindow.updateGivenCreditPopup,
                url,
                popupTitle,
                470,
                360
            );
        });

        this.$('.balancelink, .yesterdaybalancelink').click(function () {
            const url: string = self.$(this).data('url');
            const popH: number = navigator.userAgent.match(/iPad/i) != null ? 430 : 534;
            self.topWindow.popupManager.openByRelativeUrl(url, '', 520, popH);
        });

        this.$('.iplink').click(function () {
            self.topWindow.popupManager.open('/main/IpInfo?ip=' + this.text, '', 400, 400);
        });

        this.$iconInformation.on('click', this.showLegend);

        this.$('.betcreditlink').click(function (e) {
            e.preventDefault();
            self.topWindow.popupManager.open(self.$(this).attr('href'), undefined, 380, 280);
        });
    };

    showLegend = () => {
        this.$reportLegend.modal({ show: true, backdrop: false });

        var iconHelpPosition = this.$iconInformation.position();
        this.$reportLegend.css({
            bottom: 'auto',
            top: iconHelpPosition.top + 90,
            left: 0
        });
    };

    registerAutoSuggestion = () => {
        const autoSuggestionUrl: string = this.$('#OldMainRootPath').val() + 'UserNameSuggestion';
        const custId: number = this.$('#autoSuggestionCustId').val();
        const $userNameInput: JQuery = this.$('.typeahead');

        const getSuggestions: any = (query: string, process: any) => {
            return this.$.get(
                autoSuggestionUrl,
                { custid: custId, DirectDownlines: true, Query: query, isAccInfo: false, excludeCashMember: true },
                (data: any) => {
                    const suggestions: any = data;
                    return !suggestions || !suggestions.length ? process(['Not found']) : process(suggestions);
                }
            );
        };

        const typeaheadOptions: any = {
            minLength: 2,
            items: 10,
            name: 'UserName',
            autoSelect: false,
            delay: 200,
            source: getSuggestions,
            displayText: (item: any) => item.UserName + '#' + item.FirstName + ' ' + item.LastName,
            highlighter: (displayText: string) => {
                const parts: string[] = displayText.split('#');
                return parts[0] + '<br/>' + parts[1];
            },
            afterSelect: (item: any) => $userNameInput.val(item.UserName)
        };

        $userNameInput.typeahead(typeaheadOptions).on('keyup', (event: any) => {
            if (event.keyCode === 13) {
                this.submitButton.trigger('click');
            }
        });
    };
}

$(() => {
    const creditBalance: any = new CreditBalance(
        $,
        window.top,
        new PagerComponent($),
        new ColumnsControlComponent($, window.document),
        new SingleSelectionDropdownComponent($),
        new SingleSelectionDropdownComponent($),
        new CompactFeedbackComponent()
    );
    creditBalance.init();
});
