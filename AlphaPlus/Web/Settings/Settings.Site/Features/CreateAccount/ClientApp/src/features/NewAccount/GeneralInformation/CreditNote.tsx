import { useTranslation } from 'react-i18next';

import { formatNumber } from '@/utils/number-format';

type Props = {
  availableCredit: number;
  memberMaxCredit: number;
};

export default function CreditNote({ availableCredit, memberMaxCredit }: Props) {
  const { t } = useTranslation();
  const note = t('creditNote', {
    availableCredit: formatNumber(availableCredit),
    memberMaxCredit: formatNumber(memberMaxCredit),
  });

  return (
    <span
      className="icon icon-credit"
      style={{ margin: '0 0 0 5px', fontSize: '18px' }}
      data-tooltip-id="global-tooltip"
      data-tooltip-html={note}
    />
  );
}
