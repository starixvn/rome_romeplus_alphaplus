import { useTranslation } from 'react-i18next';

import { UserLevel } from '@/constants';
import useNewAccountStore from '@/store/newAccountStore';

export default function PasswordRule() {
  const { t } = useTranslation();
  const upLineRoleId = useNewAccountStore((state) => state.generalInfo.uplineRoleId);
  const passwordRule = upLineRoleId === UserLevel.Agent ? t('lblPasswordMemberRequire') : t('lblPasswordRequire');

  return (
    <span className="icon-information-outline" data-tooltip-id="global-tooltip" data-tooltip-html={passwordRule} />
  );
}
