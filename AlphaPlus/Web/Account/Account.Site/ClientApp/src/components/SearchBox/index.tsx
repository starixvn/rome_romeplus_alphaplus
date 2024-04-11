import React, { useState } from 'react';
import AutoSuggest from 'react-autosuggest';
import { useTranslation } from 'react-i18next';

import { Base_URL } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import styles from './SearchBox.module.scss';

export const getSuggestionValue = (suggestion: any) => suggestion.UserName;

export const renderSuggestion = (suggestion: any) => {
    return (
        <div id={`${suggestion.userName}-suggestion`}>
            {suggestion.userName}{' '}
            {suggestion.userName2 !== null && suggestion.userName2 !== '' && `(${suggestion.userName2})`}
            <div>
                {suggestion.firstName} {suggestion.lastName}
            </div>
        </div>
    );
};

export default function SearchBox() {
    const { t } = useTranslation();
    const { username, setUsername } = useGlobalContext();
    const [suggestions, setSuggestions] = useState([]);

    const onSuggestionsFetchRequested = ({ value }: { value: string }) => {
        fetch(`${Base_URL}Account/GetUsernamesSuggestion?query=${value}`)
            .then((response) => response.json())
            .then((data) => setSuggestions(data));
    };

    const onSuggestionsClearRequested = () => {
        setSuggestions([]);
    };

    const onChange = (e: any, { newValue }: { newValue: string }) => {
        setUsername?.(newValue);
    };

    const shouldRenderSuggestions = (value: string) => {
        return value.trim().length > 2;
    };

    const handleSearchClick = () => {
        if (username) {
            onSuggestionsClearRequested();
        }
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
                getSuggestionValue={getSuggestionValue}
                renderSuggestion={renderSuggestion}
                shouldRenderSuggestions={shouldRenderSuggestions}
                focusInputOnSuggestionClick={false}
                inputProps={inputProps}
            />
            <span className={styles['icon-search']} onClick={handleSearchClick} />
        </div>
    );
}
