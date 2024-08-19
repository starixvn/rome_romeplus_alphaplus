class UsernameSuggestionOption {
    constructor(
        public url: string,
        public onBlur = Function(),
        public directDownline = false,
        public isAccInfo = false
    ) {
        this.url = url;
        this.onBlur = onBlur;
        this.directDownline = directDownline;
        this.isAccInfo = isAccInfo;
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
                const nickname = item.Username2 ? ` (${item.Username2})` : '';
                return `${item.Username}${nickname}#${item.FirstName} ${item.LastName}`;
            },
            autoSelect: false,
            afterSelect: (item) => this.$usernameInput.val(item.Username),
            delay: 200
        };

        let hitEnterTextboxUsername = false;
        this.$usernameInput
            .typeahead(typeaheadOptions)
            .blur(() => {
                if (hitEnterTextboxUsername) {
                    hitEnterTextboxUsername = false;
                } else {
                    const visible = this.$usernameInput.next('.typeahead').is(':visible');
                    !visible && options.onBlur();
                }
            })
            .keypress(function (e) {
                const key = e.which || e.keyCode;
                if (key === 13) {
                    hitEnterTextboxUsername = true;
                    options.onBlur();
                }
            });
    }

    private buildQueryString(options: UsernameSuggestionOption) {
        const queryString = [];

        if (options.directDownline) {
            queryString.push(`directDownline=${options.directDownline}`);
        }

        if (options.isAccInfo) {
            queryString.push(`isaccinfo=${options.isAccInfo}`);
        }

        return queryString.length === 0 ? '' : queryString.join('&');
    }

    getUsername() {
        return $.trim(this.$usernameInput.val());
    }

    clearUsername() {
        this.$usernameInput.val('');
    }
}
