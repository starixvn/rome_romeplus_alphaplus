class RebateAndRetentionTour extends BaseTourController {
    protected $: JQueryStatic;
    protected maxPage = 5;
}

$(() => {
    const rebateAndRetention = new RebateAndRetentionTour($);
    rebateAndRetention.init();
})