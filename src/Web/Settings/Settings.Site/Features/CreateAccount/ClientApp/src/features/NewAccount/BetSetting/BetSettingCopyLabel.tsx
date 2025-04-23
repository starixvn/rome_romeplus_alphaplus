import cn from 'classnames';
import { useTranslation } from 'react-i18next';

export function renderCopyLabel(rowDisplayName: string, isCoping: boolean, handleCopyBetSettings: () => void) {
  return (
    <BetSettingCopyLabel label={rowDisplayName} isCoping={isCoping} handleCopyBetSettings={handleCopyBetSettings} />
  );
}

export default function BetSettingCopyLabel({
  label,
  isCoping,
  handleCopyBetSettings,
}: {
  label: string;
  isCoping: boolean;
  handleCopyBetSettings: () => void;
}) {
  const { t } = useTranslation();

  const tooltipContent = t('CopyTooltip');
  const iconClass = isCoping ? 'icon-checked-copy' : 'icon-content-copy';
  return (
    <>
      {label}
      <span
        className={cn('icon', iconClass)}
        aria-hidden
        onClick={handleCopyBetSettings}
        style={{ float: 'right', marginTop: '3px' }}
        data-tooltip-id="global-tooltip"
        data-tooltip-content={tooltipContent}
      />
    </>
  );
}

BetSettingCopyLabel.defaultProps = { copiedId: '' };
