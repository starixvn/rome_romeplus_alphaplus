interface Window {
    CompactFeedbackManager: {
        init: (window, $boxIconTitle, $like, $dislike, formData) => void;
    };
}

class CompactFeedbackComponent {
    private readonly boxIconTitleId = '#box-icon-title';

    init(currentFormData = undefined, isShow = undefined) {
        const $boxIconTitle = $(this.boxIconTitleId);
        if (!$boxIconTitle.length) {
            return;
        }

        const isShowTypeofBoolean = typeof isShow === 'boolean';
        if (!top.CompactFeedbackManager || (isShowTypeofBoolean && !isShow)) {
            this.toggleShowHide(false, $boxIconTitle);
            return;
        }

        top.CompactFeedbackManager.init(
            window,
            $boxIconTitle,
            $boxIconTitle.find('#icon-like'),
            $boxIconTitle.find('#icon-dislike'),
            currentFormData
        );

        isShowTypeofBoolean && this.toggleShowHide(isShow, $boxIconTitle);
    }

    toggleShowHide(isShow: boolean, $boxIconTitle = undefined) {
        $boxIconTitle = $boxIconTitle ?? $(this.boxIconTitleId);
        const $parent = $boxIconTitle.parent('.form-group');
        ($parent.length > 0 ? $parent : $boxIconTitle).toggleClass('hide', !isShow);
    }
}
