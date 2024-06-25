import { useTranslation } from 'react-i18next';

import { useGlobalContext } from '../../context/global-context';
import OnlineUser from '../../models/online-user';
import NoInformation from '../common/NoInformation';
import styles from './OnlineList.module.scss';

export default function OnlineList({ onlineUsers }: { onlineUsers: OnlineUser[] | undefined }) {
    const { t } = useTranslation();
    const { handleOpenIPInfo } = useGlobalContext();

    const render = () => {
        if (onlineUsers && onlineUsers.length) {
            return (
                <div className={styles['online-list']}>
                    <table className="tblRpt tblRpt-bordered width-100per">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>{t('IP')}</th>
                                <th>{t('LoginTime')}</th>
                            </tr>
                        </thead>
                        <tbody>
                            {onlineUsers.map((onlineList, index) => (
                                <tr key={index}>
                                    <td className="text-center">{index + 1}</td>
                                    <td>
                                        <a onClick={() => handleOpenIPInfo?.(onlineList.IP)} data-ip={onlineList.IP}>
                                            {onlineList.IP}
                                        </a>
                                    </td>
                                    <td className="text-center">{onlineList.FormattedLoginTime}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            );
        }
        return <NoInformation text={t('ThisUserIsNotOnline')} />;
    };

    return <>{render()}</>;
}
