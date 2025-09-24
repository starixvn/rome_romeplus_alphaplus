interface Window {
    afterDislikeSubmitCallback: Function | null;
    ageWndDislikePopup: {
        close: () => void;
        onClosing: () => void;
    };
    CompactFeedbackManager: {
        init: (currentWindow, $container, $like, $dislike, formData) => void;
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

    function init(currentWindow, $container, $like, $dislike, formData) {
        // Debounced window events for better performance
        const $currentWindow = $(currentWindow);
        $currentWindow
            .off('load.compactFeedback resize.compactFeedback')
            .on('load.compactFeedback resize.compactFeedback', () =>
                setTimeout(() => setBoxIconTitleClass($currentWindow, $container), 100)
            );

        const functionId = $container.data('fid');
        let extraInfo = '';
        if (formData) {
            formData = isJQueryForm(formData) ? getFormData(formData) : formData;
            extraInfo = JSON.stringify(formData);
        }

        $like.removeClass('active hide finished').off('click');
        $like.one('click', () => handleLikeClick($like, $dislike, functionId, extraInfo));

        $dislike.removeClass('active hide').off('click');
        $dislike.on('click', () => handleDislikeClick($like, $dislike, functionId, extraInfo));
    }

    function setBoxIconTitleClass($currentWindow, $container) {
        const elementOffset = $container.offset() || { left: 0 };
        const elementWidth = $container.outerWidth() || 0;
        const windowWidth = $currentWindow.width() || 0;
        const scrollLeft = $currentWindow.scrollLeft() || 0;
        const marginRight = 150;
        const marginLeft = 100;
        const screenLeft = elementOffset.left - scrollLeft;
        const screenRight = windowWidth - (screenLeft + elementWidth);

        // Remove existing position classes and add appropriate one
        $container.removeClass('left right').addClass(screenLeft < marginLeft ? 'left' : 'right');
        if (screenLeft < marginLeft) {
            $container.addClass('left');
        } else if (screenRight < marginRight) {
            $container.addClass('right');
        }
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
        return false;
    }

    function setAfterDislikeSubmitCallback(value: Function | null) {
        window.afterDislikeSubmitCallback = value;
    }

    function isJQueryForm(value) {
        return value.length > 0 && value.get(0).tagName.toUpperCase() === 'FORM';
    }

    function getFormData($form) {
        const formData = {};
        const formDataArray = $form.serializeArray();
        $.each(formDataArray, function () {
            if (formData[this.name] === undefined) {
                formData[this.name] = this.value || '';
            }
        });

        return formData;
    }

    return { init, closeDislikePopup };
})();
