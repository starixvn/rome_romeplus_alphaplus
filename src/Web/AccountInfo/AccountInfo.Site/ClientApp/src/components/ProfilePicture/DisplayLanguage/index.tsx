import React, { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { useOutsideClick } from '../../../hooks/outside-click';
import styles from './DisplayLanguage.module.scss';

export default function DisplayLanguage() {
    const { displayLanguage, languages, allowUpdateDisplayLanguage } = profilePictureParams;
    const { t } = useTranslation();
    const [openPanel, setOpenPanel] = useState(true);
    const [openDropdown, setOpenDropdown] = useState(false);
    const [selectedLanguage, setSelectedLanguage] = useState(displayLanguage);

    const ref = useOutsideClick(() => {
        setOpenDropdown(false);
    });

    const handleOpenLanguageDropdown = (e: React.MouseEvent<HTMLElement>) => {
        e.stopPropagation();
        setOpenDropdown(true);
    };

    const handleChangeDisplayLanguage = (e: React.MouseEvent<HTMLElement>, languageCode: string) => {
        e.stopPropagation();
        setSelectedLanguage(languageCode);
        setOpenDropdown(false);
    };

    const renderDisplayLanguage = () => {
        if (selectedLanguage) {
            return (
                <label onClick={handleOpenLanguageDropdown}>
                    <span className={'icon-flag langFlag-' + selectedLanguage.toLowerCase()}></span>
                    {languages[selectedLanguage]}
                </label>
            );
        }

        return <label onClick={handleOpenLanguageDropdown}>{t('PleaseSelectDisplayLanguage')}</label>;
    };

    const handleOnSubmitChangeDisplayLanguage = () => {
        if (selectedLanguage) {
            window.top?.headerNav.changeLanguage(selectedLanguage, true);
        }
    };

    return (
        <div className="panel">
            <div
                className={openPanel ? 'panel-heading open' : 'panel-heading'}
                onClick={() => setOpenPanel(!openPanel)}
            >
                <h4 className="panel-title">{t('DisplayLanguage')}</h4>
            </div>
            <div className="panel-content">
                <p>{t('DisplayLanguageIntro')}</p>
                <div className={styles.language}>
                    <div ref={ref} className={`${styles['block-language']} ${openDropdown ? styles['open'] : ''}`}>
                        {renderDisplayLanguage()}
                        <ul className={`dropdown-menu ${styles['dropdown-menu']}`}>
                            {Object.entries(languages).map(([key, value]) => (
                                <li key={key} onClick={(e) => handleChangeDisplayLanguage(e, key)}>
                                    <span className={'icon-flag langFlag-' + key.toLowerCase()}></span>
                                    {value}
                                </li>
                            ))}
                        </ul>
                    </div>
                    <button
                        className="btn-submit"
                        onClick={handleOnSubmitChangeDisplayLanguage}
                        disabled={!allowUpdateDisplayLanguage}
                    >
                        {t('Submit')}
                    </button>
                </div>
            </div>
        </div>
    );
}
