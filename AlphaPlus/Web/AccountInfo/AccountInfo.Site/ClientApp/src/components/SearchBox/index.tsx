import React, { useState } from 'react';
import AutoSuggest from 'react-autosuggest';
import { useTranslation } from 'react-i18next';

import { useGlobalContext } from '../../context/global-context';
import UsernameSuggestion from '../../models/username-suggestion';
import SettingsService from '../../services/settings-service';
import styles from './SearchBox.module.scss';

export default function SearchBox() {
    const { t } = useTranslation();
    const { username, setUsername, getCustomerInfo } = useGlobalContext();
    const [suggestions, setSuggestions] = useState<UsernameSuggestion[]>([]);

    const getSuggestionValue = (suggestion: UsernameSuggestion) => suggestion.UserName;

    const renderSuggestion = (suggestion: UsernameSuggestion) => {
        const { UserName, UserName2, FirstName, LastName } = suggestion;
        return (
            <div id={`${UserName}-suggestion`}>
                {UserName} {UserName2 !== null && UserName2 !== '' && `(${UserName2})`}
                <div>
                    {FirstName} {LastName}
                </div>
            </div>
        );
    };

    const onSuggestionsFetchRequested = ({ value }: { value: string }) => {
        SettingsService.GetUsernameSuggestion(value).then((results) => setSuggestions(results));
    };

    const onSuggestionsClearRequested = () => {
        setSuggestions([]);
    };

    const onSuggestionSelected = (e: React.FormEvent<any>, { suggestionValue }: { suggestionValue: string }) => {
        getCustomerInfo?.(suggestionValue);
    };

    const onChange = (e: any, { newValue }: { newValue: string }) => {
        setUsername?.(newValue);
    };

    const shouldRenderSuggestions = (value: string) => {
        return value.trim().length > 2;
    };

    const handleSearchClick = () => {
        if (username) {
            getCustomerInfo?.(username);
            onSuggestionsClearRequested();
            hideKeyboard();
        }
    };

    const hideKeyboard = () => {
        $('.txt-search').blur();
    };

    const handleSearchKeyPress = (e: React.KeyboardEvent<HTMLElement>) => {
        if (e.key === 'Enter') {
            handleSearchClick();
        }
    };

    const inputProps = {
        type: 'search',
        className: styles['txt-search'],
        onChange: onChange,
        onKeyPress: handleSearchKeyPress,
        placeholder: t('userNamePlaceHolder'),
        // onFocus: zoomDisable,
        // onBlur: zoomEnable,
        value: username ?? ''
    };

    return (
        <div className={styles.container}>
            <AutoSuggest
                suggestions={suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={onSuggestionsClearRequested}
                onSuggestionSelected={onSuggestionSelected}
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                shouldRenderSuggestions={shouldRenderSuggestions}
                focusInputOnSuggestionClick={false}
                inputProps={inputProps}
            />
            <span className={`icon-search ${styles['icon-search']}`} onClick={handleSearchClick} />
        </div>
    );
}
