import classNames from 'classnames';
import { useTranslation } from 'react-i18next';

import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import CustomAnchorTag from '../common/CustomAnchorTag';
import NoInformation from '../common/NoInformation';
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
        return <NoInformation text={t('accountNotFound')} />;
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
                        <div className={classNames('col-xs-12', 'row-info')} key={index}>
                            <div className="col-xs-4">{userLevelInfo.LevelName}</div>
                            <div className={classNames('col-xs-8', 'col-right')}>
                                {customer?.RoleId === userLevelInfo.LevelId ? <SearchAccountInfo /> : <UplineInfo />}
                            </div>
                        </div>
                    )
                );
            })}
            <div
                className={classNames('col-xs-12', 'text-center', 'row-info', styles['list-button'], {
                    [styles['two-button']]: customer?.Buttons.length === 2,
                    [styles['one-button']]: customer?.Buttons.length === 1
                })}
            >
                {customer?.Buttons?.map((button, index) => (
                    <CustomAnchorTag key={index} classNames="btn-default with-level" href={button.Url}>
                        {button.Text}
                    </CustomAnchorTag>
                ))}
            </div>
        </div>
    );
}
