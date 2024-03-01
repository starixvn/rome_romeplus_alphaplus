import { ReactNode } from 'react';

import { BlockColumn, BlockRow } from '@/features/Shared/Block';
import { GroupItem } from '@/types/new-account';

import { BetSettingRowBlock } from './BetSettingRowBlock';

type BetSettingRowProps<T> = {
  groupTypeId: number;
  groupItem: T;
  properties: (keyof T | '')[];
  label: string | ReactNode;
  isCoping?: boolean;
  validate: (configs: T, newBetSettings: T, prop: keyof T) => { isValid: boolean; message: string };
};

export default function BetSettingRow<T extends GroupItem>({
  groupTypeId,
  groupItem,
  label,
  properties,
  isCoping,
  validate,
}: BetSettingRowProps<T>) {
  return (
    <BlockRow label={label}>
      {[...properties].map((property) =>
        property === '' ? (
          <BlockColumn key={property}>{null}</BlockColumn>
        ) : (
          <BlockColumn key={property as string}>
            <BetSettingRowBlock
              groupTypeId={groupTypeId}
              groupItem={groupItem}
              property={property}
              isCoping={isCoping}
              validate={validate}
            />
          </BlockColumn>
        ),
      )}
    </BlockRow>
  );
}
