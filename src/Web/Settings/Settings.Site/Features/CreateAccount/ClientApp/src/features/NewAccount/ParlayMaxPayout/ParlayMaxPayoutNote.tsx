import { t } from 'i18next';
import { MouseEvent } from 'react';

import { customerSettingInfoRootUrl } from "@/constants";
import { IconInfo } from '@/features/Shared/Icons';
import useNewAccountStore from "@/store/newAccountStore";

export default function ParlayMaxPayoutNote() {
  const theme = useNewAccountStore((state) => state.theme);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);

  const url = `${customerSettingInfoRootUrl}?tabName=MaxPayoutSettingTab&theme=${theme}&custId=${generalInfo.uplineCustId}&roleId=${
    generalInfo?.uplineRoleId - 1
  }&site=${generalInfo.site}&siteLabel=${generalInfo.siteLabel}`;

  const handleClick = (e: MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    window.open(url, '_blank');
    e.stopPropagation();
  };

  const tooltipContent = t('MaxPayoutInfoTitle');
  return (
    <span onClick={handleClick} aria-hidden data-tooltip-id="global-tooltip" data-tooltip-content={tooltipContent}>
      {IconInfo}
    </span>
  );
}
