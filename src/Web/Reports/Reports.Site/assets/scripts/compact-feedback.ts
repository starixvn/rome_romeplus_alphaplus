class CompactFeebackView {
    initCompactFeedback(isShowCompactFeedback: boolean, initTooltip: boolean, currentFormData) {
        const $boxIconTitle = $('#box-icon-title');
        if (!$boxIconTitle.length) {
            return;
        }

        if (!top.alpha || !isShowCompactFeedback) {
            const $parentFormGroup = $boxIconTitle.parent('.form-group');
            $parentFormGroup.length ? $parentFormGroup.addClass('hide') : $boxIconTitle.addClass('hide');
            return;
        }

        const $like = $('#icon-like').removeClass('active hide finished');

        // Function to check and update position classes
        const updatePositionClass = () => {
            const elementOffset = $boxIconTitle.offset();
            const elementWidth = $boxIconTitle.outerWidth() || 0;
            const windowWidth = $(window).width() || 0;
            const scrollLeft = $(window).scrollLeft() || 0;
            
            const screenLeft = elementOffset?.left ? elementOffset.left - scrollLeft : 0;
            const screenRight = windowWidth - (screenLeft + elementWidth);           
            // Remove existing position classes
            $boxIconTitle.removeClass('left right');
            
            if (screenLeft < 100) {
                $boxIconTitle.addClass('left');
            }
            else if (screenRight < 100) {
                $boxIconTitle.addClass('right');
            }
        };
 
        // Update position after page fully loads
        $(window).off('load.compactFeedback').on('load.compactFeedback , resize.compactFeedback', () => {
            // Small delay to ensure all layout calculations are complete
            setTimeout(updatePositionClass, 100);
        });

        const $dislike = $('#icon-dislike').removeClass('active hide');
        if (initTooltip) {
            $like.tooltip();
            $dislike.tooltip();
        }

        this.registerEvents($boxIconTitle, $like, $dislike, currentFormData);
        $boxIconTitle.parent().removeClass('hide');
    }

    private registerEvents($boxIconTitle: JQuery, $like: JQuery, $dislike: JQuery, currentFormData) {
        $like.off('click').one('click', () => {
            $.post(`${top.alpha.urls.messageRootPath}Feedback/Add`, {
                functionId: $boxIconTitle.data('fid'),
                rate: 5,
                extraInfo: JSON.stringify(currentFormData)
            });
            top.popupManager.close();
            $like.addClass('active');
            setTimeout(() => $like.addClass('finished'), 1000);
            $dislike.addClass('hide');
        });

        $dislike.off('click').click(() => {
            top.afterDislikeSubmitCallback = () => {
                $like.addClass('hide');
                $dislike.addClass('active').off('click');
                setTimeout(() => top.popupManager.close(), 3000);
            };
            top.popupManager.open(
                `${top.alpha.urls.messageRootPath}Feedback/Dislike?id=${$boxIconTitle.data(
                    'fid'
                )}&extraInfo=${JSON.stringify(currentFormData)}`,
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
        });
    }
}
