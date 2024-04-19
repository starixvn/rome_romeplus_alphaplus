class UsernameSuggestionOption {
    constructor(
        public url: string,
        public onBlur = Function(),
        public directDownlines = false,
        public isAccInfo = false,
        public initialUsername = ''
    ) {
        this.url = url;
        this.onBlur = onBlur;
        this.directDownlines = directDownlines;
        this.isAccInfo = isAccInfo;
        this.initialUsername = initialUsername;
    }
}

class UsernameSuggestionComponent {
    private $usernameInput: JQuery;

    init(options: UsernameSuggestionOption) {
        this.$usernameInput = $('#txt-username-suggestion');
        const queryString = this.buildQueryString(options);
        const urlAndQueryString = `${options.url}?${queryString}${queryString.length ? '&' : ''}`;

        const typeaheadOptions = {
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
            afterSelect: (item) => this.$usernameInput.val(item.UserName),
            delay: 200
        };

        this.$usernameInput
            .typeahead(typeaheadOptions)
            .blur(() => {
                const visible = this.$usernameInput.next('.typeahead').is(':visible');
                !visible && options.onBlur();
            })
            .val(options.initialUsername);
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

    getUsername() {
        return $.trim(this.$usernameInput.val());
    }

    resetUsername() {
        this.$usernameInput.val('');
    }
}
