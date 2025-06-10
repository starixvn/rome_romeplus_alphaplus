class DashboardBase {
    protected $: JQueryStatic;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    loadInfo = (id: string, handleOnSuccessLoading: any) => {
        let $component = this.$(id),
            url = $component.attr('action-url');

        if (url !== '' && typeof url !== 'undefined') {
            const ajaxSettings = this.createAjaxSettings(url, 'GET', $component, handleOnSuccessLoading);

            this.$.ajax(ajaxSettings);
        }
    };

    private createAjaxSettings = (
        url: string,
        method: string,
        component: any,
        handleOnSuccessLoading: any
    ): JQueryAjaxSettings => {
        const ajaxSetting: JQueryAjaxSettings = {
            method: method,
            url: url,
            cache: false,
            success: (responseHtml) => {
                component.html(responseHtml);
                handleOnSuccessLoading();
            }
        };

        return ajaxSetting;
    };

    registerTooltip = (option) => {
        this.$('[data-toggle="tooltip"]').tooltip(option);
    };

    onClickCashServiceToggle = () => {
        const toggle = this.$("#cash-service-toggle");
        const isChecked = toggle.attr("data-state");
        if (isChecked === "on") {
            this.$("#disableCashServiceModal").modal('show');
        } else {
            this.$("#enableCashServiceModal").modal('show');
        }
    };

    handleEnableCashService = () => {
        this.$.ajax({
            url: '/main/Dashboard/UpdateCashServiceStatus',
            type: 'POST',
            data: {
                IsEnableCash: true
            },
            headers: {
                'RequestVerificationToken': this.$('input[name="RequestVerificationToken"]').val()
            },
            success: (response) => {
                if (response.isSuccessful) {
                    const toggle = this.$("#cash-service-toggle");
                    toggle.attr("data-state", "on");
                    toggle.addClass("active");
                    this.$("#enableCashServiceModal").modal('hide');
                } else {
                    alert(response.message);
                }
            }
        });
    };

    handleDisableCashService = () => {
        this.$.ajax({
            url: '/main/Dashboard/UpdateCashServiceStatus',
            type: 'POST',
            data: {
                IsEnableCash: false
            },
            headers: {
                'RequestVerificationToken': this.$('input[name="RequestVerificationToken"]').val()
            },
            success: (response) => {
                if (response.isSuccessful) {
                    const toggle = this.$("#cash-service-toggle");
                    toggle.attr("data-state", "off");
                    toggle.removeClass("active");
                    this.$("#disableCashServiceModal").modal('hide');
                } else {
                    alert(response.message);
                }
            }
        });
    };

    initializeCashService = () => {
        this.$("#cash-service-toggle").unbind().bind('click', () => this.onClickCashServiceToggle());
        this.$("#confirmEnableCashService").click(() => this.handleEnableCashService());
        this.$("#confirmDisableCashService").click(() => this.handleDisableCashService());
    };
}

$(() => {
    const balance = new DashboardBase($);
    balance.loadInfo('#balance', () => {
        balance.registerTooltip({ html: true });
        balance.initializeCashService();
    });
});
