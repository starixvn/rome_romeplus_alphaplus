import classNames from 'classnames';
import { ChangeEvent, useCallback, useEffect, useRef, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { shallow } from 'zustand/shallow';

import { TransferOptions, UserLevel } from '@/constants';
import { Block } from '@/features/Shared/Block';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import useNewAccountStore from '@/store/newAccountStore';

import styles from './TransferCondition.module.scss';

interface WeeklyOptions {
  [index: string]: boolean;
  mon: boolean;
  tue: boolean;
  wed: boolean;
  thurs: boolean;
  fri: boolean;
  sat: boolean;
  sun: boolean;
}

const defaultWeek = {
  sun: false,
  mon: false,
  tue: false,
  wed: false,
  thurs: false,
  fri: false,
  sat: false,
};

const TooltipSelectAtLeastDay = {
  contentKey: 'chooseOneDay',
  id: 'anchor-tooltip-uuid-01',
  autoHide: false,
};

const TooltipCheckAllOptions = {
  contentKey: 'chooseDaily',
  id: 'anchor-tooltip-uuid-02',
  autoHide: true,
};

export const SelectAtLeastDay = TooltipSelectAtLeastDay.id;

export default function TransferCondition() {
  const { t } = useTranslation();
  const [disabled, userLevel] = useNewAccountStore((state) => [state.isAutomateTransfer, state.userLevel], shallow);
  const [error, submitCount] = useNewAccountStore(
    (state) => [state.transferConditionErrors[0], state.submitCount],
    shallow,
  );
  const updateTransferCondition = useNewAccountStore((state) => state.updateGeneralInfo);

  const [weekly, setWeekly] = useState<boolean>(false);
  const [days, setDays] = useState<WeeklyOptions>(defaultWeek);
  const [openTooltip, setOpenTooltip] = useState(false);
  const [contentTooltip, setContentTooltip] = useState<string>('');
  const [targetTooltip, setTargetTooltip] = useState<string>('');
  const timeoutRef = useRef<NodeJS.Timeout | null>(null);

  const TooltipDisableTransferCondition = {
    contentKey:
      userLevel === UserLevel.Super
        ? t('TransferConditionByCompany').toString()
        : t('TransferConditionByUpline').toString(),
    id: 'anchor-tooltip-uuid-02',
    autoHide: true,
  };

  const showTooltip = useCallback((tooltip: { contentKey: string; id: string; autoHide: boolean }) => {
    setTargetTooltip(tooltip.id);
    setContentTooltip(tooltip.contentKey);
    setOpenTooltip(true);

    // Clear the previous timeout if it exists
    if (timeoutRef.current) {
      clearTimeout(timeoutRef.current);
    }

    // Start a new timeout if isAutoHide is true
    if (tooltip.autoHide) {
      timeoutRef.current = setTimeout(() => {
        setOpenTooltip(false);
      }, 3000);
    }
  }, []);

  const updateGlobalState = (values: WeeklyOptions, isWeekly: boolean) => {
    const daysArray = Object.values(values);
    const weeklyOptions = daysArray.map((day) => (day ? '1' : '0')).join('');
    updateTransferCondition({ transferOption: `${weeklyOptions}${isWeekly ? '1' : '0'}` });
  };

  const onClickCheckboxWeekly = () => {
    setWeekly(true);
    updateGlobalState(days, true);
  };

  const onClickCheckboxDaily = () => {
    setWeekly(false);
    updateTransferCondition({ transferOption: TransferOptions.Daily });
    if (openTooltip) {
      setOpenTooltip(false);
    }
  };

  const handleDayChange = (event: ChangeEvent<HTMLInputElement>) => {
    const { name, checked } = event.target;
    const options = {
      ...days,
      [name]: checked,
    };

    const checkAllOptions = !Object.values(options).includes(false);
    if (checkAllOptions) {
      showTooltip(TooltipCheckAllOptions);
      return;
    }
    const unCheckAllOptions = !Object.values(options).includes(true);
    if (unCheckAllOptions) {
      showTooltip(TooltipSelectAtLeastDay);
    } else {
      setOpenTooltip(false);
    }

    setDays(options);
    updateGlobalState(options, true);
  };

  useEffect(() => {
    if (error?.includes(TooltipSelectAtLeastDay.id)) {
      showTooltip(TooltipSelectAtLeastDay);
    } else {
      setOpenTooltip(false);
    }
  }, [error, showTooltip]);

  useEffect(() => {
    if (disabled) {
      setTargetTooltip(TooltipDisableTransferCondition.id);
      setContentTooltip(TooltipDisableTransferCondition.contentKey);
    }
  }, [TooltipDisableTransferCondition.contentKey, TooltipDisableTransferCondition.id, disabled]);

  return (
    <Block title={t('accBalanceTransferCondition')} isCollapse={{ collapse: error === '', resetCollapse: submitCount }}>
      <div
        className={classNames(styles.container, { [styles.disabled]: disabled })}
        onMouseEnter={() => disabled && setOpenTooltip(true)}
        onMouseLeave={() => disabled && setOpenTooltip(false)}
      >
        <div
          aria-hidden
          className={classNames(styles.box, styles.daily, { [styles.active]: !weekly })}
          onClick={() => !disabled && onClickCheckboxDaily()}
          onBlur={() => setOpenTooltip(false)}
        >
          <label className={styles['btn-box']} htmlFor="daily" id={TooltipCheckAllOptions.id}>
            <span className={`icon ${styles.icon} icon-time`} />
            <span className={styles.text}>{t('daily')}</span>
          </label>
        </div>
        <div
          aria-hidden
          className={classNames(styles.box, styles.weekly, { [styles.active]: weekly })}
          onClick={() => !disabled && onClickCheckboxWeekly()}
          onBlur={() => setOpenTooltip(false)}
        >
          <label className={styles['btn-box']} htmlFor="weekly">
            <span className={`icon ${styles.icon} icon-viewlog`} />
            <span className={styles.text}>{t('weekly')}</span>
          </label>
          <ul className={styles['list-day']}>
            {['mon', 'tue', 'wed', 'thurs', 'fri', 'sat', 'sun'].map((day) => (
              <li key={day}>
                <label htmlFor={day}>
                  <input
                    id={day}
                    type="checkbox"
                    name={day}
                    disabled={!weekly}
                    checked={days[day]}
                    onChange={handleDayChange}
                  />
                  <span id={day === 'mon' ? TooltipSelectAtLeastDay.id : ''}>{t(day)}</span>
                </label>
              </li>
            ))}
          </ul>
        </div>

        <WrapperTooltip
          anchorSelect={`#${targetTooltip}`}
          isOpen={openTooltip}
          content={t(contentTooltip).toString()}
          type="error"
        />
      </div>
    </Block>
  );
}
