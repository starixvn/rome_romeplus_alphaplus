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
    { value: 0, label: 'All' },
    { value: 1, label: 'Active' },
    { value: 2, label: 'Scheduled' },
    { value: 3, label: 'Expired' },
    { value: 4, label: 'Completed' },
    { value: 5, label: 'Inactive' },
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
    <div className="flex flex-wrap w-full">
      <div className="w-[190px] mr-[12px] mb-[12px]">
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

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <div className="form-group">
          <LabelTitleComponent text="Status" />
          <CustomDropdown
            options={statusOptions}
            onSelect={handleStatusChange}
            initialValue={selectedStatus}
          />
        </div>
      </div>
    </div>
  );
};

export default Filter;
