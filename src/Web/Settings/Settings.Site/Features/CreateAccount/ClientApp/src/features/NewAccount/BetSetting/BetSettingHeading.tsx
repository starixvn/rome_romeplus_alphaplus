import { useTranslation } from 'react-i18next';

import { BlockColumn, BlockRow } from '@/features/Shared/Block';

import MaxWinSettingInformationNote from './MaxWinSettingInformationNote';

export default function BetSettingHeading({ properties }: { properties: string[] }) {
  const { t } = useTranslation();
  return (
    <BlockRow isHeaderRow>
      {[...properties].map((property) => {
        return (
          <BlockColumn key={property}>
            {t(property === 'maxPayoutPerMatch' ? 'maxWinPerMatch' : property)} {property === 'maxPayoutPerMatch' && <MaxWinSettingInformationNote />}
          </BlockColumn>
        );
      })}
    </BlockRow>
  );
}

BetSettingHeading.defaultProps = { subType: '' };
