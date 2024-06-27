import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { SportsBookBetSettingItem } from '@/types/new-account';

import { renderCopyLabel } from './BetSettingCopyLabel';
import BetSettingHeading from './BetSettingHeading';
import BetSettingRow from './BetSettingRow';
import MixSportParlayNote from './MixSportParlayNote';
import SportsBookValidator from './sportsBookValidator';
import { BetSettingProps } from './type';
import { useCopyBetSetting } from './useCopyBetSetting';
import { getErrorMessageTranslate } from './utils';

const SportsBookBetSettingProps: ReadonlyArray<keyof SportsBookBetSettingItem> = [
  'minBet',
  'maxBet',
  'maxPerMatch',
  'maxPayoutPerMatch',
];

const MixSportParlayKey = '99-9';
const SoccerKey = '1-1';
const BasketballKey = '2-1';
const SportsBookCopyLabel: ReadonlyArray<string> = [SoccerKey, BasketballKey];

export default function SportsBookBetSetting({
  is789y,
  groupTypeId,
  groupItems,
}: BetSettingProps<SportsBookBetSettingItem>) {
  const { t } = useTranslation();
  const [error, submitCount] = useNewAccountStore((state) => [state.betSettingErrors, state.submitCount], shallow);
  const handleCopySportsBook = useNewAccountStore((state) => state.handleCopySportsBook);
  const [copyId, handleCopyBetSettings] = useCopyBetSetting();

  const isCopingRow = (index: number) => {
    if (copyId === '') return false;
    const [sportTypeId, betTypeId] = copyId.split('-').map((item) => Number(item));
    const copyItemIndex = groupItems.findIndex(
      (copyItem) => copyItem.sportTypeId === sportTypeId && copyItem.betTypeId === betTypeId,
    );
    return index > copyItemIndex;
  };

  const validate = (
    configs: SportsBookBetSettingItem,
    newBetSettings: SportsBookBetSettingItem,
    prop: keyof SportsBookBetSettingItem,
  ) => {
    const error = new SportsBookValidator(is789y, configs, newBetSettings).validate(prop);
    return {
      isValid: error === '',
      message: error !== '' ? getErrorMessageTranslate(error, t) : '',
    };
  };

  const renderLabel = (groupItem: SportsBookBetSettingItem) => {
    const sportTypeKey = `${groupItem.sportTypeId}-${groupItem.betTypeId}`;
    if (SportsBookCopyLabel.includes(sportTypeKey))
      return renderCopyLabel(groupItem.displayName, copyId === sportTypeKey, () => {
        const isCopyable = handleCopySportsBook(sportTypeKey, groupItem);
        isCopyable && handleCopyBetSettings(sportTypeKey);
      });
    if (sportTypeKey === MixSportParlayKey) {
      return (
        <>
          {groupItem.displayName} <MixSportParlayNote />
        </>
      );
    }
    return groupItem.displayName;
  };

  return (
    <Block title={t('BetSetting')} isCollapse={{ collapse: error.length === 0, resetCollapse: submitCount }}>
      <BetSettingHeading properties={[...SportsBookBetSettingProps]} />
      {groupItems.map((groupItem, index) => (
        <BetSettingRow
          key={groupItem.displayName}
          groupTypeId={groupTypeId}
          groupItem={groupItem}
          label={renderLabel(groupItem)}
          properties={[...SportsBookBetSettingProps]}
          validate={validate}
          isCoping={isCopingRow(index)}
        />
      ))}
    </Block>
  );
}
