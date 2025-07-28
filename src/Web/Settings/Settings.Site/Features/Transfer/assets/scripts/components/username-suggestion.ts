interface JQuery {
    typeahead(options: any): JQuery;
}

interface UsernameSuggestionOption {
    selectorId?: string;
    url: string;
    directDownlines?: boolean;
    isAccInfo?: boolean;
    onBlur?: Function;
    excludeCashMember?: boolean;
}

class UsernameSuggestionComponent {
    private $usernameInput: JQuery;

    init(options: UsernameSuggestionOption) {
        this.$usernameInput = $('#' + (options.selectorId || 'txt-username'));
        const queryString = this.buildQueryString(options);
        const urlAndQueryString = `${options.url}?${queryString}${queryString.length ? '&' : ''}`;
        this.$usernameInput.typeahead({
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
        });

        if (options.onBlur) {
            let hitEnterTextboxUsername = false;
            this.$usernameInput
                .keypress((e) => {
                    const key = e.which || e.keyCode;
                    if (key === 13) {
                        hitEnterTextboxUsername = true;
                        options.onBlur && options.onBlur();
                    }
                })
                .blur(() => {
                    if (hitEnterTextboxUsername) {
                        hitEnterTextboxUsername = false;
                    } else {
                        const visible = this.$usernameInput.next('.typeahead').is(':visible');
                        !visible && options.onBlur && options.onBlur();
                    }
                });
        }
    }

    private buildQueryString(options: UsernameSuggestionOption) {
        const queryString = [];

        if (options.directDownlines) {
            queryString.push(`directdownlines=${options.directDownlines}`);
        }

        if (!options.isAccInfo) {
            queryString.push(`isaccinfo=${options.isAccInfo}`);
        }

        return queryString.length === 0 ? '' : queryString.join('&');
    }

    getUsername() {
        if (this.$usernameInput.length) {
            return $.trim(this.$usernameInput.val());
        }
    }

    resetUsername() {
        this.$usernameInput.val('');
    }
}
