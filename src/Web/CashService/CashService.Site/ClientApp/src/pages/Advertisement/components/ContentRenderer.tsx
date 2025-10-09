import React from 'react';
import { CONTENT_FRAME } from '../constants/advertisement';
import { CashServiceStatus } from '../MainBanner';
import MainContent from './MainContent';
import StepSlider from './StepSlider';
import { AnalyzeCashServiceAds } from '../AnalzeCash';

interface ContentRendererProps {
  contentFrame: string;
  cashServiceStatus: CashServiceStatus;
  actions: {
    findOutMore: () => void;
    activateNow: () => void;
    goToStep: (step: number) => void;
    close: () => void;
  };
  onDontShowAgain?: () => void;
  steps: React.ReactNode[];
  currentStep: number;
  onStepChange: (step: number) => void;
  onConfirm?: () => void;
  nickName: string;
}

const ContentRenderer: React.FC<ContentRendererProps> = ({
  contentFrame,
  cashServiceStatus,
  actions,
  onDontShowAgain,
  steps,
  currentStep,
  onStepChange,
  onConfirm,
  nickName
}) => {
  const handleStepSliderClose = () => {
    actions.close();
    AnalyzeCashServiceAds({
      name: nickName || '',
      step: currentStep + 1,
      act: 'close'
    });
  };

  if (contentFrame === CONTENT_FRAME.MAIN) {
    return (
      <MainContent
        onClickActivateNow={cashServiceStatus.isCanUpdateCashServiceStatus ? actions.activateNow : undefined}
        findOutMore={actions.findOutMore}
        onDontShowAgain={onDontShowAgain}
      />
    );
  }

  if (contentFrame === CONTENT_FRAME.TOUR_GUIDE) {
    return (
      <StepSlider
        steps={steps}
        currentStep={currentStep}
        onStepChange={onStepChange}
        onClose={handleStepSliderClose}
        onFinish={cashServiceStatus.isCanUpdateCashServiceStatus ? onConfirm : undefined}
      />
    );
  }

  return null;
};

export default ContentRenderer;
