import { useState, useRef, useEffect } from 'react';
import { IconArrowDown } from './Icons';
import { useTranslation } from 'react-i18next';

export interface DropdownOption {
  value: string | number;
  label: string;
  icon?: any;
  attributes?: Record<string, any>;
}

export interface CustomDropdownComponentProps {
  options: DropdownOption[];
  onSelect: (option: DropdownOption) => void;
  disable?: boolean;
  initialValue?: string | number;
  showOptionAll?: boolean;
  searchable?: boolean;
  placeholder?: string;
}

const CustomDropdownComponent = ({
  options,
  onSelect,
  disable = false,
  initialValue,
  showOptionAll = false,
  searchable = false,
  placeholder = '',
}: CustomDropdownComponentProps) => {
  const { t } = useTranslation('global');

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
    if (disable) {
      return;
    }

    setIsOpen(!isOpen);
    if (!isOpen) {
      setSearchTerm('');
    }
  };

  const handleSelect = (option: DropdownOption) => {
    if (disable) {
      return;
    }

    setSelected(option);
    setIsOpen(false);
    setSearchTerm('');
    if (onSelect) onSelect(option);
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

  const displayOptions = showOptionAll ? [{ value: '', label: t('all') }, ...options] : options;

  const filteredOptions = searchTerm
    ? displayOptions.filter((option) =>
        option.label.toLowerCase().includes(searchTerm.toLowerCase())
      )
    : displayOptions;

  const displaySelected = () => {
    if (selected) return selected.label;
    if (showOptionAll) return 'All';
    return placeholder;
  };
  return (
    <div className="relative" ref={dropdownRef}>
      <div
        className={`flex items-center h-[32px] max-[768px]:h-[40px] w-[100%] border-[1px] rounded-[4px] px-[12px] border-[var(--border-control)] ${disable ? 'cursor-not-allowed bg-[#f4f4f4]' : ' bg-[var(--background-control)] cursor-pointer'} focus:outline-none focus:border-[var(--border-control-focus)] relative`}
        onClick={handleToggle}
      >
        {selected && selected.icon}
        <span className="block whitespace-nowrap overflow-hidden text-ellipsis pr-[12px]">
          {displaySelected()}
        </span>
        <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[10px] max-[768px]:top-[12px] " />
      </div>

      {isOpen && (
        <div className="max-h-[200px] overflow-y-auto absolute top-[33px] left-0 w-[100%] bg-[#fff] border-[1px] border-[#0000003D] rounded-[8px] z-[9999]">
          {searchable && (
            <div className="sticky top-0 bg-white p-2 border-b border-gray-200">
              <input
                ref={searchInputRef}
                type="text"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                placeholder={t('search')}
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
                {option.icon}
                {option.label}
              </div>
            ))
          ) : (
            <div className="py-[3px] px-[12px] text-gray-500">{t('noData')}</div>
          )}
        </div>
      )}
    </div>
  );
};

export default CustomDropdownComponent;
