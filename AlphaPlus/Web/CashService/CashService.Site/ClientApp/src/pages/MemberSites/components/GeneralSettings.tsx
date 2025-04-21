import React, { useState, useEffect } from 'react';

interface GeneralSettingsProps {
  allowRegistration: boolean;
  allowDeposit: boolean;
  onSettingsChange: (settings: { allowRegistration: boolean; allowDeposit: boolean }) => void;
}

const GeneralSettings: React.FC<GeneralSettingsProps> = ({ 
  allowRegistration: initialAllowRegistration, 
  allowDeposit: initialAllowDeposit,
  onSettingsChange 
}) => {
  const [allowRegistration, setAllowRegistration] = useState(initialAllowRegistration);
  const [allowDeposit, setAllowDeposit] = useState(initialAllowDeposit);

  useEffect(() => {
    setAllowRegistration(initialAllowRegistration);
    setAllowDeposit(initialAllowDeposit);
  }, [initialAllowRegistration, initialAllowDeposit]);

  const handleRegistrationChange = (value: boolean) => {
    setAllowRegistration(value);
    onSettingsChange({ allowRegistration: value, allowDeposit });
  };

  const handleDepositChange = (value: boolean) => {
    setAllowDeposit(value);
    onSettingsChange({ allowRegistration, allowDeposit: value });
  };

  return (
    <div className="space-y-4">
      <h2 className="text-lg font-medium mb-4">General Settings</h2>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="flex items-center justify-between">
          <div>
            <h3 className="font-medium">Registration</h3>
            <p className="text-sm text-gray-600">Allow member to register</p>
          </div>
          <label className="relative inline-flex items-center cursor-pointer">
            <input
              type="checkbox"
              checked={allowRegistration}
              onChange={(e) => handleRegistrationChange(e.target.checked)}
              className="sr-only peer"
            />
            <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#1e4d8c]"></div>
          </label>
        </div>
        <div className="flex items-center justify-between">
          <div>
            <h3 className="font-medium">Deposit</h3>
            <p className="text-sm text-gray-600">Allow member to deposit</p>
          </div>
          <label className="relative inline-flex items-center cursor-pointer">
            <input
              type="checkbox"
              checked={allowDeposit}
              onChange={(e) => handleDepositChange(e.target.checked)}
              className="sr-only peer"
            />
            <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-[#1e4d8c]"></div>
          </label>
        </div>
      </div>
    </div>
  );
};

export default GeneralSettings; 