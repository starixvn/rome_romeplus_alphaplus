import classNames from 'classnames';
import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import CellContainNumberFormat from '../common/CellContainNumberFormat';
import styles from './BalanceInfo.module.scss';

type Props = {
    showMemberMaxCreditLimit: boolean;
    memberMaxCredit: number;
    memberMaxCreditLimit: string;
};
export default function MemberCreditMaxRow({ showMemberMaxCreditLimit, memberMaxCredit, memberMaxCreditLimit }: Props) {
    const { t } = useTranslation();

    const render = () => {
        if (showMemberMaxCreditLimit) {
            return (
                <Tooltip
                    {...{
                        title: memberMaxCreditLimit,
                        position: 'bottom',
                        trigger: 'mouseenter',
                        arrow: true
                    }}
                >
                    <CellContainNumberFormat value={memberMaxCredit} />
                </Tooltip>
            );
        }

        return <CellContainNumberFormat value={memberMaxCredit} />;
    };

    return (
        <div className={classNames('row-info', styles['row-info'])}>
            <div className={classNames('col', styles['col'])}>{t('MemberMaxCredit')}</div>
            <div className={classNames('col', styles['col'], 'col-right')}>{render()}</div>
        </div>
    );
}
