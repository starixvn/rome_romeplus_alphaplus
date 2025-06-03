import 'react-date-range/dist/styles.css';
import 'react-date-range/dist/theme/default.css';

import { enUS } from 'date-fns/locale';
import React from 'react';
import { DateRange, Range, RangeKeyDict } from 'react-date-range';

import { ButtonFullComponent } from './ButtonComponents';
import { CalendarIcon } from './Icons';
import './DateRangePickerComponent.scss';
import { convertToGMT4 } from '../common/commonFunctions';

export interface DateRangePickerProps {
  value: [Date | null, Date | null];
  onChange: (value: [Date | null, Date | null]) => void;
  className?: string;
  minYear?: number;
  maxRangeDays?: number;
}

function formatRange(value: [Date | null, Date | null]) {
  if (!value[0] || !value[1]) return '';
  return value[0].toLocaleDateString('en-US') + ' - ' + value[1].toLocaleDateString('en-US');
}

function getDaysCount(start: Date | null, end: Date | null) {
  if (!start || !end) return 0;
  const diff = Math.abs(end.getTime() - start.getTime());
  return Math.floor(diff / (1000 * 60 * 60 * 24)) + 1;
}

// Helper function to apply max range constraint
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

      // Determine which end was likely moved or if it's a new range (like a quick select)
      if (previousSelection && prevStartDate && prevEndDate) {
        const startDateChanged = startDate.getTime() !== prevStartDate.getTime();
        const endDateChanged = endDate.getTime() !== prevEndDate.getTime();

        if (startDateChanged && !endDateChanged) {
          // Start date changed, anchor end date
          startDate = new Date(endDate.getTime() - (maxDays - 1) * 24 * 60 * 60 * 1000);
        } else if (!startDateChanged && endDateChanged) {
          // End date changed, anchor start date
          endDate = new Date(startDate.getTime() + (maxDays - 1) * 24 * 60 * 60 * 1000);
        } else {
          // Both changed or couldn't determine anchor, default to adjusting end date based on start date
          endDate = new Date(startDate.getTime() + (maxDays - 1) * 24 * 60 * 60 * 1000);
        }
      } else {
        // No previous selection to infer from (e.g. quick range or first selection), adjust end date based on start date
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
  const [open, setOpen] = React.useState(false);
  const ref = React.useRef<HTMLDivElement>(null);

  const minDate = minYear ? new Date(minYear, 0, 1) : undefined;

  const today = new Date();
  const tomorrow = new Date(convertToGMT4(today));
  tomorrow.setDate(today.getDate() + 1);
  tomorrow.setHours(23, 59, 59, 999);

  let maxDate: Date = tomorrow;

  const [pendingRange, setPendingRange] = React.useState<Range[]>([
    {
      startDate: value[0] ?? new Date(),
      endDate: value[1] ?? new Date(),
      key: 'selection',
    },
  ]);

  React.useEffect(() => {
    setPendingRange([
      {
        startDate: value[0] ?? new Date(),
        endDate: value[1] ?? new Date(),
        key: 'selection',
      },
    ]);
  }, [value[0]?.getTime?.(), value[1]?.getTime?.()]);

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
  }, [open, onChange, pendingRange]);

  const handleApply = () => {
    onChange([pendingRange[0].startDate ?? null, pendingRange[0].endDate ?? null]);
    setOpen(false);
  };

  const handleQuickRangeClick = (item: {
    label: string;
    range: () => { startDate: Date; endDate: Date };
  }) => {
    const proposedRange = item.range();

    const selectionWithKey: Range = { ...proposedRange, key: 'selection' };
    const finalRange = applyMaxRangeConstraint(selectionWithKey, maxRangeDays);
    setPendingRange([finalRange]);
    onChange([finalRange.startDate ?? null, finalRange.endDate ?? null]);
    setOpen(false);
  };

  const selectedStart = pendingRange[0].startDate;
  const selectedEnd = pendingRange[0].endDate;
  const days = getDaysCount(selectedStart ?? null, selectedEnd ?? null);

  return (
    <div className={`relative text-[13px] custom-datarangepicker ${className || ''}`} ref={ref}>
      <input
        readOnly
        className="flex items-center h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[12px] border-[var(--border-control)]  bg-[var(--background-control)] cursor-pointer focus:outline-none focus:border-[var(--border-control-focus)] relative"
        value={formatRange(value) || 'Select date range'}
        onClick={() => setOpen((v) => !v)}
      />
      <span className="pointer-events-none absolute inset-y-0 right-2 flex items-center text-gray-400">
        <CalendarIcon className="w-5 h-5" />
      </span>
      {open && (
        <div className="absolute z-50 bg-white dropdown-custom-datarangepicker border border-[var(--datarangepicker-border)] left-0 mt-[7px]">
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
              onChange={(item: RangeKeyDict) => {
                const newSelection = applyMaxRangeConstraint(
                  item.selection,
                  maxRangeDays,
                  pendingRange[0]
                );
                setPendingRange([newSelection]);
              }}
              moveRangeOnFirstSelection={false}
              ranges={pendingRange}
              rangeColors={['#163f78']}
              showDateDisplay={false}
              months={1}
              direction="vertical"
              locale={enUS}
              minDate={minDate}
              maxDate={maxDate}
            />
          </div>
          <div className="border-t border-[var(--datarangepicker-border)] p-[10px] flex justify-between items-center">
            <div className="">
              <span className="">Selected:</span>{' '}
              <span className="text-[var(--datarangepicker-color-selected-days)] bg-[var(--datarangepicker-cell-background)] px-[4px] py-[3px] rounded-[4px]">
                {selectedStart && selectedEnd ? (
                  <>
                    <span className="font-semibold">
                      {selectedStart.toLocaleDateString('en-US')}
                    </span>{' '}
                    -{' '}
                    <span className="font-semibold">{selectedEnd.toLocaleDateString('en-US')}</span>
                    <span className="italic"> ({days} days)</span>
                  </>
                ) : (
                  'No range selected'
                )}
              </span>
            </div>
            <ButtonFullComponent text="Apply" onClick={handleApply} />
          </div>
        </div>
      )}
    </div>
  );
};

export default DateRangePicker;
