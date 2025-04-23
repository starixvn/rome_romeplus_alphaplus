import { t } from 'i18next';

import { customerSettingInfoRootUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';

export default function PageInformationNote() {
  const theme = useNewAccountStore((state) => state.theme);
  const generalInfo = useNewAccountStore((state) => state.generalInfo);
  const url = `${customerSettingInfoRootUrl}?tabindex=4&theme=${theme}&custId=${generalInfo.uplineCustId}&roleId=${
    generalInfo?.uplineRoleId - 1
  }&site=${generalInfo.site}&siteLabel=${generalInfo.siteLabel}`;
  return (
    <>
      <a href={url} target="_blank" rel="noopener noreferrer" id="productSettingInfo">
        {IconInfo}
      </a>
      <WrapperTooltip anchorSelect="#productSettingInfo">{t('ProductSettingInfoTitle')}</WrapperTooltip>
    </>
  );
}
