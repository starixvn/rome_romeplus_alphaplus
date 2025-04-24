class Popup {
    topWindow: any;
    protected $: JQueryStatic;
    protected domElements = {
        announcementModal: null,
        announcementIframe: null,
        announcementContent: null
    };

    constructor($: JQueryStatic, topWindow: any) {
        this.$ = $;
        this.topWindow = topWindow;
    }

    init = () => {
        this.cacheElements();

        if (this.hasEmptyAnnouncementData()) {
            return;
        }

        this.displayAnnouncementModal();
    };

    cacheElements = () => {
        this.domElements.announcementModal = this.topWindow.$('#announcement-modal');
        this.domElements.announcementIframe = this.topWindow.$('#annoucement-iframe');
        this.domElements.announcementContent = this.$('#popupContent');
    };

    hasEmptyAnnouncementData = (): boolean => {
        return this.domElements.announcementContent.length == 0;
    };

    displayAnnouncementModal = () => {
        this.calculateIframeHeight();
        this.domElements.announcementModal
            .modal({ show: true, backdrop: false })
            .tinyDraggable({ exclude: 'button.icon.icon-close.close, .modal-body, p, a' });
    };


    calculateIframeHeight = () => {
        let announcementContent = this.domElements.announcementIframe;
        announcementContent[0].onload = () => {
            let contentHeight = announcementContent.contents().find("body").height();
            let iframeHeightPadding = 10;
            let iframeHeight = contentHeight + iframeHeightPadding;
            announcementContent.css("height", iframeHeight);
        };
    };    
}

$(document).ready(() => {

    const announcement = new Popup($, window.top);

    announcement.init();
})