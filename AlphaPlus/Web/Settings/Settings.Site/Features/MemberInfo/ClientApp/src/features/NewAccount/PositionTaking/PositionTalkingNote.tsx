import { t } from 'i18next';
import { MouseEvent } from 'react';

import { oldMainUrl } from '@/constants';
import { IconInfo } from '@/features/Shared/Icons';

export default function PositionTalkingNote() {
  const url = `${oldMainUrl}/PositionTakingInformation/PositionTakingBetTypeReference`;

  const handleClick = (e: MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    window.open(url, '_blank');
    e.stopPropagation();
  };

  const tooltipContent = t('PTInfoTitle');
  return (
    <span onClick={handleClick} aria-hidden data-tooltip-id="global-tooltip" data-tooltip-content={tooltipContent}>
      {IconInfo}
    </span>
  );
}
