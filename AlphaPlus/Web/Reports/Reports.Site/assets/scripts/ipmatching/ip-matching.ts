/// <reference path="../core.ts" />

class DatePickerOption {
    constructor(public minDate: null, public allowInputToggle: boolean = false) {
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
}

class IPMatchingView extends BaseReport {
    constructor($: JQueryStatic, linkBuilderComponent: LinkBuilderComponent) {
        super($, linkBuilderComponent);
    }

    init() {
        this.$.blockUI();

        const options = new DatePickerOption(this.$('#MinDate').val(), true);
        this.$('#dpkContainer').datepicker(options).data('DateTimePicker').maxDate(this.$('#Today').val());

        this.$('.view-bet-list').click(() => this.$.blockUI());

        this.$.unblockUI();
    }
}

$(function () {
    new IPMatchingView($, new LinkBuilderComponent($)).init();
});
