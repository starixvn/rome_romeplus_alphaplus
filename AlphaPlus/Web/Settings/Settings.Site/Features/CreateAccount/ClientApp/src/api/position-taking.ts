import axios from 'axios';

import { siteMemberInfoUrl, UserLevel } from '@/constants';
import { Result, SubMessagesResult } from '@/types';
import { AgentPTConfigItem, GroupItem, MasterPTConfigItem, ProductGroup, SuperPTConfigItem } from '@/types/new-account';
import { CreatePTTemplateResult, PTTemplate } from '@/types/position-taking';

import apiHelpers from './api-helpers';

export const getPTSettings = async (actorLevelId: number) => {
  const { data: productGroups } = await axios.get<ProductGroup[]>(`${siteMemberInfoUrl}PTTemplate/GetPTSettings`);
  const ptSettings = productGroups.map((group) => {
    const subGroups: { [key: number | string]: string } = {};
    const groupItems = group.groupTypes.map((x) => x.groupItems).flat();
    const groupItemsBySubGroup = groupItems.reduce((prev: { [key: number | string]: GroupItem[] }, groupItem) => {
      const { subGroupId, subGroupName } = groupItem;
      const subgroupKey = `+${subGroupId}`;
      prev[subgroupKey] = prev[subgroupKey] ?? [];
      prev[subgroupKey].push(groupItem);
      subGroups[subgroupKey] = subGroups[subgroupKey] ?? subGroupName;
      return prev;
    }, {});

    return {
      groupId: group.id,
      groupName: group.name,
      subGroups: Object.entries(groupItemsBySubGroup).map(([key, value]) => {
        const subGroupId = parseInt(key, 10);
        const subgroupKey = `+${subGroupId}`;
        if (actorLevelId === UserLevel.Super) {
          const max = Math.min(...(value as SuperPTConfigItem[]).map((item) => item.s4));
          const maxN4 = Math.max(...(value as SuperPTConfigItem[]).map((item) => item.n4));
          const minM4 = Math.min(...(value as SuperPTConfigItem[]).map((item) => item.m4));
          return { subGroupId, subGroupName: subGroups[subgroupKey], min: Math.max(0, maxN4 - minM4), max };
        }

        if (actorLevelId === UserLevel.Master) {
          const max = Math.min(...(value as MasterPTConfigItem[]).map((item) => item.m3));
          const maxN3 = Math.max(...(value as MasterPTConfigItem[]).map((item) => item.n3));
          const minA4 = Math.min(...(value as MasterPTConfigItem[]).map((item) => item.a4));
          return { subGroupId, subGroupName: subGroups[subgroupKey], min: Math.max(0, maxN3 - minA4), max };
        }

        const min = Math.max(...(value as AgentPTConfigItem[]).map((item) => item.n2));
        const max = Math.min(...(value as AgentPTConfigItem[]).map((item) => item.a2));
        return { subGroupId, subGroupName: subGroups[subgroupKey], min, max };
      }),
    };
  });

  return ptSettings;
};

export const getPTTemplates = async () => axios.get<PTTemplate[]>(`${siteMemberInfoUrl}PTTemplate/GetPTTemplates`);

export const createPTTemplate = async (data: PTTemplate) =>
  axios.post<CreatePTTemplateResult>(`${siteMemberInfoUrl}PTTemplate/Create`, data, {
    headers: apiHelpers.getHeadersForPostRequest(),
  });

export const editPTTemplate = async (data: PTTemplate) =>
  axios.post<SubMessagesResult>(`${siteMemberInfoUrl}PTTemplate/Edit`, data, {
    headers: apiHelpers.getHeadersForPostRequest(),
  });

export const deletePTTemplate = async (templateId: number) =>
  axios.post<Result>(
    `${siteMemberInfoUrl}PTTemplate/Delete/${templateId}`,
    {},
    {
      headers: apiHelpers.getHeadersForPostRequest(),
    },
  );
