class ProfilePictureTour extends BaseTourController {
    public init() {
        super.init();
        this.$('#lnk-profile-picture').attr('href', this.$('#ProfilePictureUrl').val());
    }
}

$(function () {
    new ProfilePictureTour($).init();
});
