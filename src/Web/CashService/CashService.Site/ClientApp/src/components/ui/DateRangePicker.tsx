import React from 'react';
import { DateRange, Range, RangeKeyDict } from 'react-date-range';
import 'react-date-range/dist/styles.css';
import 'react-date-range/dist/theme/default.css';
import { enUS } from 'date-fns/locale';
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

  // State tạm cho popup
  const [pendingRange, setPendingRange] = React.useState<Range[]>([
    {
      startDate: value[0] ?? new Date(),
      endDate: value[1] ?? new Date(),
      key: 'selection',
    },
  ]);

  // Khi value prop thay đổi từ ngoài, sync lại pendingRange
  React.useEffect(() => {
    setPendingRange([
      {
        startDate: value[0] ?? new Date(),
        endDate: value[1] ?? new Date(),
        key: 'selection',
      },
    ]);
  }, [value[0]?.getTime?.(), value[1]?.getTime?.()]);

  // Đóng popup khi click ngoài
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

  // Khi nhấn Apply
  const handleApply = () => {
    onChange([pendingRange[0].startDate ?? null, pendingRange[0].endDate ?? null]);
    setOpen(false);
  };

  // Dữ liệu hiển thị Selected
  const selectedStart = pendingRange[0].startDate;
  const selectedEnd = pendingRange[0].endDate;
  const days = getDaysCount(selectedStart ?? null, selectedEnd ?? null);

  return (
    <div className={`relative ${className || ''}`} ref={ref}>
      <input
        readOnly
        className="appearance-none block w-full bg-white border border-gray-300 px-3 py-1.5 pr-8 rounded text-gray-900 focus:outline-none focus:ring-1 focus:ring-blue-500 focus:border-blue-500 sm:text-sm h-9 cursor-pointer"
        value={formatRange(value) || 'Select date range'}
        onClick={() => setOpen((v) => !v)}
      />
      <span className="pointer-events-none absolute inset-y-0 right-2 flex items-center text-gray-400">
        <CalendarIcon className="w-5 h-5" />
      </span>
      {open && (
        <div className="absolute z-50 mt-2 bg-white border border-gray-200 rounded shadow-lg left-0">
          <div className="flex">
            <div className="flex flex-col border-r pr-1 mr-1 min-w-[110px] pt-3">
              {quickRanges.map((item) => (
                <button
                  key={item.label}
                  className="text-left px-2 py-1 text-xs text-gray-700 hover:bg-blue-100 hover:text-blue-700 rounded transition cursor-pointer"
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
              rangeColors={['#2563eb']}
              showDateDisplay={false}
              months={1}
              direction="horizontal"
              className="border-none"
              locale={enUS}
            />
          </div>
          <div className="px-4 py-2">
            <div className="flex items-center justify-between text-xs text-gray-700 mb-2">
              <span className="font-semibold">Selected:</span>{' '}
              {selectedStart && selectedEnd
                ? `${selectedStart.toLocaleDateString('en-US')} - ${selectedEnd.toLocaleDateString('en-US')} (${days} days)`
                : 'No range selected'}
              <button
                className="ml-4 bg-blue-600 hover:bg-blue-700 text-white text-sm px-4 py-1.5 rounded transition"
                onClick={handleApply}
                disabled={!selectedStart || !selectedEnd}
              >
                Apply
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default DateRangePicker;
