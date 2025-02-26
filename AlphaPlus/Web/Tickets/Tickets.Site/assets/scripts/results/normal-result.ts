var showSoccer5Result;

class NormalResultView {
    init() {
        const ua = window.navigator.userAgent;
        const msie = ua.indexOf('MSIE ');
        const isIe = msie > 0 ? parseInt(ua.substring(msie + 5, ua.indexOf('.', msie))) < 10 : false;

        this.registerEvents(isIe);
    }

    registerEvents(isIe: boolean) {
        const $dpkContainer: any = $('#dpkContainer');
        const $ddlSports = $('#SportId');
        const $ddlLeagues = $('#LeagueId');
        const $resultDetails = $('#result-details');
        const resultDetailsUrl = $('#hidden-resultDetailsUrl').attr('href');
        const $resultDetailTitle = $('#result-detailt-title');
        const $frmDetailsResult = $('#result-detail-frm');
        const $loadingImage = $('#loading-image');

        $dpkContainer
            .datepicker({
                minDate: this.parseDate($('#MinDate').val()),
                allowInputToggle: true
            })
            .data('DateTimePicker')
            .maxDate(this.parseDate($('#Today').val()));

        $dpkContainer.on('dp.change', function () {
            $.blockUI();
            $ddlSports.val('');
            $ddlLeagues.val('');
            $('#result-filter').submit();
        });

        $ddlSports.select2();
        $ddlLeagues.select2();

        $('.btn-other-day')
            .off('click.normalResult')
            .on('click.normalResult', function () {
                $.blockUI();
                $dpkContainer.data('DateTimePicker').date($(this).attr('value'));
                $ddlSports.val('');
                $ddlLeagues.val('');
            });

        $ddlSports.off('change.normalResult').on('change.normalResult', function () {
            $.blockUI();
            $ddlLeagues.val($ddlLeagues.find('option:first').val());
            $('#result-filter').submit();
        });

        $ddlLeagues.off('change.normalResult').on('change.normalResult', function () {
            $.blockUI();
            $('#result-filter').submit();
        });

        $resultDetails.off('hidden.bs.modal').on('hidden.bs.modal', function () {
            $frmDetailsResult.hide();
            $loadingImage.show();
        });

        $('#main-result')
            .off('click.normalResult', '.detail-link')
            .on('click.normalResult', '.detail-link', function () {
                const $this = $(this);
                const requestParams = {
                    matchId: $this.data('match-id'),
                    leagueId: $this.data('league-id'),
                    sportId: $('#SportId').val()
                };

                $frmDetailsResult.attr('src', `${resultDetailsUrl}?${$.param(requestParams)}`);
                $resultDetails.modal({});
            });

        $frmDetailsResult.off('load.normalResult').on('load.normalResult', function () {
            $loadingImage.hide();
            const $this = $(this);
            const detailTitle = $this.contents().find('title').html();
            $resultDetailTitle.html(detailTitle);

            $this.show();

            if (isIe) {
                const bodyWidth = $('body').width();
                $this.width(bodyWidth - 200);
                $resultDetails.modal({});
                $resultDetails.find('.modal-dialog').width(bodyWidth - 100);
            }
        });

        showSoccer5Result = function (matchId, balls) {
            const params = {
                SportTypeId: 164, // Happy 5
                BetTypeId: 3604, // Soccer 5
                MatchId: matchId,
                Balls: balls
            };

            const url = `${$('#hidden-resultSoccer5Url').attr('href')}?${$.param(params)}`;
            $frmDetailsResult.attr('src', url);
            $resultDetails.modal({});
        };
    }

    parseDate(dateString) {
        const dateStringArray = dateString.split('/');
        return new Date(+dateStringArray[2], +dateStringArray[0] - 1, +dateStringArray[1]);
    }
}

$(function () {
    new NormalResultView().init();
});
