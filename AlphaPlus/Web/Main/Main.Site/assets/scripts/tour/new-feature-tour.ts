class NewFeatureTour extends BaseTourController {
    protected maxPage = 3;

    init() {
        super.init();
        this.$('#lnk-new-feature').attr('href', this.$('#NewFeatureUrl').val());
    }
}

$(() => new NewFeatureTour($).init());
