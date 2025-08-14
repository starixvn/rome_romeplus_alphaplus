class Banner {
    private readonly $: JQueryStatic;
    private $bannerCarousel: JQuery;

    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init() {
        this.$bannerCarousel = this.$('#bannerCarousel');
        if (this.$bannerCarousel.length) {
            this.$bannerCarousel.carousel();
            this.$bannerCarousel.on('touchstart', function (e) {
                const touch = (e.originalEvent as TouchEvent).touches[0];
                $(this).data('touchStartX', touch.clientX);
            });

            this.$bannerCarousel.on('touchend', function (e) {
                const touch = (e.originalEvent as TouchEvent).changedTouches[0];
                const startX = $(this).data('touchStartX');
                const endX = touch.clientX;
                const diff = startX - endX;
                const minSwipeDistance = 50;

                if (Math.abs(diff) > minSwipeDistance) {
                    // Minimum swipe distance
                    if (diff > 0) {
                        $(this).carousel('next');
                    } else {
                        $(this).carousel('prev');
                    }
                }
            });

            // Add smooth transitions
            this.$bannerCarousel.on('slide.bs.carousel', function () {
                $(this).find('.item').css('opacity', '0.8');
            });

            this.$bannerCarousel.on('slid.bs.carousel', function () {
                $(this).find('.item.active').css('opacity', '1');
            });

            const self = this;
            this.$('.video-banner').on('click', function () {
                const targetUrl = $(this).data('target-url');
                self.openVideo(targetUrl);
            });
        }
    }

    openVideo(url: string) {
        // Check if URL is valid and not empty
        if (!url || url.trim() === '') {
            console.info('Video URL is empty or invalid');
            return;
        }

        // Check if URL has valid video extension
        const validExtensions = ['.mp4', '.webm', '.ogg', '.avi', '.mov'];
        const hasValidExtension = validExtensions.some((ext) => url.toLowerCase().indexOf(ext) !== -1);

        if (!hasValidExtension) {
            console.info('Video URL does not have a valid video extension:', url);
            return;
        }

        const video =
            "<div class='wrapper-video'><div class='video-container'><div class='icon-close' id='closeVideo'></div><video controls autoplay><source src='" +
            url +
            "' type='video/mp4'></video></div></div>";
        document.body.style.overflow = 'hidden';
        document.body.appendChild(this.$(video)[0]);

        this.$('#closeVideo').on('click', () => {
            this.closeVideo();
        });
    }

    closeVideo() {
        const wrapperVideo = document.querySelector('.wrapper-video');
        wrapperVideo.remove();
        document.body.style.overflow = 'auto';
    }
}

$(document).ready(() => {
    const banner = new Banner($);
    banner.init();
});
