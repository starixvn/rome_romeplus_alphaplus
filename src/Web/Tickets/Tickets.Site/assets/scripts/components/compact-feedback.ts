interface Window {
    bootboxAlert: (message: string) => void;
    afterDislikeSubmitCallback: () => void;
}

class CompactFeedbackComponent {
    private readonly boxIconTitleId = '#box-icon-title';
    private extraInfo = '';

    init(currentFormData?: any) {
        const $boxIconTitle = $(this.boxIconTitleId);
        if (!$boxIconTitle.length) {
            return;
        }

        if (!top.alpha) {
            this.toggleShowHide(false);
            return;
        }

        if (currentFormData) {
            const formData = this.isJQueryForm(currentFormData) ? this.getFormData(currentFormData) : currentFormData;
            this.setExtraInfo(formData);
        }

        this.registerEvents($boxIconTitle);
    }

    private registerEvents($boxIconTitle: JQuery) {
        const $like = $('#icon-like').removeClass('active hide finished');
        const $dislike = $('#icon-dislike').removeClass('active hide');
        const isBootstrapTooltip = $like.data('toggle') === 'tooltip';
        if (isBootstrapTooltip) {
            $like.tooltip();
            $dislike.tooltip();
        } else {
            // Debounced window events for better performance
            $(window)
                .off('load.compactFeedback resize.compactFeedback')
                .on('load.compactFeedback resize.compactFeedback', () =>
                    setTimeout(() => this.setBoxIconTitleClass($boxIconTitle), 100)
                );
        }

        $like.off('click').one('click', () => this.handleLikeClick($boxIconTitle.data('fid'), $like, $dislike));
        $dislike.off('click').on('click', () => this.handleDislikeClick($boxIconTitle.data('fid'), $like, $dislike));
    }

    private setBoxIconTitleClass($boxIconTitle: JQuery) {
        const elementOffset = $boxIconTitle.offset() || { left: 0 };
        const elementWidth = $boxIconTitle.outerWidth() || 0;
        const windowWidth = $(window).width() || 0;
        const scrollLeft = $(window).scrollLeft() || 0;

        const screenLeft = elementOffset.left - scrollLeft;
        const screenRight = windowWidth - (screenLeft + elementWidth);

        // Remove existing position classes and add appropriate one
        $boxIconTitle.removeClass('left right').addClass(screenLeft < 100 ? 'left' : 'right');
        if (screenLeft < 100) {
            $boxIconTitle.addClass('left');
        } else if (screenRight < 100) {
            $boxIconTitle.addClass('right');
        }
    }

    private handleLikeClick(functionId: number, $like: JQuery, $dislike: JQuery) {
        $.post(
            `${top.alpha.urls.messageRootPath}Feedback/Add`,
            {
                functionId,
                rate: 5,
                extraInfo: this.extraInfo
            },
            (result) => {
                if (result.Code) {
                    top.bootboxAlert(result.Message);
                } else {
                    $like.addClass('active');
                    setTimeout(() => $like.addClass('finished'), 1000);
                    $dislike.addClass('hide');
                }
            }
        );
        top.popupManager.close();
    }

    private handleDislikeClick(functionId: number, $like: JQuery, $dislike: JQuery) {
        top.afterDislikeSubmitCallback = () => {
            $like.addClass('hide');
            $dislike.addClass('active').off('click');
            setTimeout(() => top.popupManager.close(), 3000);
        };

        top.popupManager.open(
            `${top.alpha.urls.messageRootPath}Feedback/Dislike?id=${functionId}&extraInfo=${this.extraInfo}`,
            undefined,
            440,
            260,
            null,
            null,
            null,
            null,
            $like.position().top + 195,
            null,
            {
                customClass: 'feedback-modal'
            }
        );
    }

    private isJQueryForm(value: JQuery) {
        return value instanceof jQuery && value.length > 0 && value.get(0).tagName.toUpperCase() === 'FORM';
    }

    private getFormData($form: JQuery) {
        const formData: any = {};
        const formDataArray = $form.serializeArray();
        $.each(formDataArray, function () {
            if (formData[this.name] === undefined) {
                formData[this.name] = this.value || '';
            }
        });

        return formData;
    }

    toggleShowHide(isShow: boolean) {
        const $boxIconTitle = $(this.boxIconTitleId);
        const $target = $boxIconTitle.parent('.form-group').length
            ? $boxIconTitle.parent('.form-group')
            : $boxIconTitle;
        $target.toggleClass('hide', !isShow);
    }

    setExtraInfo(currentFormData: any) {
        this.extraInfo = currentFormData ? JSON.stringify(currentFormData) : '';
    }
}
