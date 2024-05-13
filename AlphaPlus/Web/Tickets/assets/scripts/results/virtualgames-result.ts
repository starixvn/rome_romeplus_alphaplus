$(function () {
    const $linkVirtualGamesResult = $('#result-link');
    const url = $linkVirtualGamesResult.attr('href');

    $linkVirtualGamesResult.click(function (e) {
        e.preventDefault();
        window.open(
            url,
            'VirtualGames_Result',
            'width=900,height=700,scrollbars=1,directories=0,titlebar=0,toolbar=0,location=0,status=0,menubar=0'
        );
    });
});
