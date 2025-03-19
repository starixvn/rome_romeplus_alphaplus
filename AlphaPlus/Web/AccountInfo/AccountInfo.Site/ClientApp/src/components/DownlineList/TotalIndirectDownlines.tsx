import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import TotalDownline from '../../models/total-downline';
import styles from './DownlineList.module.scss';

export default function TotalIndirectDownlines({ TotalActive, TotalClosed, TotalSuspended }: TotalDownline) {
    const { t } = useTranslation();

    const tooltipContent = (
        <>
            <div className="nowrap bold">
                {t('Active')}: <span className={styles['status-active']}>{TotalActive}</span>
            </div>
            <div className="nowrap bold">
                {t('Closed')}: <span className={styles['status-closed']}>{TotalClosed}</span>
            </div>
            <div className="nowrap bold">
                {t('Suspended')}: <span className={styles['status-suspended']}>{TotalSuspended}</span>
            </div>
        </>
    );

    return (
        <div className="col-xs-12 row-info">
            <div className="col-xs-6 px-0">{t('Members')}</div>
            <div className="col-xs-6 col-right px-0">
                <div className="pull-right pointer">
                    <Tooltip
                        {...{
                            position: 'top',
                            trigger: 'mouseenter',
                            arrow: true,
                            theme: 'light',
                            html: tooltipContent
                        }}
                    >
                        <span className={styles['status-active']}>{TotalActive}/</span>
                        <span className={styles['status-closed']}>{TotalClosed}/</span>
                        <span className={styles['status-suspended']}>{TotalSuspended}</span>
                    </Tooltip>
                </div>
            </div>
            <div className="col-xs-12">
                <hr />
            </div>
        </div>
    );
}
