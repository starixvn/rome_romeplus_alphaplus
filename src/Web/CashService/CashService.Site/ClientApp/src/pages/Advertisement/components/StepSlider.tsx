import React from 'react';
import { useTranslation } from 'react-i18next';
import classNames from 'classnames';
import styles from '../cash-service-ads.module.scss';

interface StepSliderProps {
  steps: React.ReactNode[];
  currentStep: number;
  onStepChange: (step: number) => void;
  onClose?: () => void;
  onFinish?: () => void;
}

export const StepSlider: React.FC<StepSliderProps> = ({ 
  steps, 
  currentStep, 
  onStepChange, 
  onClose, 
  onFinish 
}) => {
  const { t } = useTranslation('advertisement');
  const stepIndex = currentStep;
  const isLast = stepIndex === steps.length - 1;
  const isFirst = stepIndex === 0;

  const buttonClasses = {
    back: classNames(styles['step-button'], styles['back-button']),
    next: classNames(styles['step-button'], styles['next-button'])
  };

  return (
    <div className={styles['step-slider']}>
      {onClose && (
        <button onClick={onClose} className={styles['close-button']}>
          ×
        </button>
      )}

      <div className={styles['step-content']}>{steps[stepIndex]}</div>

      <div className={styles['step-navigation']}>
        <button 
          onClick={() => onStepChange(currentStep - 1)} 
          disabled={isFirst} 
          className={buttonClasses.back}
        >
          {t('Back')}
        </button>
        
        {isLast ? (
          <button onClick={onFinish} disabled={!onFinish} className={buttonClasses.next}>
            {t('Confirm')}
          </button>
        ) : (
          <>
            <button onClick={onFinish} disabled={!onFinish} className={buttonClasses.next}>
              {t('ActivateNow')}
            </button>
            <button onClick={() => onStepChange(currentStep + 1)} className={buttonClasses.back}>
              {t('Next')}
            </button>
          </>
        )}
      </div>
      
      <div className={styles['step-counter']}>
        {currentStep + 1} of {steps.length}
      </div>
    </div>
  );
};