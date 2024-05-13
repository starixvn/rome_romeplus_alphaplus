import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { BlockIds } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import OnlineUser from '../../models/online-user';
import SettingsService from '../../services/settings-service';
import AccordionItem from '../common/AccordionItem';
import NoInformation from '../common/NoInformation';
import styles from './OnlineList.module.scss';

export default function OnlineList() {
    const { t } = useTranslation();
    const { expandedBlockId, handleBlockClick, customer, handleOpenIPInfo } = useGlobalContext();
    const [onlineUsers, setOnlineUsers] = useState<OnlineUser[] | undefined>(undefined);

    useEffect(() => {
        setOnlineUsers(undefined);
    }, [customer?.CustId]);

    const handleClick = () => {
        handleBlockClick?.(BlockIds.OnlineList);
        if (customer?.CustId && onlineUsers === undefined) {
            SettingsService.GetOnlineList(customer.CustId).then((data) => setOnlineUsers(data));
        }
    };

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

    return (
        <AccordionItem
            expanded={expandedBlockId === BlockIds.OnlineList}
            iconClass="icon-onlinelist"
            featureText={t('OnlineList')}
            onClick={handleClick}
        >
            {render()}
        </AccordionItem>
    );
}
