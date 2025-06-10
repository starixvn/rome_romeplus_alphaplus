import 'react-date-range/dist/styles.css';
import 'react-date-range/dist/theme/default.css';

import { enUS } from 'date-fns/locale';
import React from 'react';
import { DateRange, Range, RangeKeyDict } from 'react-date-range';

import { ButtonFullComponent } from './ButtonComponents';
import { CalendarIcon } from './Icons';
import { convertToGMT4 } from '../utils/commonFunctions';
import './DateRangePickerComponent.scss';
import { useTranslation } from 'react-i18next';

export interface DateRangePickerProps {
  value: [Date | null, Date | null];
  onChange: (value: [Date | null, Date | null]) => void;
  className?: string;
  minYear?: number;
  maxRangeDays?: number;
}

function formatDateWithLeadingZeros(date: Date | null): string {
  if (!date) return '';
  const day = date.getDate();
  const month = date.getMonth() + 1;
  const year = date.getFullYear();
  return `${String(month).padStart(2, '0')}/${String(day).padStart(2, '0')}/${year}`;
}

function formatRange(value: [Date | null, Date | null]) {
  if (!value[0] || !value[1]) return '';
  return formatDateWithLeadingZeros(value[0]) + ' - ' + formatDateWithLeadingZeros(value[1]);
}

function getDaysCount(start: Date | null, end: Date | null) {
  if (!start || !end) return 0;
  const diff = Math.abs(end.getTime() - start.getTime());
  return Math.floor(diff / (1000 * 60 * 60 * 24)) + 1;
}

function applyMaxRangeConstraint(
  currentSelection: Range,
  maxDays?: number,
  previousSelection?: Range
): Range {
  let { startDate, endDate, ...rest } = currentSelection;

  if (startDate && endDate && maxDays && maxDays > 0) {
    if (getDaysCount(startDate, endDate) > maxDays) {
      const prevStartDate = previousSelection?.startDate;
      const prevEndDate = previousSelection?.endDate;

      if (previousSelection && prevStartDate && prevEndDate) {
        const startDateChanged = startDate.getTime() !== prevStartDate.getTime();
        const endDateChanged = endDate.getTime() !== prevEndDate.getTime();

        if (startDateChanged && !endDateChanged) {
          startDate = new Date(endDate.getTime() - (maxDays - 1) * 24 * 60 * 60 * 1000);
        } else if (!startDateChanged && endDateChanged) {
          endDate = new Date(startDate.getTime() + (maxDays - 1) * 24 * 60 * 60 * 1000);
        } else {
          endDate = new Date(startDate.getTime() + (maxDays - 1) * 24 * 60 * 60 * 1000);
        }
      } else {
        endDate = new Date(startDate.getTime() + (maxDays - 1) * 24 * 60 * 60 * 1000);
      }
    }
  }
  return { ...rest, startDate, endDate };
}

