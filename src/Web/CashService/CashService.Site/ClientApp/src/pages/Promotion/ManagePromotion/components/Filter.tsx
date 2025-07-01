import React, { useState } from 'react';
import CustomDropdown from '../../../../components/CustomDropdownComponent';
import { LabelTitleComponent } from '../../../../components/Title';

interface FilterProps {
  onFilterChange: (type: number, status: number) => void;
}

const Filter: React.FC<FilterProps> = ({ onFilterChange }) => {
  const [selectedType, setSelectedType] = useState<number>(0);
  const [selectedStatus, setSelectedStatus] = useState<number>(0);

  const promotionTypes = [
    { value: 0, label: 'Welcome Bonus' },
    { value: 1, label: 'Reload Bonus' },
    { value: 2, label: 'Cashback' },
    { value: 3, label: 'Tournament' },
  ];

  const statusOptions = [
    { value: 0, label: 'Active' },
    { value: 1, label: 'Scheduled' },
    { value: 2, label: 'Expired' },
  ];

  const handleTypeChange = (option: any) => {
    setSelectedType(option.value);
    onFilterChange(option.value, selectedStatus);
  };

  const handleStatusChange = (option: any) => {
    setSelectedStatus(option.value);
    onFilterChange(selectedType, option.value);
  };

  return (
    <div className="flex flex-nowrap gap-[16px] max-[768px]:justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <div className="form-group">
          <LabelTitleComponent text="Promotion Type" />
          <CustomDropdown
            options={promotionTypes}
            onSelect={handleTypeChange}
            initialValue={selectedType}
            showOptionAll={true}
          />
        </div>
      </div>

      <div className="w-[230px] pt-[4px] max-[768px]:w-[48%] mb-[8px]">
        <div className="form-group">
          <LabelTitleComponent text="Status" />
          <CustomDropdown
            options={statusOptions}
            onSelect={handleStatusChange}
            initialValue={selectedStatus}
            showOptionAll={true}
          />
        </div>
      </div>
    </div>
  );
};

export default Filter;
