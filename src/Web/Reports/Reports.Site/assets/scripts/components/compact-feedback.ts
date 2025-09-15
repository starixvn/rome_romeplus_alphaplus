interface Window {
    CompactFeedbackManager: {
        init: (window, $boxIconTitle, $like, $dislike, formData) => void;
    };
}

class CompactFeebackComponent {
    init(currentFormData, isShow: boolean = undefined) {
        const $boxIconTitle = $('#box-icon-title');
        if (!$boxIconTitle.length) {
            return;
        }

        const isShowTypeofBoolean = typeof isShow === 'boolean';
        if (!top.CompactFeedbackManager || (isShowTypeofBoolean && !isShow)) {
            this.toggleShowHide($boxIconTitle, false);
            return;
        }

        const $iconLike = $boxIconTitle.find('#icon-like');
        $iconLike.data('toggle') === 'tooltip' && $iconLike.tooltip('destroy').tooltip();
        const $iconDislike = $boxIconTitle.find('#icon-dislike');
        $iconDislike.data('toggle') === 'tooltip' && $iconDislike.tooltip('destroy').tooltip();

        top.CompactFeedbackManager.init(window, $boxIconTitle, $iconLike, $iconDislike, currentFormData);
        isShowTypeofBoolean && this.toggleShowHide($boxIconTitle, isShow);
    }

    private toggleShowHide($boxIconTitle, isShow: boolean) {
        const $parent = $boxIconTitle.parent('.form-group');
        ($parent.length > 0 ? $parent : $boxIconTitle).toggleClass('hide', !isShow);
    }
}
