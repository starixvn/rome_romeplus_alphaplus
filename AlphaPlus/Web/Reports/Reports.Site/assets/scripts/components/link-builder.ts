class LinkBuilderOption {
    linkClasses: string[];

    constructor(linkClasses: string[] = ['a.breadcrumb-link,a.downline-link']) {
        this.linkClasses = linkClasses;
    }
}

class LinkBuilderComponent {
    options = new LinkBuilderOption();

    render = (options: LinkBuilderOption = new LinkBuilderOption()) => {
        this.options = $.extend({}, this.options, options);
    };

    updateParam = (name: string, value: string) => {
        this.options.linkClasses.forEach((className: string) => {
            const $linkClass = $(className);

            $linkClass.each(function (_, element: Element) {
                const $link = $(element);
                let currentUrl = $link.attr('href');
                currentUrl = alphaReports.replaceQueryStringValue(currentUrl, name, value);

                $link.attr('href', currentUrl);
            });
        });
    };
}
