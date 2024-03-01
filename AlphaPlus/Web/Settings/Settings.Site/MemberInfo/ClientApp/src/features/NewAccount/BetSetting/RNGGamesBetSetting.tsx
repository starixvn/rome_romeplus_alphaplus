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

const RNGGameBetSettingProps: ReadonlyArray<keyof RNGGamesBetSettingItem> = ['minBet', 'maxBet', 'maxPerMatch'];
const RNGGamesBetSettingSubTypeProps: { [key: number]: (keyof RNGGamesBetSettingItem | '')[] } = {
  1: [...RNGGameBetSettingProps, ''],
  2: [...RNGGameBetSettingProps, 'maxPerBall'],
};

const LotteryKey = '220-1024';
const NumberGameKey = '161-81';
const RngGamesCopyLabel: ReadonlyArray<string> = [LotteryKey, NumberGameKey];

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
      {Object.keys(RNGGamesBetSettingSubTypeProps).map((subType) => {
        const subTypeNumber = Number(subType);
        const properties = [...RNGGamesBetSettingSubTypeProps[subTypeNumber]];
        const subGroupItems = groupItems.filter((groupItem) => groupItem.subType === subTypeNumber);

        return (
          <React.Fragment key={subType}>
            <BetSettingHeading key={subType} subType={subType} properties={properties} />
            {subGroupItems.map((subGroupItem, index) => (
              <BetSettingRow
                key={`${subGroupItem.sportTypeId}-${subGroupItem.betTypeId}`}
                groupTypeId={groupTypeId}
                groupItem={subGroupItem}
                label={renderLabel(subGroupItem)}
                properties={[...RNGGamesBetSettingSubTypeProps[Number(subType)]]}
                validate={validate}
                isCoping={isCopingRow(subGroupItems, index)}
              />
            ))}
          </React.Fragment>
        );
      })}
    </Block>
  );
}
