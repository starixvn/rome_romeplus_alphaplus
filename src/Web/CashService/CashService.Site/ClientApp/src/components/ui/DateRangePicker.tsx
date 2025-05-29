import 'react-date-range/dist/styles.css';
import 'react-date-range/dist/theme/default.css';

import { enUS } from 'date-fns/locale';
import React from 'react';
import { DateRange, Range, RangeKeyDict } from 'react-date-range';

import { ButtonFullComponent } from '../ButtonComponents';
import { CalendarIcon } from '../Icons';

export interface DateRangePickerProps {
  value: [Date | null, Date | null];
  onChange: (value: [Date | null, Date | null]) => void;
  className?: string;
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

const quickRanges = [
  {
    label: 'Today',
    range: () => {
      const today = new Date();
      return { startDate: today, endDate: today };
    },
  },
  {
    label: 'Yesterday',
    range: () => {
      const yesterday = new Date();
      yesterday.setDate(yesterday.getDate() - 1);
      return { startDate: yesterday, endDate: yesterday };
    },
  },
  {
    label: 'Last 7 Days',
    range: () => {
      const end = new Date();
      const start = new Date();
      start.setDate(end.getDate() - 6);
      return { startDate: start, endDate: end };
    },
  },
  {
    label: 'Last 30 Days',
    range: () => {
      const end = new Date();
      const start = new Date();
      start.setDate(end.getDate() - 29);
      return { startDate: start, endDate: end };
    },
  },
  {
    label: 'This Month',
    range: () => {
      const now = new Date();
      return { startDate: new Date(now.getFullYear(), now.getMonth(), 1), endDate: now };
    },
  },
  {
    label: 'Last Month',
    range: () => {
      const now = new Date();
      const start = new Date(now.getFullYear(), now.getMonth() - 1, 1);
      const end = new Date(now.getFullYear(), now.getMonth(), 0);
      return { startDate: start, endDate: end };
    },
  },
];

const DateRangePicker: React.FC<DateRangePickerProps> = ({ value, onChange, className }) => {
  const [open, setOpen] = React.useState(false);
  const ref = React.useRef<HTMLDivElement>(null);

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
        setOpen(false);
      }
    }
    if (open) {
      document.addEventListener('mousedown', handleClickOutside);
    }
    return () => {
      document.removeEventListener('mousedown', handleClickOutside);
    };
  }, [open]);

  const handleApply = () => {
    onChange([pendingRange[0].startDate ?? null, pendingRange[0].endDate ?? null]);
    setOpen(false);
  };

  const selectedStart = pendingRange[0].startDate;
  const selectedEnd = pendingRange[0].endDate;
  const days = getDaysCount(selectedStart ?? null, selectedEnd ?? null);

  return (
    <div className={`relative text-[13px] custom-datarangepicker ${className || ''}`} ref={ref}>
      <input
        readOnly
        className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 h-[32px] max-[768px]:h-[40px] cursor-pointer"
        value={formatRange(value) || 'Select date range'}
        onClick={() => setOpen((v) => !v)}
      />
      <span className="pointer-events-none absolute inset-y-0 right-2 flex items-center text-gray-400">
        <CalendarIcon className="w-5 h-5" />
      </span>
      {open && (
        <div className="absolute z-50 bg-white  border border-[var(--datarangepicker-border)] left-0 mt-[7px]">
          <div className="flex">
            <div className="flex flex-col w-[158px] py-[10px] border-r border-[var(--datarangepicker-border)]">
              {quickRanges.map((item) => (
                <button
                  key={item.label}
                  className="font-semibold transition cursor-pointer block w-full text-left px-[10px] py-[5px] leading-[20px]  text-[var(--datarangepicker-color)] hover:bg-[var(--datarangepicker-cell-background-active)] hover:text-[var(--datarangepicker-color-hover)]"
                  onClick={() => setPendingRange([{ ...item.range(), key: 'selection' }])}
                  type="button"
                >
                  {item.label}
                </button>
              ))}
            </div>
            <DateRange
              editableDateInputs={true}
              onChange={(item: RangeKeyDict) => setPendingRange([item.selection])}
              moveRangeOnFirstSelection={false}
              ranges={pendingRange}
              rangeColors={['#163f78']}
              showDateDisplay={false}
              months={1}
              direction="vertical"
              locale={enUS}
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
