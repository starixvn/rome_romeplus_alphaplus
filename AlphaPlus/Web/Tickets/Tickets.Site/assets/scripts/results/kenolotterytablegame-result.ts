$(function () {
    const $linkKenoLotteryTableGameResult = $('#result-link');
    const url = $linkKenoLotteryTableGameResult.attr('href');
    const productName = $linkKenoLotteryTableGameResult.data('productname');

    $linkKenoLotteryTableGameResult.click(function (event) {
        event.preventDefault();
        window.open(
            url,
            productName,
            'width=900,height=700,scrollbars=1,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0'
        );
    });
});
