import { useState, useRef, useEffect } from 'react';
import { IconArrowDown } from './Icons';

export interface DropdownOption {
  value: string | number;
  label: string;
  code?: string;
  attributes?: Record<string, any>;
}

export interface CustomDropdownComponentProps {
  options: DropdownOption[];
  onSelect: (option: DropdownOption) => void;
  disable?: boolean;
  initialValue?: string | number;
  showOptionAll?: boolean;
  searchable?: boolean;
}

const CustomDropdownComponent = ({
  options,
  onSelect,
  disable = false,
  initialValue,
  showOptionAll = false,
  searchable = false,
}: CustomDropdownComponentProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [selected, setSelected] = useState<DropdownOption | ''>('');
  const [searchTerm, setSearchTerm] = useState('');
  const dropdownRef = useRef<HTMLDivElement>(null);
  const searchInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (initialValue) {
      const option = options.find((opt) => opt.value == initialValue);
      setSelected(option || '');
    }
  }, [initialValue, options]);

  useEffect(() => {
    if (isOpen && searchable && searchInputRef.current) {
      searchInputRef.current.focus();
    }
  }, [isOpen, searchable]);

  const handleToggle = () => {
    if (!disable) {
      setIsOpen(!isOpen);
      if (!isOpen) {
        setSearchTerm('');
      }
    }
  };

  const handleSelect = (option: DropdownOption) => {
    if (!disable) {
      setSelected(option);
      setIsOpen(false);
      setSearchTerm('');
      if (onSelect) onSelect(option);
    }
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
        setSearchTerm('');
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const displayOptions = showOptionAll
    ? [{ value: '', label: 'All', code: 'all' }, ...options]
    : options;

  const filteredOptions = searchTerm
    ? displayOptions.filter((option) =>
        option.label.toLowerCase().includes(searchTerm.toLowerCase())
      )
    : displayOptions;

  return (
    <div className="relative z-20" ref={dropdownRef}>
      <div
        className={`flex items-center h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[12px] border-[var(--border-control)] ${disable ? 'cursor-not-allowed bg-[#f4f4f4]' : ' bg-[var(--background-control)] cursor-pointer'} focus:outline-none focus:border-[var(--border-control-focus)] relative`}
        onClick={handleToggle}
      >
        {selected && selected.code && selected.value !== '' && (
          <img
            src={`../assets/bank/${selected.code}.png`}
            className="w-[16px] h-[16px] mr-[8px] inline-block"
          />
        )}
        <span className="block whitespace-nowrap overflow-hidden text-ellipsis pr-[12px]">
          {selected ? selected.label : 'All'}
        </span>
        <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[10px] max-[768px]:top-[12px] " />
      </div>

      {isOpen && (
        <div className="max-h-[200px] overflow-y-auto absolute top-[33px] left-0 w-[100%] bg-[#fff] border-[1px] border-[#0000003D] rounded-[8px]">
          {searchable && (
            <div className="sticky top-0 bg-white p-2 border-b border-gray-200">
              <input
                ref={searchInputRef}
                type="text"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                placeholder="Search..."
                className="w-full px-2 py-1 border border-gray-300 rounded focus:outline-none focus:border-[var(--border-control-focus)]"
                onClick={(e) => e.stopPropagation()}
              />
            </div>
          )}
          {filteredOptions.length > 0 ? (
            filteredOptions.map((option) => (
              <div
                key={option.value}
                onClick={() => handleSelect(option)}
                className="py-[3px] px-[12px] hover:bg-[#1967D2] hover:text-[#fff] cursor-pointer flex items-center max-[768px]:py-[6px]"
              >
                {option.code && option.value !== '' && (
                  <img
                    src={`../assets/bank/${option.code}.png`}
                    alt={option.label}
                    className="w-[16px] h-[16px] mr-[8px] inline-block"
                  />
                )}
                {option.label}
              </div>
            ))
          ) : (
            <div className="py-[3px] px-[12px] text-gray-500">No options found</div>
          )}
        </div>
      )}
    </div>
  );
};

export default CustomDropdownComponent;
