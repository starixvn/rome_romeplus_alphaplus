$(document).ready(function () {
    // Initialize carousel with enhanced options
    $('#bannerCarousel').carousel({
        interval: 6000, // Auto-advance every 6 seconds
        pause: 'hover', // Pause on hover
        wrap: true,     // Loop continuously
        keyboard: true  // Enable keyboard navigation
    });

    // Add touch/swipe support for mobile
    $('#bannerCarousel').on('touchstart', function (e) {
        var touch = (e.originalEvent as TouchEvent).touches[0];
        $(this).data('touchStartX', touch.clientX);
    });

    $('#bannerCarousel').on('touchend', function (e) {
        var touch = (e.originalEvent as TouchEvent).changedTouches[0];
        var startX = $(this).data('touchStartX');
        var endX = touch.clientX;
        var diff = startX - endX;

        if (Math.abs(diff) > 50) { // Minimum swipe distance
            if (diff > 0) {
                $(this).carousel('next');
            } else {
                $(this).carousel('prev');
            }
        }
    });

    // Add smooth transitions
    $('#bannerCarousel').on('slide.bs.carousel', function () {
        $(this).find('.item').css('opacity', '0.8');
    });

    $('#bannerCarousel').on('slid.bs.carousel', function () {
        $(this).find('.item.active').css('opacity', '1');
    });

    // Auto-hide loading indicator after 3 seconds
    setTimeout(function () {
        hideLoading();
    }, 3000);
});

function hideLoading() {
    $('#loadingIndicator').fadeOut(500);
}

function handleImageError(img) {
    // Handle image loading errors gracefully
    console.warn('Image failed to load:', img.src);
    img.style.display = 'none';
    hideLoading();
}

// Add keyboard navigation
$(document).keydown(function (e) {
    if (e.keyCode === 37) { // Left arrow
        $('#bannerCarousel').carousel('prev');
    } else if (e.keyCode === 39) { // Right arrow
        $('#bannerCarousel').carousel('next');
    }
});

function openVideo(url) {
    // Check if URL is valid and not empty
    if (!url || url.trim() === '') {
        console.warn('Video URL is empty or invalid');
        return;
    }
    
    // Check if URL has valid video extension
    const validExtensions = ['.mp4', '.webm', '.ogg', '.avi', '.mov'];
    const hasValidExtension = validExtensions.some(ext => url.toLowerCase().includes(ext));
    
    if (!hasValidExtension) {
        console.warn('Video URL does not have a valid video extension:', url);
        return;
    }
             
    const video = "<div class='wrapper-video'><div class='video-container'><div class='icon-close' onclick='closeVideo()'></div><video controls autoplay><source src='"+url+"' type='video/mp4'></video></div></div>";
    document.body.style.overflow = 'hidden';
    document.body.innerHTML += video;
}

function closeVideo() {
    const wrapperVideo = document.querySelector('.wrapper-video');
    wrapperVideo.remove();
    document.body.style.overflow = 'auto';
}