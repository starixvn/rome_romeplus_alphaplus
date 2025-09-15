import { t } from 'i18next';

import { customerSettingInfoRootUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';
import useNewAccountStore from "@/store/newAccountStore";

export default function MaxWinSettingInformationNote() {
  const theme = useNewAccountStore((state) => state.theme);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);

  const url = `${customerSettingInfoRootUrl}?tabName=MaxWinSettingTab&theme=${theme}&custId=${generalInfo.uplineCustId}&roleId=${
    generalInfo?.uplineRoleId - 1
  }&site=${generalInfo.site}&siteLabel=${generalInfo.siteLabel}`;

  const tooltipContent = t('MaxWinSettingTab');

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
