import { useTranslation } from 'react-i18next';

import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import styles from './CustomerInfo.module.scss';
import SearchAccountInfo from './search-account-info';
import UplineInfo from './upline-info';

type Props = {
    searchCounter: number;
};

export default function CustomerInfo({ searchCounter }: Props) {
    const { t } = useTranslation();
    const { mobileView, customer } = useGlobalContext();

    if (searchCounter === 0 && ((mobileView && customer === null) || !mobileView)) {
        return null;
    }
    if (searchCounter > 0 && customer === null) {
        return <div className={styles['no-information']}>{t('accountNotFound')}</div>;
    }

    const userLevelInfoArray = [
        {
            Name: customer?.MasterName,
            LevelName: t('master'),
            LevelId: UserLevel.Master,
            Visible: customer?.ShowMaster
        },
        {
            Name: customer?.AgentName,
            LevelName: t('agent'),
            LevelId: UserLevel.Agent,
            Visible: customer?.ShowAgent
        },
        {
            Name: customer?.MemberName,
            LevelName: t('customer'),
            LevelId: UserLevel.Member,
            Visible: customer?.ShowMember
        }
    ];

    return (
        <div className={styles['block-info']}>
            {userLevelInfoArray.map((userLevelInfo, index) => {
                return (
                    userLevelInfo.Visible && (
                        <div className="col-xs-12 row-info" key={index}>
                            <div className="col-xs-4">{userLevelInfo.LevelName}</div>
                            <div className="col-xs-8 col-right custom-col">
                                {customer?.RoleId === userLevelInfo.LevelId ? <SearchAccountInfo /> : <UplineInfo />}
                            </div>
                        </div>
                    )
                );
            })}
        </div>
    );
}
