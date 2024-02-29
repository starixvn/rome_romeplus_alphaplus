import React, { useState } from 'react';
import AutoSuggest from 'react-autosuggest';
import { useTranslation } from 'react-i18next';

import styles from './SearchBox.module.scss';

export const getSuggestionValue = (suggestion: any) => suggestion.UserName;

export const renderSuggestion = (suggestion: any) => {
    return (
        <div id={`${suggestion.UserName}-suggestion`}>
            {suggestion.UserName}{' '}
            {suggestion.UserName2 !== null && suggestion.UserName2 !== '' && `(${suggestion.UserName2})`}
            <div>
                {suggestion.FirstName} {suggestion.LastName}
            </div>
        </div>
    );
};

export default function SearchBox() {
    const { t } = useTranslation();
    const [username, setUsername] = useState('');
    const [suggestions, setSuggestions] = useState([]);

    const onSuggestionsFetchRequested = ({ value }: { value: string }) => {
        setSuggestions([]);
    };

    const onSuggestionsClearRequested = () => {
        setSuggestions([]);
    };

    const onChange = (e: any, { newValue }: { newValue: string }) => {
        setUsername(newValue);
    };

    const inputProps = {
        type: 'search',
        className: styles['txt-search'],
        onChange: onChange,
        // onKeyPress: this.handleSearchKeyPress,
        placeholder: t('userNamePlaceHolder'),
        // onFocus: zoomDisable,
        // onBlur: zoomEnable,
        value: username
    };

    return (
        <div className={styles.container}>
            <AutoSuggest
                suggestions={suggestions}
                onSuggestionsFetchRequested={onSuggestionsFetchRequested}
                onSuggestionsClearRequested={onSuggestionsClearRequested}
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                inputProps={inputProps}
            />
            <span className={styles['icon-search']} />
        </div>
    );
}
