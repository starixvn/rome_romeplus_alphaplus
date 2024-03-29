/// <reference path="../core.ts" />

interface JQuery {
    multiselect(option: any): void;
}

class LeagueMultiSelectionComponent {
    $dropdownLeague: JQuery;
    $hiddenInput: JQuery;
    $isUserSelectsAllLeagues: JQuery;
    $multiSelectButton: JQuery;

    isReady = false;

    render = () => {
        this.$dropdownLeague = $('#dropdown-league');
        this.$hiddenInput = $('#league-ids');
        this.$isUserSelectsAllLeagues = $('#IsUserSelectsAllLeagues');
        const hasAnyLeague = this.$dropdownLeague.data('hasdata');

        this.$dropdownLeague.multiselect({
            maxHeight: 300,
            numberDisplayed: 1,
            nonSelectedText: hasAnyLeague
                ? this.$dropdownLeague.data('label-selectleagues')
                : this.$dropdownLeague.data('label-nodata'),
            nSelectedText: ` ${this.$dropdownLeague.data('label-selected')}`,
            allSelectedText: this.$dropdownLeague.data('label-allleagues'),
            includeSelectAllOption: hasAnyLeague,
            onChange: () => this.isReady && this.onChange(),
            onSelectAll: (checked) => this.isReady && this.onSelectAll(checked),
            language: this.$dropdownLeague.data('language'),
            enableCaseInsensitiveFiltering: true
        });

        this.$multiSelectButton = this.$dropdownLeague.parent().find('.multiselect.btn:not(#btn-loadleague)');
        this.isReady = true;

        return this.$dropdownLeague;
    };

    private onChange() {
        this.validateLeagueIds();
        const leagueIds = this.getLeagueIds();
        this.$hiddenInput.val(leagueIds.length === 0 ? '-1' : leagueIds);
        this.$isUserSelectsAllLeagues.val('false');
    }

    private onSelectAll(checked: boolean) {
        checked ? this.$multiSelectButton.removeClass('error') : this.$multiSelectButton.addClass('error');
        this.$hiddenInput.val(checked ? '' : '-1');
        this.$isUserSelectsAllLeagues.val(checked.toString());
    }

    validateLeagueIds = (): boolean => {
        const isValid = this.$dropdownLeague.find('option:selected').length > 0;
        isValid ? this.$multiSelectButton.removeClass('error') : this.$multiSelectButton.addClass('error');

        return isValid;
    };

    getLeagueIds = (): string => {
        const selected = [];
        this.$dropdownLeague.find('option:selected').each(function (_, item) {
            selected.push($(item).val());
        });

        return selected.join(',');
    };
}
