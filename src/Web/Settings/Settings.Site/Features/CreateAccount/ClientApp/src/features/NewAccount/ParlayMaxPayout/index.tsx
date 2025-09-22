import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block, BlockColumn, BlockRow } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { GroupItem, ParlayMaxPayoutItem } from '@/types/new-account';

import ParlayMaxPayoutRowBlock from './ParlayMaxPayoutRowBlock';
import ParlayMaxPayoutNote from './ParlayMaxPayoutNote';

const ParlayHeadingNames: ReadonlyArray<keyof ParlayMaxPayoutItem> = [
  'singleMatchParlay',
  'combo2',
  'combo35',
  'comboOver6',
];

type ParlayMaxPayoutProps<T extends GroupItem> = {
  groupTypeId: number;
  groupItems: T[];
};

export default function ParlayMaxPayout({ groupTypeId, groupItems }: ParlayMaxPayoutProps<ParlayMaxPayoutItem>) {
  const { t } = useTranslation();
  const [error, submitCount] = useNewAccountStore((state) => [state.parlayMaxPayoutErrors, state.submitCount], shallow);
  const BlockTitle = (
    <>
      {t('ParlayMaxPayoutPerBet')} <ParlayMaxPayoutNote />
    </>
  );
  return (
    <Block title={BlockTitle} isCollapse={{ collapse: error.length === 0, resetCollapse: submitCount }}>
      <BlockRow isHeaderRow>
        {ParlayHeadingNames.map((header) => (
          <BlockColumn key={header}>{t(header)}</BlockColumn>
        ))}
      </BlockRow>
      {groupItems.map((groupItem) => (
        <BlockRow key={`${groupItem.sportTypeId}-${groupItem.betTypeId}`} label={groupItem.displayName}>
          <BlockColumn>
            <ParlayMaxPayoutRowBlock
              groupTypeId={groupTypeId}
              groupItem={groupItem}
              valueProp="singleMatchParlay"
              maxValueProp="singleMatchParlay"
              isUnlimitedProp="isAllowUnlimitSingleMatchParlay"
            />
          </BlockColumn>
          <BlockColumn>
            <ParlayMaxPayoutRowBlock
              groupTypeId={groupTypeId}
              groupItem={groupItem}
              valueProp="combo2"
              maxValueProp="combo2"
              isUnlimitedProp="isAllowUnlimitCombo2"
            />
          </BlockColumn>
          <BlockColumn>
            <ParlayMaxPayoutRowBlock
              groupTypeId={groupTypeId}
              groupItem={groupItem}
              valueProp="combo35"
              maxValueProp="combo35"
              isUnlimitedProp="isAllowUnlimitCombo35"
            />
          </BlockColumn>
          <BlockColumn>
            <ParlayMaxPayoutRowBlock
              groupTypeId={groupTypeId}
              groupItem={groupItem}
              valueProp="comboOver6"
              maxValueProp="comboOver6"
              isUnlimitedProp="isAllowUnlimitComboOver6"
            />
          </BlockColumn>
        </BlockRow>
      ))}
    </Block>
  );
}
