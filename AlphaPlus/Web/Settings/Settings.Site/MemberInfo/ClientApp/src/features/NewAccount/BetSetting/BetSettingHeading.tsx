import { useTranslation } from 'react-i18next';

import { BlockColumn, BlockRow } from '@/features/Shared/Block';

import MaxPayoutSettingInformationNote from './MaxPayoutSettingInformationNote';

export default function BetSettingHeading({ properties, subType = '' }: { properties: string[]; subType?: string }) {
  const { t } = useTranslation();
  return (
    <BlockRow isHeaderRow>
      {[...properties].map((property, index) => {
        const key = `${index}-${subType}-${property}`;
        return (
          <BlockColumn key={key}>
            {t(property)} {property === 'maxPayoutPerMatch' && <MaxPayoutSettingInformationNote />}
          </BlockColumn>
        );
      })}
    </BlockRow>
  );
}

BetSettingHeading.defaultProps = { subType: '' };
