import { useState } from 'react';
import { useTranslation } from 'react-i18next';

import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import TotalDownline from '../../models/total-downline';
import styles from './DownlineList.module.scss';
import RadioButtonStatus from './RadioButtonStatus';

const ACTIVE_ID = 1;
const CLOSED_ID = 2;
const SUSPENDED_ID = 3;

interface Props extends TotalDownline {
    isDirectMember?: boolean;
    fetchDownlineListFromServer: (
        pageIndex?: number,
        reloadByStatus?: boolean,
        statusId?: number,
        totalPages?: number
    ) => void;
}

export default function TotalDirectDownlines({
    isDirectMember,
    TotalActive,
    TotalClosed,
    TotalSuspended,
    fetchDownlineListFromServer
}: Props) {
    const { t } = useTranslation();
    const { customer } = useGlobalContext();
    const [checkedStatusId, setCheckedStatusId] = useState(ACTIVE_ID);

    const StatusItems = [
        { id: ACTIVE_ID, title: 'A', className: 'status-active', value: TotalActive },
        { id: CLOSED_ID, title: 'C', className: 'status-closed', value: TotalClosed },
        { id: SUSPENDED_ID, title: 'S', className: 'status-suspended', value: TotalSuspended }
    ];

    const handleChange = (statusId: number) => {
        setCheckedStatusId(statusId);
        fetchDownlineListFromServer(1, true, statusId);
    };

    return (
        <div style={{ float: 'left', width: '100%' }}>
            <div className="row-info bold">
                {isDirectMember
                    ? t('DirectMembers')
                    : customer?.RoleId === UserLevel.Master
                    ? t('Agents')
                    : t('Members')}
            </div>
            <div className={styles['status-list']}>
                {StatusItems.map(({ id, title, className, value }) => (
                    <RadioButtonStatus
                        key={`${id}_${isDirectMember}`}
                        isDirectMember={isDirectMember}
                        statusId={id}
                        title={title}
                        className={styles[className]}
                        value={value}
                        checked={id === checkedStatusId}
                        handleChange={handleChange}
                    />
                ))}
            </div>
        </div>
    );
}
