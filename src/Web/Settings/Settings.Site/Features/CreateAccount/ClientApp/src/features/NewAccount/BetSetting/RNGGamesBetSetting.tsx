import React from 'react';
import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { RNGGamesBetSettingItem } from '@/types/new-account';

import { renderCopyLabel } from './BetSettingCopyLabel';
import BetSettingHeading from './BetSettingHeading';
import BetSettingRow from './BetSettingRow';
import RNGGamesValidator from './rngGamesValidator';
import { BetSettingProps } from './type';
import { useCopyBetSetting } from './useCopyBetSetting';
import { getErrorMessageTranslate } from './utils';

const RNGGameBetSettingProps: ReadonlyArray<keyof RNGGamesBetSettingItem> = [
  'minBet',
  'maxBet',
  'maxPerMatch',
  'maxPerBall',
];

const LotteryKey = '169-3611';
const RngGamesCopyLabel: ReadonlyArray<string> = [LotteryKey];

export default function RNGGamesBetSetting({
  is789y,
  groupTypeId,
  groupItems,
}: BetSettingProps<RNGGamesBetSettingItem>) {
  const { t } = useTranslation();
  const [error, submitCount] = useNewAccountStore((state) => [state.betSettingErrors, state.submitCount], shallow);
  const handleCopyRngGames = useNewAccountStore((state) => state.handleCopyRngGames);
  const [copyId, handleCopyBetSettings] = useCopyBetSetting();

  const isCopingRow = (subGroupItems: RNGGamesBetSettingItem[], index: number) => {
    if (copyId === '') return false;
    const [sportTypeId, betTypeId] = copyId.split('-');
    const copyItemIndex = subGroupItems.findIndex(
      (copyItem) => copyItem.sportTypeId === Number(sportTypeId) && copyItem.betTypeId === Number(betTypeId),
    );
    return index > copyItemIndex && copyItemIndex !== -1;
  };

  const validate = (
    configs: RNGGamesBetSettingItem,
    newBetSettings: RNGGamesBetSettingItem,
    prop: keyof RNGGamesBetSettingItem,
  ) => {
    const error = new RNGGamesValidator(is789y, configs, newBetSettings).validate(prop);

    return {
      isValid: error === '',
      message: error !== '' ? getErrorMessageTranslate(error, t) : '',
    };
  };

  const renderLabel = (groupItem: RNGGamesBetSettingItem) => {
    const sportTypeKey = `${groupItem.sportTypeId}-${groupItem.betTypeId}`;
    if (RngGamesCopyLabel.includes(sportTypeKey))
      return renderCopyLabel(groupItem.displayName, copyId === sportTypeKey, () => {
        const isCopyable = handleCopyRngGames(sportTypeKey, groupItem);
        isCopyable && handleCopyBetSettings(sportTypeKey);
      });
    return groupItem.displayName;
  };

  return (
    <Block title={t('BetSetting')} isCollapse={{ collapse: error.length === 0, resetCollapse: submitCount }}>
      <React.Fragment>
        <BetSettingHeading properties={[...RNGGameBetSettingProps]} />
        {groupItems.map((item, index) => (
          <BetSettingRow
            key={`${item.sportTypeId}-${item.betTypeId}`}
            groupTypeId={groupTypeId}
            groupItem={item}
            label={renderLabel(item)}
            properties={[...RNGGameBetSettingProps]}
            validate={validate}
            isCoping={isCopingRow(groupItems, index)}
          />
        ))}
      </React.Fragment>
    </Block>
  );
}
