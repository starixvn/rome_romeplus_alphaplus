import { useTranslation } from 'react-i18next';

import { Block, BlockColumn, BlockRow } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { CommissionItem } from '@/types/new-account';

import styles from './Commission.module.scss';
import CommissionSelect from './CommissionSelect';

interface Props {
  productGroupId: number;
  groupItems: CommissionItem[];
}

export default function Commission({ productGroupId, groupItems }: Props) {
  const { t } = useTranslation();
  const updateCommissions = useNewAccountStore((state) => state.updateCommissions);
  const commissions = useNewAccountStore((state) => state.commissions[productGroupId].groupItems as CommissionItem[]);
  const subGroupIds = [...new Set(groupItems.map((i) => i.subGroupId))];
  const subGroups = subGroupIds.map((id) => {
    const items = groupItems.filter((i) => i.subGroupId == id);
    const max = Math.min(...items.map((item) => item.discount));
    const value = Math.min(...commissions.filter((i) => i.subGroupId == id).map((item) => item.discount));
    return { id, value, max, name: items[0].subGroupName, tooltipMessage: items[0].tooltipMessage };
  });

  return (
    <div className={styles.shortContent}>
      <Block title={t('Commission')}>
        {subGroups.map(({ id, name, value, max, tooltipMessage }) => (
          <BlockRow 
            key={id} 
            label={
              <>
                {name}
                {tooltipMessage && tooltipMessage.trim() && (
                  <span 
                    className="icon-information-outline" 
                    data-tooltip-id="global-tooltip" 
                    data-tooltip-content={tooltipMessage}
                    style={{ marginLeft: '8px', cursor: 'pointer' }}
                  />
                )}
              </>
            }
          >
            <BlockColumn>
              <CommissionSelect
                max={max}
                value={value}
                onChange={(e) => updateCommissions(productGroupId, id, Number(e.target.value))}
              />
            </BlockColumn>
          </BlockRow>
        ))}
      </Block>
    </div>
  );
}
