import React, { useState } from 'react';
import CustomDropdown from '../../../../components/CustomDropdownComponent';
import { TitleComponent } from '../../../../components/Title';

interface FilterProps {
  onFilterChange: (type: number, status: number) => void;
}

const Filter: React.FC<FilterProps> = ({ onFilterChange }) => {
  const [selectedType, setSelectedType] = useState<number>(0);
  const [selectedStatus, setSelectedStatus] = useState<number>(0);

  const promotionTypes = [
    { value: 0, label: 'All' },
    { value: 1, label: 'Turnover Rebate' }
  ];

  const statusOptions = [
    { value: 0, label: 'All' },
    { value: 1, label: 'Enable' },
    { value: 2, label: 'Disable' }
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
          <TitleComponent text="Promotion Type" type='label'/>
          <CustomDropdown
            options={promotionTypes}
            onSelect={handleTypeChange}
            initialValue={selectedType}
          />
        </div>
      </div>

      <div className="w-[190px] mr-[12px] mb-[12px]">
        <div className="form-group">
          <TitleComponent text="Status" type='label'/>
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
