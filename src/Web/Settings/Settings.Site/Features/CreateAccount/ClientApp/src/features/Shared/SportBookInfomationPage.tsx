import { t } from 'i18next';
import { MouseEvent } from 'react';

import { customerSettingInfoRootUrl } from "@/constants";
import { IconInfo } from '@/features/Shared/Icons';
import useNewAccountStore from "@/store/newAccountStore";

interface UseSportBookInfomationPageProps {
  tabName: keyof typeof TabNames;
  tooltipContentI18nKey: string;
}

export const TabNames = {
  BetSettingTab: 'BetSettingTab', //0
  MaxPayoutSettingTab: 'MaxPayoutSettingTab', //1
  PTSettingTab: 'PTSettingTab',//2
  CommissionHintTab: 'CommissionHintTab',//3
  ProductCategorySettingTab: 'ProductCategorySettingTab', //4
  WinLossLimitTab: 'WinLossLimitTab',//5
  PotentialWinLimitSettingTab: 'PotentialWinLimitSettingTab',//6
  MaxWinSettingTab: 'MaxWinSettingTab',//7
};

export const SportBookInfomationPage = ({ tabName, tooltipContentI18nKey }: UseSportBookInfomationPageProps) => {
  const theme = useNewAccountStore((state) => state.theme);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);

  const url = `${customerSettingInfoRootUrl}?tabName=${tabName}&theme=${theme}&custId=${generalInfo.uplineCustId}&roleId=${
    generalInfo?.uplineRoleId - 1
  }&site=${generalInfo.site}&siteLabel=${generalInfo.siteLabel}`;

  const handleClick = (e: MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    window.open(url, '_blank');
    e.stopPropagation();
  };

  const tooltipContent = t(tooltipContentI18nKey);
  return (
    <span onClick={handleClick} style={{ cursor: 'pointer' }} aria-hidden data-tooltip-id="global-tooltip" data-tooltip-content={tooltipContent}>
      {IconInfo}
    </span>
  );
}
