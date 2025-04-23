import { useTranslation } from 'react-i18next';

import { UserLevel } from '@/constants';
import { ProductGroupIds } from '@/constants/product';
import { Block, BlockColumn, BlockItem, BlockRow } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { CommissionItem } from '@/types/new-account';

import CommissionSelect from '../CommissionSelect';
import CommissionGroup from './CommissionGroup';
import styles from './SportsBookCommission.module.scss';

interface ICommissionProps {
  uplineCommissions: CommissionItem[];
  commissions: CommissionItem[];
  handleCommissionChange: (sportTypeId: number, betTypeId: number, discount: number) => void;
}

function SMASportsBookCommission({ uplineCommissions, commissions, handleCommissionChange }: ICommissionProps) {
  return (
    <div className={styles.container}>
      {uplineCommissions.map((item) => {
        const value = (
          commissions.find(
            (c) => c.sportTypeId === item.sportTypeId && c.betTypeId === item.betTypeId,
          ) as CommissionItem
        ).discount;
        return (
          <BlockItem key={`${item.sportTypeId}${item.betTypeId}`} label={item.displayName}>
            <CommissionSelect
              max={item.discount}
              value={value}
              onChange={(e) => handleCommissionChange(item.sportTypeId, item.betTypeId, Number(e.target.value))}
            />
          </BlockItem>
        );
      })}
    </div>
  );
}

function MemberSportsBookCommission({ uplineCommissions, commissions, handleCommissionChange }: ICommissionProps) {
  const { t } = useTranslation();
  const asianHDPCommValue = useNewAccountStore((state) => {
    const commissionGroup = state.group;
    const lastChar = commissionGroup[commissionGroup.length - 1];
    return state.generalInfo.commissions.find((e) => e.name === commissionGroup || e.name === lastChar)?.discount || 0;
  });
  const generalInfo = useNewAccountStore((state) => state.generalInfo);
  const group = useNewAccountStore((state) => state.group);
  const updateGroup = useNewAccountStore((state) => state.updateGroup);
  const updateSportsBookCommissions = useNewAccountStore((state) => state.updateSportsBookCommissions);

  return (
    <>
      <BlockRow label={t('Group')}>
        <BlockColumn>
          {generalInfo && group && (
            <CommissionGroup
              value={group}
              onChange={(e) => {
                const newCommissionGroup = e.target.value;
                updateGroup(e.target.value);
                const lastChar = newCommissionGroup[newCommissionGroup.length - 1];
                const newCommission =
                  generalInfo.commissions.find((e) => e.name === newCommissionGroup || e.name === lastChar)?.discount ??
                  0;
                updateSportsBookCommissions(1, 1, newCommission);
              }}
            />
          )}
        </BlockColumn>
        <BlockColumn>{''}</BlockColumn>
      </BlockRow>
      <BlockRow isHeaderRow>
        <BlockColumn>{t('lblCommissionForAgent')}</BlockColumn>
        <BlockColumn>{t('lblCommissionForMember')}</BlockColumn>
      </BlockRow>
      {uplineCommissions.map((item) => {
        const isAsianHDP = item.sportTypeId === 1 && item.betTypeId === 1;
        const value = (
          commissions.find(
            (c) => c.sportTypeId === item.sportTypeId && c.betTypeId === item.betTypeId,
          ) as CommissionItem
        ).discount;

        return (
          <BlockRow key={`${item.sportTypeId}${item.betTypeId}`} label={item.displayName}>
            <BlockColumn>
              <div className={styles.blockCenter}>
                <input value={isAsianHDP ? asianHDPCommValue : item.discount} disabled />
              </div>
            </BlockColumn>
            <BlockColumn>
              <CommissionSelect
                max={isAsianHDP ? asianHDPCommValue : item.discount}
                value={value}
                onChange={(e) => handleCommissionChange(item.sportTypeId, item.betTypeId, Number(e.target.value))}
              />
            </BlockColumn>
          </BlockRow>
        );
      })}
    </>
  );
}

export default function SportsBookCommission({ groupItems }: { groupItems: CommissionItem[] }) {
  const { t } = useTranslation();
  const upLineRoleId = useNewAccountStore((state) => state.generalInfo.uplineRoleId);
  const commissions = useNewAccountStore(
    (state) => state.sportsBookCommissions[ProductGroupIds.SportsBook].groupItems as CommissionItem[],
  );
  const handleCommissionChange = useNewAccountStore((state) => state.updateSportsBookCommissions);

  const props = {
    uplineCommissions: groupItems,
    commissions,
    handleCommissionChange,
  };

  return (
    <Block title={t('Commission')}>
      {upLineRoleId === UserLevel.Agent ? (
        <MemberSportsBookCommission {...props} />
      ) : (
        <SMASportsBookCommission {...props} />
      )}
    </Block>
  );
}
