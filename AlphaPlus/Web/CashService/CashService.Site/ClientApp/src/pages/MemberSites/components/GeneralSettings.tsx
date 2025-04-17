import React, { useState } from 'react';


const GeneralSettings: React.FC = () => {
  const [allowRegistration, setAllowRegistration] = useState(false);
  const [allowDeposit, setAllowDeposit] = useState(false);

  return (
    <div className="settings-container">
      <h2 className="member-sites-title">General Settings</h2>
      <div className="space-y-4">
        <div className="flex items-center justify-between">
          <span className="text-gray-700">Registration</span>
          <div className="toggle-switch" data-active={allowRegistration} onClick={() => setAllowRegistration(!allowRegistration)} />
        </div>
        <div className="flex items-center justify-between">
          <span className="text-gray-700">Deposit</span>
          <div className="toggle-switch" data-active={allowDeposit} onClick={() => setAllowDeposit(!allowDeposit)} />
        </div>
      </div>
    </div>
  );
};

export default GeneralSettings; 