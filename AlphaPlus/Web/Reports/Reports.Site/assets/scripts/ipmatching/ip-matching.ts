/// <reference path="../core.ts" />

class DatePickerOption {
    constructor(public minDate: null, public allowInputToggle: boolean = false) {
        this.minDate = minDate;
        this.allowInputToggle = allowInputToggle;
    }
}

class IPMatchingView extends BaseReport {
    constructor(linkBuilderComponent: LinkBuilderComponent) {
        super(linkBuilderComponent);
    }

    init() {
        $.blockUI();

        const options = new DatePickerOption($('#MinDate').val(), true);
        $('#dpkContainer').datepicker(options).data('DateTimePicker').maxDate($('#Today').val());

        $('.view-bet-list').click(() => $.blockUI());

        $.unblockUI();
    }
}

$(function () {
    new IPMatchingView(new LinkBuilderComponent()).init();
});
