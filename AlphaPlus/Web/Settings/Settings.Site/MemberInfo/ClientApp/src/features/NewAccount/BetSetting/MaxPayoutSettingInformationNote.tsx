import { t } from 'i18next';

import { oldMainUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';

export default function MaxPayoutSettingInformationNote() {
  const url = `${oldMainUrl}/ViewMaxPayoutSettingInformation/Index.aspx`;
  const tooltipContent = t('MaxPayoutInfoTitle');

  return (
    <a
      href={url}
      target="_blank"
      rel="noopener noreferrer"
      data-tooltip-id="global-tooltip"
      data-tooltip-content={tooltipContent}
    >
      {IconInfo}
    </a>
  );
}
