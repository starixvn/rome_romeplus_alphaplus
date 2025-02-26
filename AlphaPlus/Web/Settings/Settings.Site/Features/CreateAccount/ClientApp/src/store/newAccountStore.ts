import { create } from 'zustand';

import createBetSettingSlice, { BetSettingSlice } from './betSettingSlice';
import createCommissionSlice, { CommissionSlice } from './commissionSlice';
import createGeneralInfoSlice, { GeneralInfoSlice } from './generalInfoSlice';
import createNewAccountSlice, { NewAccountSlice } from './newAccountSlice';
import createParlayMaxPayoutSlice, { ParlayMaxPayoutSlice } from './parlayMaxPayoutSlice';
import createPositionTakingSlice, { PositionTakingSlice } from './positionTakingSlice';
import createSportsBookCommissionSlice, {
    SportsBookCommissionSlice
} from './sportsBookCommissionSlice';

type AppState = GeneralInfoSlice &
  SportsBookCommissionSlice &
  CommissionSlice &
  BetSettingSlice &
  ParlayMaxPayoutSlice &
  PositionTakingSlice &
  NewAccountSlice;

const useNewAccountStore = create<AppState>()((...a) => ({
  ...createGeneralInfoSlice(...a),
  ...createSportsBookCommissionSlice(...a),
  ...createCommissionSlice(...a),
  ...createBetSettingSlice(...a),
  ...createParlayMaxPayoutSlice(...a),
  ...createPositionTakingSlice(...a),
  ...createNewAccountSlice(...a),
}));

export default useNewAccountStore;
