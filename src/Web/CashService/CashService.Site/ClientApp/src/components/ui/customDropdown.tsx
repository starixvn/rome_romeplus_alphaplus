import { useState, useRef, useEffect } from "react";
import { IconArrowDown } from "../Icons";

export interface DropdownOption {
  value: string | number;
  label: string;
}

export interface CustomDropdownProps {
  options: DropdownOption[];
  placeholder?: string;
  onSelect: (option: DropdownOption) => void;
  disable?: boolean;
  initialValue?: string | number;
  showOptionAll?: boolean;
}

const CustomDropdown = ({ options, placeholder = "All", onSelect, disable = false, initialValue, showOptionAll = true }: CustomDropdownProps) => {
  const [isOpen, setIsOpen] = useState(false);
  const [selected, setSelected] = useState<DropdownOption | null>(() => {
    if (initialValue) {
      const option = options.find(opt => opt.value === initialValue);
      return option || null;
    }
    return null;
  });
  const dropdownRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (initialValue) {
      const option = options.find(opt => opt.value === initialValue);
      setSelected(option || null);
    }
  }, [initialValue, options]);

  const allOptions = showOptionAll ? [
    { value: "0", label: "All" },
    ...options
  ] : options;

  const handleToggle = () => {
    if (!disable) {
      setIsOpen(!isOpen);
    }
  };

  const handleSelect = (option: DropdownOption) => {
    if (!disable) {
      setSelected(option);
      setIsOpen(false);
      if (onSelect) onSelect(option);
    }
  };

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => document.removeEventListener("mousedown", handleClickOutside);
  }, []);
 
  return (
    <div className="relative z-20" ref={dropdownRef}>
      <div className={`flex items-center h-[35px] w-[100%] border-[1px] rounded-[4px] px-[12px] text-[12px] leading-[35px] ${disable ? 'cursor-not-allowed border-[#0000003D] bg-[#f4f4f4]' : 'border-[#0000003D] bg-[#ffffff] cursor-pointer'} focus:outline-none focus:border-[#163f78] relative`} onClick={handleToggle}>


      <img src="../assets/bank/BAAC.png" className='w-[16px] h-[16px] mr-[8px] inline-block' />
        {selected ? selected.label : placeholder}
        <IconArrowDown className="w-[12px] h-[12px] absolute right-[3px] top-[10px] "/>

        
      </div>
      {isOpen && (
        <div className="max-h-[200px] overflow-y-auto absolute top-[36px] text-[12px] left-0 w-[100%] bg-[#fff] border-[1px] border-[#0000003D] rounded-[8px]">
          {allOptions.map((option) => (
            <div
              key={option.value}
              onClick={() => handleSelect(option)}
              className="py-[3px] px-[12px] hover:bg-[#1967D2] hover:text-[#fff] cursor-pointer"
            >
              <img src="../assets/bank/BAAC.png" alt={option.label} className='w-[16px] h-[16px] mr-[8px] inline-block' />
              {option.label}
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default CustomDropdown;
