interface Window {
    watchlistAccountData: any;
    reloadMyWatchlistDataTable: Function;
}

class ConfirmAddToWatchlist {
    private $: JQueryStatic;
    private topWindow: any;

    constructor(
        $: JQueryStatic,
        topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;

    }

    public init() {
        this.$.blockUI();
        this.bindEvents();
        this.$.unblockUI();
    }

    bindEvents(): void {

        $("#btnCancel").on("click touchend", () => {
            this.topWindow["popupManager"].close();
        });

        $("#btnSubmit").on("click touchend", () => {
            const newlyAddedAccounts = this.$('#NewlyAddedAccounts').val();
            const watchlistPage = this.$('#member-info-url').val() + `MyWatchlistAccounts`;
            const params = `?IsOpenFromProblemAccount=true&custIds=${newlyAddedAccounts}`;

            const mainFrame = this.topWindow.$('#main');
            mainFrame.attr('src', watchlistPage + params);
        });

        const toggleItem = ["already", "newly"];
        toggleItem.forEach((item) => {

            this.$(`#${item}-more.more-link`).click(() => {
                this.$(`#${item}-more.more-link`).parent().prev().toggle();
                this.$(`#${item}-more.more-link`).prev().toggle();
                this.$(`#${item}-more.more-link`).toggle();
                this.$(`#${item}-less.less-content`).toggle();
                return false;
            });

            this.$(`#${item}-less.less-content`).click(() => {
                this.$(`#${item}-less.less-content`).toggle();
                this.$(`#${item}-more.more-link`).parent().prev().toggle();
                this.$(`#${item}-more.more-link`).prev().toggle();
                this.$(`#${item}-more.more-link`).toggle();
                return false;
            });
        });
    }
}

$(function () {
    new ConfirmAddToWatchlist($, window.top).init();
});