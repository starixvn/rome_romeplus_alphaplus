import axios from 'axios';
import { useState } from 'react';
import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';
import { Tooltip } from 'react-tippy';

import { UserLevel } from '../../constants';
import { formatNumber } from '../../helpers';
import Result from '../../models/result';
import { handleError } from '../../services/settings-service';
import CellContainNumberFormat from '../common/CellContainNumberFormat';
import styles from './GivenCredit.module.scss';

function Row({ title, value }: { title: string; value?: number }) {
    return (
        <div className={`col-xs-12 ${styles['form-group']}`}>
            <div className="col-xs-7">{title}</div>
            <CellContainNumberFormat value={value} classNames={`col-xs-5 ${styles.value}`} />
        </div>
    );
}

export default function GivenCredit() {
    const {
        searchAccountId,
        searchAccountRoleId,
        searchAccountName,
        currentGivenCredit,
        givenCreditMin,
        givenCreditMax,
        availableCredit,
        memberMaxCredit
    } = givenCreditParams;
    const { t } = useTranslation();
    const [hideMessages, setHideMessages] = useState(true);
    const [updateSuccess, setUpdateSuccess] = useState(true);
    const [message, setMessage] = useState('');
    const [newGivenCredit, setNewGivenCredit] = useState(currentGivenCredit);

    const handleClose = (updateSuccessfully = false) => {
        if (
            window.top?.updateGivenCreditPopup &&
            window.top.updateGivenCreditPopup.onClosing &&
            typeof window.top.updateGivenCreditPopup.onClosing === 'function'
        ) {
            window.top.updateGivenCreditPopup.onClosing(
                searchAccountName, // use for reload left panel
                updateSuccessfully,
                newGivenCredit, // use for update value on html Show QR code form
                searchAccountId
            ); // use for update value on html Credit Balance form
        }
    };

    const showErrorMessage = (message: string) => {
        setHideMessages(false);
        setMessage(message);
        setUpdateSuccess(false);
    };

    const updateGivenCredit = async (searchAccountId: number, searchAccountRoleId: number, creditAmount: number) => {
        return axios
            .post<Result>(
                `${window.top?.alpha.urls['accountInfoRootPath']}SearchAccount/UpdateGivenCredit`,
                { searchAccountId, searchAccountRoleId, creditAmount },
                {
                    headers: {
                        RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                        'X-Requested-With': 'XMLHttpRequest'
                    }
                }
            )
            .then((response) => response.data);
    };

    const handleSubmit = () => {
        if (newGivenCredit < 0 || newGivenCredit < givenCreditMin || newGivenCredit > givenCreditMax) {
            showErrorMessage(t('InvalidValue'));
            return;
        }

        if (newGivenCredit === currentGivenCredit) {
            showErrorMessage(t('ThereIsNoChange'));
            return;
        }

        $.blockUI?.();

        updateGivenCredit(searchAccountId, searchAccountRoleId, newGivenCredit)
            .then((data) => {
                setHideMessages(false);
                setMessage(data.IsSuccessful ? t('UpdateGivenCreditSuccessfully') : data.Message.trim());
                setUpdateSuccess(data.IsSuccessful);

                if (data.IsSuccessful) {
                    handleClose(data.IsSuccessful);
                }

                $.unblockUI?.();
            })
            .catch(handleError);
    };

    const buildNumberFormat = (value: number) => (
        <NumericFormat
            className={styles.number}
            value={value}
            displayType={'text'}
            decimalScale={2}
            fixedDecimalScale={true}
            thousandSeparator={true}
        />
    );

    const renderTooltip = () => {
        if (searchAccountRoleId == UserLevel.Member && memberMaxCredit > 0) {
            const value = `${t('CreditHint').replace('{0}', formatNumber(availableCredit))}<br>${t(
                'MemberMaxCreditHint'
            ).replace('{0}', formatNumber(memberMaxCredit))}`;
            return (
                <Tooltip
                    {...{
                        title: value,
                        position: 'left',
                        trigger: 'mouseenter',
                        arrow: true
                    }}
                >
                    <span className={`icon-credit ${styles['icon-credit']}`}></span>
                </Tooltip>
            );
        }

        return null;
    };

    const buildGivenCreditLimitNote = () => {
        return (
            <div className={`col-xs-12 ${styles['form-group']} ${styles.note}`}>
                <div className={styles.value}>
                    {t('Min')}: {buildNumberFormat(givenCreditMin)} - {t('Max')}: {buildNumberFormat(givenCreditMax)}
                    {renderTooltip()}
                </div>
            </div>
        );
    };

    const handleKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
        if (e.key === 'Enter') {
            handleSubmit();
        }
    };

    return (
        <div className={styles['given-credit']}>
            <div className={hideMessages ? `hidden` : ``}>
                <span className={`col-xs-12 msg ${styles.msg} ${updateSuccess ? 'msg-success' : 'msg-error'}`}>
                    {message}
                </span>
            </div>
            <Row title={t('CurrentGivenCredit')} value={currentGivenCredit} />

            <div className={`col-xs-12 ${styles['form-group']}`}>
                <div className="col-xs-7">{t('GivenCredit')}</div>
                <NumericFormat
                    className={`col-xs-5 ${styles.textbox}`}
                    value={newGivenCredit}
                    fixedDecimalScale={true}
                    thousandSeparator={true}
                    allowNegative={false}
                    decimalScale={2}
                    onKeyUp={handleKeyPress}
                    onValueChange={(values) => {
                        const { floatValue } = values;
                        setNewGivenCredit(floatValue === undefined ? 0 : floatValue);
                    }}
                />
            </div>

            {buildGivenCreditLimitNote()}

            <Row title={t('Difference')} value={newGivenCredit - currentGivenCredit} />

            <div className={`col-xs-12 row-info popup-bottom ${styles['list-button']}`}>
                <button className="btn-submit" onClick={handleSubmit}>
                    {t('Submit')}
                </button>
                <button className="btn-cancel" onClick={() => handleClose()}>
                    {t('Cancel')}
                </button>
            </div>
        </div>
    );
}
