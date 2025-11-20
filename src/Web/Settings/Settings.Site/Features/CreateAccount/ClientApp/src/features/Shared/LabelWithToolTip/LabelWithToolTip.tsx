import React from 'react';

import styles from './LabelWithToolTip.module.scss';

interface LabelWithTooltipProps {
  label: React.ReactNode;
  tooltipMessage?: string;
}

export function LabelWithTooltip({ label, tooltipMessage }: LabelWithTooltipProps) {
  return (
    <>
      {label}
      {tooltipMessage?.trim() && (
        <span 
          className={`icon-information-outline ${styles.tooltipIcon}`}
          data-tooltip-id="global-tooltip" 
          data-tooltip-content={tooltipMessage}
          tabIndex={0} 
        />
      )}
    </>
  );
}
