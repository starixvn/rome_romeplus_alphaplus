interface Window {
    CompactFeedbackManager: {
        init: ($like, $dislike, functionId: number, extraInfo: string) => void;
    };
}

class CompactFeedbackComponent {
    private readonly boxIconTitleId = '#box-icon-title';

    init(currentFormData?: any) {
        const $boxIconTitle = $(this.boxIconTitleId);
        if (!$boxIconTitle.length) {
            return;
        }

        if (!top.CompactFeedbackManager) {
            this.toggleShowHide(false);
            return;
        }

        currentFormData =
            currentFormData ??
            (this.isJQueryForm(currentFormData) ? this.getFormData(currentFormData) : currentFormData);
        const extraInfo = currentFormData ? JSON.stringify(currentFormData) : '';

        this.registerEvents($boxIconTitle, extraInfo);
    }

    private registerEvents($boxIconTitle: JQuery, extraInfo: string) {
        // Debounced window events for better performance
        $(window)
            .off('load.compactFeedback resize.compactFeedback')
            .on('load.compactFeedback resize.compactFeedback', () =>
                setTimeout(() => this.setBoxIconTitleClass($boxIconTitle), 100)
            );
        top.CompactFeedbackManager.init($('#icon-like'), $('#icon-dislike'), $boxIconTitle.data('fid'), extraInfo);
    }

    private setBoxIconTitleClass($boxIconTitle: JQuery) {
        const elementOffset = $boxIconTitle.offset() || { left: 0 };
        const elementWidth = $boxIconTitle.outerWidth() || 0;
        const windowWidth = $(window).width() || 0;
        const scrollLeft = $(window).scrollLeft() || 0;
        const marginRight = 150;
        const marginLeft = 100;
        const screenLeft = elementOffset.left - scrollLeft;
        const screenRight = windowWidth - (screenLeft + elementWidth);

        // Remove existing position classes and add appropriate one
        $boxIconTitle.removeClass('left right').addClass(screenLeft < marginLeft ? 'left' : 'right');
        if (screenLeft < marginLeft) {
            $boxIconTitle.addClass('left');
        } else if (screenRight < marginRight) {
            $boxIconTitle.addClass('right');
        }
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
        const $parent = $boxIconTitle.parent('.form-group');
        ($parent.length > 0 ? $parent : $boxIconTitle).toggleClass('hide', !isShow);
    }
}
