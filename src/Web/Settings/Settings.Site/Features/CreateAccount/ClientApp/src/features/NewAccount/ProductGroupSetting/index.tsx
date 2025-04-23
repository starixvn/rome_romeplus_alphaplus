import React from 'react';
import { useParams } from 'react-router-dom';
import { shallow } from 'zustand/shallow';

import { site3Decimals, site789y, UserLevel } from '@/constants';
import { GroupTypeIds } from '@/constants/product';
import useNewAccountStore from '@/store/newAccountStore';
import {
  CommissionItem,
  GroupType,
  OtherGamesBetSettingItem,
  ParlayMaxPayoutItem,
  RNGGamesBetSettingItem,
  SportsBookBetSettingItem,
} from '@/types/new-account';

import OtherGamesBetSetting from '../BetSetting/OtherGamesBetSetting';
import RNGGamesBetSetting from '../BetSetting/RNGGamesBetSetting';
import SportsBookBetSetting from '../BetSetting/SportsBookBetSetting';
import Commission from '../Commission';
import SportsBookCommission from '../Commission/SportsBook';
import ParlayMaxPayout from '../ParlayMaxPayout';
import AgentPTSettings from '../PositionTaking/AgentPTSettings';
import MasterPTSettings from '../PositionTaking/MasterPTSettings';
import SuperPTSettings from '../PositionTaking/SuperPTSettings';

function RenderSettings(
  userLevel: number,
  is789y: boolean,
  is3Decimals: boolean,
  productGroupId: number,
  groupType: GroupType,
) {
  const { id, groupItems } = groupType;

  switch (id) {
    case GroupTypeIds.SportsBookCommission:
      return <SportsBookCommission groupItems={groupItems as CommissionItem[]} />;
    case GroupTypeIds.SportsBookBetSetting:
      return (
        <SportsBookBetSetting is789y={is789y} groupTypeId={id} groupItems={groupItems as SportsBookBetSettingItem[]} />
      );
    case GroupTypeIds.ParlayMaxPayout:
      return <ParlayMaxPayout groupTypeId={id} groupItems={groupItems as ParlayMaxPayoutItem[]} />;
    case GroupTypeIds.PositionTaking:
      if (userLevel === UserLevel.Super) {
        return <SuperPTSettings is3DecimalSite={is3Decimals} productGroupId={productGroupId} />;
      }
      if (userLevel === UserLevel.Master) {
        return <MasterPTSettings is3DecimalSite={is3Decimals} productGroupId={productGroupId} />;
      }
      if (userLevel === UserLevel.Agent) {
        return <AgentPTSettings is3DecimalSite={is3Decimals} productGroupId={productGroupId} />;
      }
      return '';
    case GroupTypeIds.RNGGamesBetSetting:
      return (
        <RNGGamesBetSetting is789y={is789y} groupTypeId={id} groupItems={groupItems as RNGGamesBetSettingItem[]} />
      );
    case GroupTypeIds.Commission:
      return <Commission productGroupId={productGroupId} groupItems={groupItems as CommissionItem[]} />;
    case GroupTypeIds.OtherGamesBetSetting:
      return (
        <OtherGamesBetSetting is789y={is789y} groupTypeId={id} groupItems={groupItems as OtherGamesBetSettingItem[]} />
      );
    default:
      return '';
  }
}

export default function ProductGroupSetting() {
  const { productId } = useParams();
  const [siteLabel, userLevel, productGroups] = useNewAccountStore(
    (state) => [state.siteLabel, state.userLevel, state.productGroupsDict],
    shallow,
  );

  const is789y = siteLabel === site789y;
  const isSupportPT3Decimals = site3Decimals
    .split(',')
    .map((item) => item.trim().toLocaleLowerCase())
    .includes(siteLabel);
  const productGroupId = Number(productId) || 1;
  const groupTypes = productGroups[productGroupId]?.groupTypes ?? [];
  return (
    <>
      {groupTypes.map(
        (groupType) =>
          groupType &&
          groupType.groupItems.length > 0 && (
            <React.Fragment key={`${productGroupId}-${groupType?.id}`}>
              {RenderSettings(userLevel, is789y, isSupportPT3Decimals, productGroupId, groupType)}
            </React.Fragment>
          ),
      )}
    </>
  );
}