const quickRanges = [
  {
    label: 'Today',
    range: () => {
      const todayStart = new Date();
      const todayStartInGMT4 = convertToGMT4(todayStart);
      todayStartInGMT4.setHours(0, 0, 0, 0);

      const todayEnd = new Date();
      const todayEndInGMT4 = convertToGMT4(todayEnd);
      todayEndInGMT4.setHours(23, 59, 59, 0);
      return { startDate: todayStartInGMT4, endDate: todayEndInGMT4 };
    },
  },
  {
    label: 'Yesterday',
    range: () => {
      const yesterdayStart = new Date();
      const yesterdayStartInGMT4 = convertToGMT4(yesterdayStart);
      yesterdayStartInGMT4.setDate(yesterdayStartInGMT4.getDate() - 1);
      yesterdayStartInGMT4.setHours(0, 0, 0, 0);

      const yesterdayEnd = new Date();
      const yesterdayEndInGMT4 = convertToGMT4(yesterdayEnd);
      yesterdayEndInGMT4.setDate(yesterdayEndInGMT4.getDate() - 1);
      yesterdayEndInGMT4.setHours(23, 59, 59, 0);
      return { startDate: yesterdayStartInGMT4, endDate: yesterdayEndInGMT4 };
    },
  },
  {
    label: 'Last 7 Days',
    range: () => {
      const end = new Date();
      const endInGMT4 = convertToGMT4(end);
      endInGMT4.setHours(23, 59, 59, 0);

      const sevenDaysAgo = new Date();
      const sevenDaysAgoInGMT4 = convertToGMT4(sevenDaysAgo);
      sevenDaysAgoInGMT4.setDate(sevenDaysAgoInGMT4.getDate() - 6);
      sevenDaysAgoInGMT4.setHours(0, 0, 0, 0);
      return { startDate: sevenDaysAgoInGMT4, endDate: endInGMT4 };
    },
  },
  {
    label: 'Last 30 Days',
    range: () => {
      const end = new Date();
      const start = new Date();
      const startInGMT4 = convertToGMT4(start);
      const endInGMT4 = convertToGMT4(end);
      startInGMT4.setDate(endInGMT4.getDate() - 29);
      startInGMT4.setHours(0, 0, 0, 0);
      endInGMT4.setHours(23, 59, 59, 0);
      return { startDate: startInGMT4, endDate: endInGMT4 };
    },
  },
  {
    label: 'This Month',
    range: () => {
      const now = new Date();
      const nowInGMT4 = convertToGMT4(now);
      const startInGMT4 = new Date(nowInGMT4.getFullYear(), nowInGMT4.getMonth(), 1);
      startInGMT4.setHours(0, 0, 0, 0);

      const endInGMT4 = new Date(nowInGMT4.getFullYear(), nowInGMT4.getMonth() + 1, 0);
      if (endInGMT4 > nowInGMT4) {
        endInGMT4.setDate(nowInGMT4.getDate());
      }
      endInGMT4.setHours(23, 59, 59, 0);

      return { startDate: startInGMT4, endDate: endInGMT4 };
    },
  },
  {
    label: 'Last Month',
    range: () => {
      const now = new Date();
      const nowInGMT4 = convertToGMT4(now);
      const startInGMT4 = new Date(nowInGMT4.getFullYear(), nowInGMT4.getMonth() - 1, 1);
      startInGMT4.setHours(0, 0, 0, 0);
      const endInGMT4 = new Date(nowInGMT4.getFullYear(), nowInGMT4.getMonth(), 0);
      endInGMT4.setHours(23, 59, 59, 0);
      return { startDate: startInGMT4, endDate: endInGMT4 };
    },
  },
];

