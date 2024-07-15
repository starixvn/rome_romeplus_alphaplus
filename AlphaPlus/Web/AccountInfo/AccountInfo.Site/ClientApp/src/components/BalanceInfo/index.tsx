import { useTranslation } from 'react-i18next';

import { UserLevel } from '../../constants';
import { useGlobalContext } from '../../context/global-context';
import NoInformation from '../common/NoInformation';
import styles from './BalanceInfo.module.scss';
import BalanceInfoRow from './BalanceInfoRow';
import GivenCreditRow from './GivenCreditRow';
import MemberCreditMaxRow from './MemberCreditMaxRow';

export default function BalanceInfo() {
    const { t } = useTranslation();
    const { customer } = useGlobalContext();

    if (!customer) {
        return <NoInformation />;
    }

    const { BalanceInfo } = customer;

    const sharedContents = [
        <BalanceInfoRow
            title={t('TotalBalance')}
            value={BalanceInfo.TotalBalance}
            hrefLink={BalanceInfo.ShowTotalBalanceTransferLink ? BalanceInfo.TotalBalanceTransferLink : ``}
        />,
        <BalanceInfoRow
            title={t('YesterdayBalance')}
            value={BalanceInfo.YesterdayBalance}
            hrefLink={BalanceInfo.ShowYesterdayBalanceTransferLink ? BalanceInfo.YesterdayBalanceTransferLink : ``}
        />,
        <BalanceInfoRow
            title={BalanceInfo.ReportLink?.Text}
            value={BalanceInfo.Outstanding}
            hrefLink={BalanceInfo.ShowOutstandingLink ? BalanceInfo.ReportLink.Url : ``}
        />,
        <BalanceInfoRow title={t('PendingTransfer3rd')} value={BalanceInfo.PendingTransfer3rd} />
    ];

    const givenCreditContent = (
        <GivenCreditRow editable={BalanceInfo.CanUpdateAccount} givenCredit={BalanceInfo.GivenCredit} />
    );

    const masterContents = [
        <BalanceInfoRow title={t('TotalCash')} value={BalanceInfo.TotalCash} />,
        <BalanceInfoRow title={t('YesterdayCash')} value={BalanceInfo.YesterdayCash} />,
        ...sharedContents,
        <BalanceInfoRow title={t('UsedCredit')} value={BalanceInfo.UsedCredit} />,
        givenCreditContent
    ];

    const balanceInfoByLevel = [
        {
            Id: UserLevel.Master,
            Contents: masterContents
        },
        {
            Id: UserLevel.Agent,
            Contents: [
                ...masterContents,
                <MemberCreditMaxRow
                    memberMaxCredit={BalanceInfo.MemberMaxCredit}
                    showMemberMaxCreditLimit={BalanceInfo.ShowMemberMaxCreditLimit}
                    memberMaxCreditLimit={BalanceInfo.MemberMaxCreditLimit}
                />
            ]
        },
        {
            Id: UserLevel.Member,
            Contents: [
                ...sharedContents,
                givenCreditContent,
                <BalanceInfoRow title={t('NetPosition')} value={BalanceInfo.NetPosition} />,
                <BalanceInfoRow title={t('BetCredit')} value={BalanceInfo.BetCredit} />
            ]
        }
    ];

    const contents = balanceInfoByLevel.find((level) => customer.RoleId === level.Id)?.Contents;

    return (
        <div className={styles['balance-info']}>
            {contents?.map((content, index) => (
                <div key={index}>{content} </div>
            ))}
        </div>
    );
}
