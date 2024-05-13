class OutrightResultView {
    init() {
        const minResultDate = $('#MinDate').val();
        const maxResultDate = $('#Today').val();
        const dpkSettings = {
            minDate: minResultDate,
            allowInputToggle: true
        };
        const $fromDatePicker: any = $('#dpk-fromdate');
        const $toDatePicker: any = $('#dpk-todate');

        $fromDatePicker.datepicker(dpkSettings);
        $fromDatePicker.data('DateTimePicker').maxDate(maxResultDate);
        $fromDatePicker.on('dp.change', function (e) {
            $toDatePicker.data('DateTimePicker').minDate(e.date);
        });

        $toDatePicker.datepicker(dpkSettings);
        $toDatePicker.data('DateTimePicker').maxDate(maxResultDate);
        $toDatePicker.on('dp.change', function (e) {
            $fromDatePicker.data('DateTimePicker').maxDate(e.date);
        });

        const $ddlSports = $('#SportType');
        $ddlSports.select2();
        $ddlSports.off().on('change', function () {
            $('#frm-outright').submit();
        });
        const $ddlLeagues = $('#LeagueId');
        $ddlLeagues.select2();
        $ddlLeagues.off().on('change', function () {
            $('#frm-outright').submit();
        });

        $('.btn-other-day').click(function () {
            const date = $(this).attr('value');
            $fromDatePicker.data('DateTimePicker').date(date);
            $toDatePicker.data('DateTimePicker').date(date);
        });
    }
}

$(function () {
    new OutrightResultView().init();
});
