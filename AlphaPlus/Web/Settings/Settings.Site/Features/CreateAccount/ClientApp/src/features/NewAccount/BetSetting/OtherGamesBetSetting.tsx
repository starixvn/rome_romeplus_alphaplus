import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { Block } from '@/features/Shared/Block';
import useNewAccountStore from '@/store/newAccountStore';
import { OtherGamesBetSettingItem } from '@/types/new-account';

import BetSettingHeading from './BetSettingHeading';
import BetSettingRow from './BetSettingRow';
import OtherGamesValidator from './otherGamesValidator';
import { BetSettingProps } from './type';
import { getErrorMessageTranslate } from './utils';

const OtherGamesBetSettingProps: ReadonlyArray<keyof OtherGamesBetSettingItem> = ['minBet', 'maxBet', 'maxPerMatch'];

export default function OtherGamesBetSetting({
  is789y,
  groupTypeId,
  groupItems,
}: BetSettingProps<OtherGamesBetSettingItem>) {
  const { t } = useTranslation();
  const [error, submitCount] = useNewAccountStore((state) => [state.betSettingErrors, state.submitCount], shallow);

  const validate = (
    configs: OtherGamesBetSettingItem,
    newBetSettings: OtherGamesBetSettingItem,
    prop: keyof OtherGamesBetSettingItem,
  ) => {
    const error = new OtherGamesValidator(is789y, configs, newBetSettings).validate(prop);
    return {
      isValid: error === '',
      message: error !== '' ? getErrorMessageTranslate(error, t) : '',
    };
  };

  return (
    <Block title={t('BetSetting')} isCollapse={{ collapse: error.length === 0, resetCollapse: submitCount }}>
      <BetSettingHeading properties={[...OtherGamesBetSettingProps]} />
      {groupItems.map((subGroupItem) => (
        <BetSettingRow
          key={`${subGroupItem.sportTypeId}-${subGroupItem.betTypeId}`}
          groupTypeId={groupTypeId}
          groupItem={subGroupItem}
          label={subGroupItem.displayName}
          properties={[...OtherGamesBetSettingProps]}
          validate={validate}
        />
      ))}
    </Block>
  );
}
