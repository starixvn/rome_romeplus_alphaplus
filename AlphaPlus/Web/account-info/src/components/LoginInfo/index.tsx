import classNames from 'classnames';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { BlockIds } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import SettingsService from '../../services/settings-service';
import AccordionItem from '../common/AccordionItem';
import NoInformation from '../common/NoInformation';
import styles from './LoginInfo.module.scss';

function LoginInfoRow({ title, value }: { title: string; value: string }) {
    return (
        <div className={styles['group']}>
            <div className={classNames('row-info', 'bold', styles['row-info'])}>{title}:</div>
            <div className={classNames('row-info', styles['row-info'])}>{value}</div>
        </div>
    );
}

function LoginInfoWithLinkRow({ title, value }: { title: string; value: string }) {
    return (
        <div className={styles['group']}>
            <div className={classNames('row-info', 'bold', styles['row-info'])}>{title}:</div>
            <div className={classNames('row-info', styles['row-info'])}>
                <span className="link" data-ip={value}>
                    {value}
                </span>
            </div>
        </div>
    );
}

export default function LoginInfo() {
    const { t } = useTranslation();
    const { customer, expandedBlockId, handleBlockClick } = useGlobalContext();
    const [lastTxn, setLastTxn] = useState('');

    useEffect(() => {
        setLastTxn('');
    }, [customer?.CustId]);

    const loginInfo = customer?.LoginInfo;
    if (!loginInfo) {
        return <NoInformation />;
    }

    const loginInfoPropsArray = [
        {
            title: t('CreatedDate'),
            value: loginInfo.CreatedDate
        },
        {
            title: t('LastLogin'),
            value: loginInfo.LastLogin
        },
        {
            title: t('LastTxn'),
            value: lastTxn
        }
    ];

    if (loginInfo.AllowOpenInfoPopup) {
        loginInfoPropsArray.push({
            title: t('LoginIP'),
            value: loginInfo.LoginIP
        });
    }

    const handleClick = () => {
        handleBlockClick?.(BlockIds.LoginInfo);

        if (loginInfo && !lastTxn) {
            // $.blockUI();
            SettingsService.GetLastTxn(customer.UserName).then((lastTxn) => setLastTxn(lastTxn));
        }
    };

    return (
        <AccordionItem
            expanded={expandedBlockId === BlockIds.LoginInfo}
            iconClass="icon-login"
            featureText={t('LoginInfo')}
            onClick={handleClick}
        >
            <div className={styles['login-info']}>
                {loginInfoPropsArray.map((loginInfoProps, index) => (
                    <LoginInfoRow {...loginInfoProps} key={index} />
                ))}
                {loginInfo.AllowOpenInfoPopup ? (
                    <LoginInfoRow {...{ title: t('LoginIP'), value: loginInfo.LoginIP }} />
                ) : (
                    <LoginInfoWithLinkRow
                        {...{
                            title: t('LoginIP'),
                            value: loginInfo.LoginIP
                        }}
                    />
                )}
            </div>
        </AccordionItem>
    );
}
