import classNames from 'classnames';
import { useTranslation } from 'react-i18next';

import { useGlobalContext } from '../../context/global-context';
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
    const { handleOpenIPInfo } = useGlobalContext();
    return (
        <div className={styles['group']}>
            <div className={classNames('row-info', 'bold', styles['row-info'])}>{title}:</div>
            <div className={classNames('row-info', styles['row-info'])}>
                <span className="link" onClick={() => handleOpenIPInfo?.(value)}>
                    {value}
                </span>
            </div>
        </div>
    );
}

type Props = {
    lastTxn: string;
};

export default function LoginInfo({ lastTxn }: Props) {
    const { t } = useTranslation();
    const { customer } = useGlobalContext();

    const loginInfo = customer?.LoginInfo;
    if (!loginInfo) {
        return '';
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

    return (
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
    );
}
