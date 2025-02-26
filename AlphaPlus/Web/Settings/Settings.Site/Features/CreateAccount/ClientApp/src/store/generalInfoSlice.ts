import { StateCreator } from 'zustand';

import { SelectAtLeastDay } from '@/features/NewAccount/TransferCondition';
import { GeneralInfo } from '@/types/new-account';

export type GeneralInfoSlice = {
  userLevel: number;
  siteLabel: string;
  disableAutoMPT: boolean;
  isAutomateTransfer: boolean;
  generalInfo: GeneralInfo;
  group: string;
  displayTransferCondition: boolean;
  transferConditionErrors: string[];
  updateGeneralInfo: (data: Partial<GeneralInfo>) => void;
  validateTransferCondition: () => boolean;
};

const createGeneralInfoSlice: StateCreator<GeneralInfoSlice, [], [], GeneralInfoSlice> = (set, get) => ({
  userLevel: 0,
  siteLabel: '',
  disableAutoMPT: false,
  isAutomateTransfer: false,
  generalInfo: {} as GeneralInfo,
  group: '',
  displayTransferCondition: false,
  transferConditionErrors: [],
  updateGeneralInfo: (data) =>
    set((state) => ({
      generalInfo: {
        ...state.generalInfo,
        ...data,
      },
    })),
  validateTransferCondition: () => {
    const { transferOption } = get().generalInfo;

    const isWeeklyOptionWithoutSelectedDays = '00000001';
    let errorId = '';
    if (transferOption === isWeeklyOptionWithoutSelectedDays) {
      errorId = `${SelectAtLeastDay}_${Date.now().toString()}`;
    }

    if (errorId) {
      set({ transferConditionErrors: [errorId, ...get().transferConditionErrors] });
    }
    return errorId === '';
  },
});

export default createGeneralInfoSlice;
