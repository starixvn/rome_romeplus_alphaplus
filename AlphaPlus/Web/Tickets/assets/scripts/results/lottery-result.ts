$(function () {
    const $linkLotteryResult = $('#result-link');
    const url = $linkLotteryResult.attr('href');

    $linkLotteryResult.click(function (e) {
        e.preventDefault();
        window.open(
            url,
            'Lottery_Result',
            'width=900,height=700,scrollbars=1,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0'
        );
    });
});
