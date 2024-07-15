interface JQuery {
    typeahead(options: any): JQuery;
}

class UsernameSuggestionOption {
    constructor(
        public $usernameInput: JQuery,
        public url: string,
        public directDownlines = false,
        public isAccInfo = false
    ) {
        this.$usernameInput = $usernameInput;
        this.url = url;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
    }
}

class UsernameSuggestionComponent {
    init(options: UsernameSuggestionOption) {
        const queryString = this.buildQueryString(options);
        const urlAndQueryString = `${options.url}?${queryString}${queryString.length ? '&' : ''}`;

        options.$usernameInput.typeahead({
            source: function (query: string, process) {
                return $.get(`${urlAndQueryString}query=${query}`, process);
            },
            items: 10,
            minLength: 2,
            highlighter: function (displayText: string) {
                const parts = displayText.split('#');
                return `${parts[0]} <br/> ${parts[1]}`;
            },
            displayText: function (item) {
                const nickname = item.UserName2 ? ` (${item.UserName2})` : '';
                return `${item.UserName}${nickname}#${item.FirstName} ${item.LastName}`;
            },
            autoSelect: false,
            afterSelect: (item) => options.$usernameInput.val(item.UserName),
            delay: 200
        });
    }

    private buildQueryString(options: UsernameSuggestionOption) {
        const queryString = [];

        if (options.directDownlines) {
            queryString.push(`directdownlines=${options.directDownlines}`);
        }

        if (options.isAccInfo) {
            queryString.push(`isaccinfo=${options.isAccInfo}`);
        }

        return queryString.length === 0 ? '' : queryString.join('&');
    }
}
