import cn from 'classnames';
import { ChangeEvent, useEffect, useRef, useState } from 'react';
import { NumericFormat } from 'react-number-format';

import { MaxNumberInputLength } from '@/constants/product';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';
import { GroupItem } from '@/types/new-account';
import { formatThousandNumber, getNumberOfCommaString } from '@/utils/number-format';

import { Operator } from './type';

type BetSettingInputProps = {
  value: string;
  tooltipId: string;
  tooltipContent: string;
  onChange: (e: ChangeEvent<HTMLInputElement>) => void;
  onFocus: () => void;
  onBlur: () => void;
};

function BetSettingNumericInput({ value, tooltipId, tooltipContent, onChange, onFocus, onBlur }: BetSettingInputProps) {
  return (
    <NumericFormat
      className="number"
      data-tooltip-id={tooltipId}
      data-tooltip-content={tooltipContent}
      value={value}
      maxLength={MaxNumberInputLength}
      onChange={onChange}
      onFocus={onFocus}
      onBlur={onBlur}
      allowNegative={false}
      decimalScale={2}
      thousandSeparator=","
    />
  );
}

type BetSettingRowBlockProps<T> = {
  groupTypeId: number;
  groupItem: T;
  validate: (configs: T, newBetSettings: T, prop: keyof T) => { isValid: boolean; message: string };
  property: keyof T;
  isCoping?: boolean;
};

export function BetSettingRowBlock<T extends GroupItem>({
  groupTypeId,
  groupItem,
  property,
  isCoping,
  validate,
}: BetSettingRowBlockProps<T>) {
  const componentId = `${groupItem.sportTypeId}-${groupItem.betTypeId}-${property as string}`;
  const operator = property === 'minBet' ? Operator.GreaterEqual : Operator.LessEqual;
  const groupItemData = groupItem[property];
  const error = useNewAccountStore((state) => state.betSettingErrors.find((item) => item === componentId));
  const value = useNewAccountStore(
    (state) =>
      state.betSettings[groupTypeId].groupItems.find(
        (item) => item.sportTypeId === groupItem.sportTypeId && item.betTypeId === groupItem.betTypeId,
      ) as T,
  );
  const clearError = useNewAccountStore((state) => state.clearBetSettingError);
  const updateBetSettingItem = useNewAccountStore((state) => state.updateBetSettingItem);
  const [isValid, setIsValid] = useState(true);
  const [isFocus, setIsFocus] = useState(false);
  const errorMessage = useRef('');

  useEffect(() => {
    const validData = validate(groupItem, value, property);
    errorMessage.current = validData.message;
    setIsValid(validData.isValid);
  }, [groupItem, groupTypeId, isValid, property, value]);

  const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
    const newValue = e.target.value ? getNumberOfCommaString(e.target.value) : undefined;
    updateBetSettingItem(groupTypeId, { ...value, [property]: newValue });
    const validData = validate(groupItem, { ...value, [property]: newValue }, property);
    errorMessage.current = validData.message;
    setIsValid(validData.isValid);
  };

  return (
    <>
      {groupItemData !== null && (
        <div className={cn({ copy: isCoping && isValid }, { error: !isValid || error === componentId })}>
          <BetSettingNumericInput
            tooltipId={componentId}
            tooltipContent={errorMessage.current}
            value={value[property] as string}
            onChange={handleOnChange}
            onFocus={() => {
              setIsFocus(true);
              clearError();
            }}
            onBlur={() => setIsFocus(false)}
          />
          <WrapperTooltip
            content={errorMessage.current}
            type="error"
            id={componentId}
            isOpen={(!isValid && isFocus) || error === componentId}
          />
        </div>
      )}
      {groupItemData ? `${operator} ${formatThousandNumber(groupItemData as string)}` : ''}
    </>
  );
}
