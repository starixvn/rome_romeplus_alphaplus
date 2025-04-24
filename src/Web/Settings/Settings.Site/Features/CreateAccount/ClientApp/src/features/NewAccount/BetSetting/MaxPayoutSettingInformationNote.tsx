import { t } from 'i18next';

import { customerSettingInfoRootUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';
import useNewAccountStore from "@/store/newAccountStore";

export default function MaxPayoutSettingInformationNote() {
  const theme = useNewAccountStore((state) => state.theme);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);

  const url = `${customerSettingInfoRootUrl}?tabindex=1&theme=${theme}&custId=${generalInfo.uplineCustId}&roleId=${
    generalInfo?.uplineRoleId - 1
  }&site=${generalInfo.site}&siteLabel=${generalInfo.siteLabel}`;

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
