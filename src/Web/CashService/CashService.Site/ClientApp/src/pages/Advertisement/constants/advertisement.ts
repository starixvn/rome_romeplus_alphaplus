export interface StepConfig {
  id: number;
  title: string;
  image?: string;
  video?: string;
  description?: string;
  className?: string;
  isSpecial?: boolean;
}

export const STEP_CONFIGS: StepConfig[] = [
  {
    id: 0,
    title: 'InstantDepositsAndWithdrawals',
    image: 'step1.jpg',
    description: 'EasilyAddAPaymentGatewayAndBankAccountsSoYourMembersCanStartPlayingInstantly'
  },
  {
    id: 1,
    title: 'ManageCreditAndCashMember',
    image: 'step2.jpg',
    description: 'MaximiseYourEarningsByManagingBothCreditAndCashInSingleAccount'
  },
  {
    id: 2,
    title: 'DedicatedDomain',
    video: 'step3.mp4',
    description: 'ElevateYourBrandWithASignUpLinkedForMemberRegistrations',
    className: 'support-scale'
  },
  {
    id: 3,
    title: 'CustomisePromotions',
    image: 'step4.jpg',
    description: 'CreateAndManagePromotionsFeaturingPopularProductsAndProvidersToKeepYourMembersEntertained'
  },
  {
    id: 4,
    title: 'SuperchargeYourCashFlow',
    isSpecial: true
  }
];

export const MESSAGE_TYPES = {
  OPEN_POPUP: 'openCashServicePopup',
  FIND_OUT_MORE: 'triggerFindOutMore',
  ACTIVATE_NOW: 'triggerActivateNow',
  GO_TO_STEP: 'triggerGoToStep',
  CLOSE: 'triggerClose'
} as const;

export const CONTENT_FRAME = {
  MAIN: 'MainContent',
  TOUR_GUIDE: 'TourGuide'
} as const;

export const STORAGE_KEYS = {
  HIDE_ADS: (nickName: string) => `${nickName}-hideCashServiceAds`
} as const;
