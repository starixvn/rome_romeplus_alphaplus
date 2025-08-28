interface Window {
    afterDislikeSubmitCallback: Function | null;
    ageWndDislikePopup: {
        close: () => void;
        onClosing: () => void;
    };
    CompactFeedbackManager: {
        init: ($like, $dislike, functionId: number, extraInfo: string) => void;
        closeDislikePopup: () => void;
    };
}

// Use a single global namespace object
window.CompactFeedbackManager = (function () {
    const DelayCloseDislikePopupInMilliseconds = 3000;
    const DislikePopupPaddingTop = 195;
    const DislikePopupWidth = 440;
    const DislikePopupHeight = 260;
    const FeedbackRootPath = `${alpha.urls.messageRootPath}Feedback`;

    function init($like, $dislike, functionId: number, extraInfo: string) {
        $like.removeClass('active hide finished').off('click');
        $like.one('click', () => handleLikeClick($like, $dislike, functionId, extraInfo));

        $dislike.removeClass('active hide').off('click');
        $dislike.on('click', () => handleDislikeClick($like, $dislike, functionId, extraInfo));
    }

    function handleLikeClick($like, $dislike, functionId: number, extraInfo: string) {
        $.post(`${FeedbackRootPath}/Add`, { functionId, rate: 5, extraInfo }, (result) => {
            if (result.Code) {
                top.bootboxAlert(result.Message);
            } else {
                setTimeout(() => $like.addClass('finished'), 1000);
                $like.addClass('active');
                $dislike.addClass('hide');
            }
        });
        closeDislikePopupInstance();
    }

    function handleDislikeClick($like, $dislike, functionId: number, extraInfo: string) {
        setAfterDislikeSubmitCallback(() => {
            $like.addClass('hide');
            $dislike.addClass('active').off('click');
            setTimeout(() => closeDislikePopup(), DelayCloseDislikePopupInMilliseconds);
        });

        openDislikePopup(functionId, extraInfo, $like.position().top + DislikePopupPaddingTop);
    }

    function openDislikePopup(functionId: number, extraInfo: string, topx: number) {
        const handler = getDislikePopupInstance();
        handler.onClosing = () => {
            setDislikePopupInstance(null);
            setAfterDislikeSubmitCallback(null);
        };
        setDislikePopupInstance(handler);
        popupManager.openWithHanlder(
            handler,
            `${FeedbackRootPath}/Dislike?id=${functionId}&extraInfo=${extraInfo}`,
            undefined,
            DislikePopupWidth,
            DislikePopupHeight,
            undefined,
            undefined,
            undefined,
            undefined,
            topx,
            undefined,
            { customClass: 'feedback-modal' }
        );
    }

    function closeDislikePopup() {
        // fallback in case the popup reference is lost (site-reports)
        closeDislikePopupInstance() || popupManager.close();
    }

    function getDislikePopupInstance() {
        return window.ageWndDislikePopup ? window.ageWndDislikePopup : window.ageWnd.createInstance();
    }

    function setDislikePopupInstance(instance) {
        window.ageWndDislikePopup = instance;
    }

    function closeDislikePopupInstance() {
        if (window.ageWndDislikePopup) {
            window.ageWndDislikePopup.close();
            return true;
        }
    }

    function setAfterDislikeSubmitCallback(value: Function | null) {
        window.afterDislikeSubmitCallback = value;
    }

    return { init, closeDislikePopup };
})();
