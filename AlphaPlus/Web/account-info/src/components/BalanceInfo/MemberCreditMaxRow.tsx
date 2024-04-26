import { useTranslation } from 'react-i18next';
import { Tooltip } from 'react-tippy';

import CellContainNumberFormat from '../common/CellContainNumberFormat';

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
        <div className="row-info">
            <div className="col">{t('MemberMaxCredit')}</div>
            <div className="col col-right">{render()}</div>
        </div>
    );
}
