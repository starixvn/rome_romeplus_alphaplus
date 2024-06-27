import cn from 'classnames';
import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { NumericFormat } from 'react-number-format';

import { UserLevel } from '@/constants';
import { MaxNumberInputLength } from '@/constants/product';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';
import { ParlayMaxPayoutItem } from '@/types/new-account';
import { formatThousandNumber, getNumberOfCommaString } from '@/utils/number-format';

import styles from './ParlayMaxPayout.module.scss';

const ParlayMaxPayoutMissConfigValue = 0.1;

export function validateParlayMaxPayoutValue(
  userLevel: UserLevel,
  isAllowUnlimit: boolean,
  parlay: number,
  maxParlay: number,
) {
  if (isAllowUnlimit) {
    return userLevel === UserLevel.Agent ? parlay >= 1 : true;
  }

  const isLessThanMax = maxParlay !== 0 ? parlay <= maxParlay : true;
  return isLessThanMax && parlay >= 1;
}

type Props = {
  groupTypeId: number;
  groupItem: ParlayMaxPayoutItem;
  valueProp: keyof ParlayMaxPayoutItem;
  maxValueProp: keyof ParlayMaxPayoutItem;
  isUnlimitedProp: keyof ParlayMaxPayoutItem;
};

export default function ParlayMaxPayoutRowBlock({
  groupTypeId,
  groupItem,
  valueProp,
  maxValueProp,
  isUnlimitedProp,
}: Props) {
  const { t } = useTranslation();
  const componentId = `${groupItem.sportTypeId}-${groupItem.betTypeId}-${valueProp.toString()}`;
  const value = useNewAccountStore(
    (state) =>
      state.parlayMaxPayouts[groupTypeId].groupItems.find(
        (item) => item.sportTypeId === groupItem.sportTypeId && item.betTypeId === groupItem.betTypeId,
      ) as ParlayMaxPayoutItem,
  );
  const userLevel = useNewAccountStore((state) => state.userLevel);
  const error = useNewAccountStore((state) => state.parlayMaxPayoutErrors.find((item) => item === componentId));
  const updateParlayMaxPayoutItem = useNewAccountStore((state) => state.updateParlayMaxPayoutItem);

  const [isValid, setIsValid] = useState(true);
  const [isOpenTooltip, setIsOpenTooltip] = useState(false);

  useEffect(() => {
    setIsValid(
      validateParlayMaxPayoutValue(
        userLevel,
        Boolean(value[isUnlimitedProp]),
        Number(value[valueProp]),
        Number(value[maxValueProp]),
      ),
    );
  }, [value]);

  const handleOnChange = (e: ChangeEvent<HTMLInputElement>) => {
    const newValue = getNumberOfCommaString(e.target.value);
    updateParlayMaxPayoutItem(groupTypeId, {
      ...value,
      [valueProp]: newValue,
    });
  };

  const handleOnCheckUnlimited = (e: ChangeEvent<HTMLInputElement>) => {
    updateParlayMaxPayoutItem(groupTypeId, {
      ...value,
      [isUnlimitedProp]: e.target.checked,
    });
  };

  return (
    <>
      <div className={styles['full-line']}>
        {value[isUnlimitedProp] && userLevel !== UserLevel.Agent ? (
          <input type="text" value={t('unlimited') ?? ''} disabled />
        ) : (
          <NumericFormat
            className={cn('number', { error: !isValid })}
            data-tooltip-id={componentId}
            data-tooltip-html={
              getNumberOfCommaString(value[valueProp] as string) < 1
                ? t('theValueMustBeAPositiveAmount')
                : t('theSettingOfNameMustBeLessThanOrEqualToUpLine', { name: t(valueProp) })
            }
            maxLength={MaxNumberInputLength}
            value={value[valueProp] as string}
            onChange={handleOnChange}
            allowNegative={false}
            decimalScale={value[valueProp] === ParlayMaxPayoutMissConfigValue ? 1 : 0}
            thousandSeparator=","
            onFocus={() => setIsOpenTooltip(true)}
            onBlur={() => setIsOpenTooltip(false)}
          />
        )}
      </div>

      <div className={styles['max-value']}>
        {Boolean(groupItem[isUnlimitedProp]) === false &&
          ` <= ${formatThousandNumber(groupItem[maxValueProp].toString())}`}
        {groupItem[isUnlimitedProp] && userLevel !== UserLevel.Agent && (
          <div className={styles.inline}>
            <input
              id={`unlimited-input-${componentId}`}
              type="checkbox"
              onChange={handleOnCheckUnlimited}
              checked={value[isUnlimitedProp] as boolean}
            />
            <label htmlFor={`unlimited-input-${componentId}`}>{t('unlimited')}</label>
          </div>
        )}
      </div>
      <WrapperTooltip type="error" id={componentId} isOpen={(!isValid && isOpenTooltip) || error === componentId} />
    </>
  );
}
