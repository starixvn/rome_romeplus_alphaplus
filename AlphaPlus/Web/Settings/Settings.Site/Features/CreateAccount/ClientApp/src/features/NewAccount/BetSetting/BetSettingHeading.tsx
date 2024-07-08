import { useTranslation } from 'react-i18next';

import { BlockColumn, BlockRow } from '@/features/Shared/Block';

import MaxPayoutSettingInformationNote from './MaxPayoutSettingInformationNote';

export default function BetSettingHeading({ properties }: { properties: string[] }) {
  const { t } = useTranslation();
  return (
    <BlockRow isHeaderRow>
      {[...properties].map((property) => {
        return (
          <BlockColumn key={property}>
            {t(property)} {property === 'maxPayoutPerMatch' && <MaxPayoutSettingInformationNote />}
          </BlockColumn>
        );
      })}
    </BlockRow>
  );
}

BetSettingHeading.defaultProps = { subType: '' };
