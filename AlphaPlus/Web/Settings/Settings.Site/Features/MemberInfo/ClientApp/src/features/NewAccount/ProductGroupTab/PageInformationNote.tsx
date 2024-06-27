import { t } from 'i18next';

import { productSettingInfoRootUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';

export default function PageInformationNote() {
  const theme = useNewAccountStore((state) => state.theme);

  return (
    <>
      <a
        href={`${productSettingInfoRootUrl}/?theme=${theme}`}
        target="_blank"
        rel="noopener noreferrer"
        id="productSettingInfo"
      >
        {IconInfo}
      </a>
      <WrapperTooltip anchorSelect="#productSettingInfo">{t('ProductSettingInfoTitle')}</WrapperTooltip>
    </>
  );
}
