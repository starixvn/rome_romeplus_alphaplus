class IconInformation {
    init() {
        $('#icon-information').on('click', this.showGuidelines);
    }

    showGuidelines() {
        const iconInformation = $('#icon-information');
        const guideNote = $('#info-note');
        guideNote.modal({ show: true, backdrop: false });

        const iconHelpPosition = iconInformation.position();
        guideNote.css({
            bottom: 'auto',
            top: iconHelpPosition.top + 40
        });
    }
}

$(function () {
    new IconInformation().init();
});