const DateRangePicker: React.FC<DateRangePickerProps> = ({
  value,
  onChange,
  className,
  minYear,
  maxRangeDays,
}) => {
  const { t } = useTranslation('global');
  const [open, setOpen] = React.useState(false);
  const ref = React.useRef<HTMLDivElement>(null);

  const overallMinDate = React.useMemo(() => {
    return minYear ? new Date(minYear, 0, 1) : undefined;
  }, [minYear]);

  const todayForMaxDateDep = new Date(); // Use stable parts of today for dependency
  const overallMaxDate = React.useMemo(() => {
    const freshNowInGMT4 = convertToGMT4(new Date());
    const maxDate = new Date(freshNowInGMT4);
    maxDate.setDate(freshNowInGMT4.getDate() + 1);
    maxDate.setHours(23, 59, 59, 999);
    return maxDate;
  }, [
    todayForMaxDateDep.getFullYear(),
    todayForMaxDateDep.getMonth(),
    todayForMaxDateDep.getDate(),
  ]);

  const [pendingRange, setPendingRange] = React.useState<Range[]>([
    {
      startDate: value[0] ?? undefined,
      endDate: value[1] ?? undefined,
      key: 'selection',
    },
  ]);

  const [calendarMinDate, setCalendarMinDate] = React.useState<Date | undefined>(overallMinDate);
  const [calendarMaxDate, setCalendarMaxDate] = React.useState<Date | undefined>(overallMaxDate);

  const val0Time = value[0]?.getTime();
  const val1Time = value[1]?.getTime();
  React.useEffect(() => {
    setPendingRange([
      {
        startDate: value[0] ?? undefined,
        endDate: value[1] ?? undefined,
        key: 'selection',
      },
    ]);
    setCalendarMinDate(overallMinDate);
    setCalendarMaxDate(overallMaxDate);
  }, [val0Time, val1Time, overallMinDate, overallMaxDate]);

  React.useEffect(() => {
    if (!open) {
      setCalendarMinDate(overallMinDate);
      setCalendarMaxDate(overallMaxDate);
    }
  }, [open, overallMinDate, overallMaxDate]);

  const handleApply = React.useCallback(() => {
    onChange([pendingRange[0].startDate ?? null, pendingRange[0].endDate ?? null]);
    setOpen(false);
  }, [onChange, pendingRange]);

  React.useEffect(() => {
    function handleClickOutside(event: MouseEvent) {
      if (ref.current && !ref.current.contains(event.target as Node)) {
        handleApply();
      }
    }
    if (open) {
      document.addEventListener('mousedown', handleClickOutside);
    }
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [open, handleApply]);

  const handleQuickRangeClick = (item: {
    label: string;
    range: () => { startDate: Date; endDate: Date };
  }) => {
    const proposedRange = item.range();
    const selectionWithKey: Range = { ...proposedRange, key: 'selection' };
    const finalRange = applyMaxRangeConstraint(selectionWithKey, maxRangeDays, undefined);

    setPendingRange([finalRange]);
    onChange([finalRange.startDate ?? null, finalRange.endDate ?? null]);
    setOpen(false);
  };

  const handleCalendarChange = (item: RangeKeyDict) => {
    const { startDate: newStartDate, endDate: newEndDate } = item.selection;

    let currentRangeToProcess: Range = {
      startDate: newStartDate,
      endDate: newEndDate,
      key: 'selection',
    };

    currentRangeToProcess = applyMaxRangeConstraint(
      currentRangeToProcess,
      maxRangeDays,
      pendingRange[0]
    );

    setPendingRange([currentRangeToProcess]);

    const finalSelectedStartDate = currentRangeToProcess.startDate;
    const finalSelectedEndDate = currentRangeToProcess.endDate;

    if (
      finalSelectedStartDate &&
      finalSelectedEndDate &&
      finalSelectedStartDate.getTime() === finalSelectedEndDate.getTime()
    ) {
      setCalendarMinDate(finalSelectedStartDate);
      if (maxRangeDays && maxRangeDays > 0) {
        const potentialMaxEndDate = new Date(
          finalSelectedStartDate.getTime() + (maxRangeDays - 1) * 24 * 60 * 60 * 1000
        );
        setCalendarMaxDate(
          overallMaxDate && potentialMaxEndDate > overallMaxDate
            ? overallMaxDate
            : potentialMaxEndDate
        );
      } else {
        setCalendarMaxDate(overallMaxDate);
      }
    } else if (finalSelectedStartDate && finalSelectedEndDate) {
      setCalendarMinDate(overallMinDate);
      setCalendarMaxDate(overallMaxDate);
    } else {
      setCalendarMinDate(overallMinDate);
      setCalendarMaxDate(overallMaxDate);
    }
  };

  const selectedStart = pendingRange[0].startDate;
  const selectedEnd = pendingRange[0].endDate;
  const days = getDaysCount(selectedStart ?? null, selectedEnd ?? null);

  return (
    <div className={`relative text-[13px] custom-datarangepicker ${className || ''}`} ref={ref}>
      <input
        readOnly
        className="control-field"
        value={formatRange(value) || 'Select date range'}
        onClick={() => setOpen((v) => !v)}
      />
      <span className="pointer-events-none absolute right-[2px] top-[3px]">
        <CalendarIcon className="w-[22px] h-[22px] fill-[var(--datarangepicker-color-icon)]" />
      </span>
      {open && (
        <div className="absolute z-50 bg-[var(--datarangepicker-background)] dropdown-custom-datarangepicker border border-[var(--datarangepicker-border)] left-0 mt-[7px]">
          <div className="flex">
            <div className="flex flex-col w-[158px] py-[10px] border-r border-[var(--datarangepicker-border)]">
              {quickRanges.map((item) => (
                <button
                  key={item.label}
                  className="font-semibold transition cursor-pointer block w-full text-left px-[10px] py-[5px] leading-[20px]  text-[var(--datarangepicker-color)] hover:bg-[var(--datarangepicker-cell-background-active)] hover:text-[var(--datarangepicker-color-hover)]"
                  onClick={() => handleQuickRangeClick(item)}
                  type="button"
                >
                  {item.label}
                </button>
              ))}
            </div>
            <DateRange
              editableDateInputs={true}
              onChange={handleCalendarChange}
              moveRangeOnFirstSelection={false}
              ranges={pendingRange}
              rangeColors={['#163f78']}
              showDateDisplay={false}
              months={1}
              direction="vertical"
              locale={enUS}
              minDate={calendarMinDate}
              maxDate={calendarMaxDate}
            />
          </div>
          <div className="border-t border-[var(--datarangepicker-border)] p-[10px] flex justify-between items-center">
            <div className="">
              <span className="">Selected:</span>{' '}
              <span className="text-[var(--datarangepicker-color-selected-days)] bg-[var(--datarangepicker-cell-background)] px-[4px] py-[3px] rounded-[4px]">
                {selectedStart && selectedEnd ? (
                  <>
                    <span className="font-semibold">
                      {formatDateWithLeadingZeros(selectedStart)}
                    </span>{' '}
                    -{' '}
                    <span className="font-semibold">{formatDateWithLeadingZeros(selectedEnd)}</span>
                    <span className="italic"> ({days} days)</span>
                  </>
                ) : (
                  'No range selected'
                )}
              </span>
            </div>
            <ButtonFullComponent text={t('apply')} onClick={handleApply} />
          </div>
        </div>
      )}
    </div>
  );
};

export default DateRangePicker;
