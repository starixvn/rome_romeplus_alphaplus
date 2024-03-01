import { CommissionTypeIds, ProductGroupIds } from '@/constants/product';
import { CommissionItem, GroupTypeDict, ProductGroupDict } from '@/types/new-account';

export const initCommissionState = (productGroupsDict: ProductGroupDict) => {
  const commissions = Object.keys(CommissionTypeIds).reduce((acc: GroupTypeDict, currentProduct) => {
    const productGroupId = Number(currentProduct);
    const commissionGroup = productGroupsDict[productGroupId]?.groupTypes.find(
      (groupType) => groupType.id === CommissionTypeIds[productGroupId],
    );

    if (commissionGroup) {
      const comItems = commissionGroup.groupItems;
      if (comItems.length > 0 && comItems[0].subGroupId !== null) {
        let subGroupIds: number[] = [];
        let groupItems: CommissionItem[] = [];
        comItems.forEach((item) => {
          if (!subGroupIds.includes(item.subGroupId)) {
            subGroupIds = [...subGroupIds, item.subGroupId];
          }
        });

        subGroupIds.forEach((subGroupId) => {
          const subGroupItems = comItems.filter((item) => item.subGroupId === subGroupId);
          const discount = Math.min(...subGroupItems.map((item) => (item as CommissionItem).discount));
          subGroupItems.forEach((item) => groupItems.push({ ...(item as CommissionItem), discount }));
        });

        acc[productGroupId] = {
          ...commissionGroup,
          groupItems,
        };
      } else {
        acc[productGroupId] = commissionGroup;
      }
    }

    return acc;
  }, {});

  return {
    sportsBookCommissions: {
      [ProductGroupIds.SportsBook]: commissions[ProductGroupIds.SportsBook],
    },
    commissions: {
      [ProductGroupIds.LiveRNGGames]: commissions[ProductGroupIds.LiveRNGGames],
      [ProductGroupIds.OthersGames]: commissions[ProductGroupIds.OthersGames],
    },
  };
};
