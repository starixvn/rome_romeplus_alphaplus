import React, {} from 'react';


interface AddBankAccountComponentProps {
  onClose: () => void;
  onSave: () => void;
}

const AddBankAccountComponent: React.FC<AddBankAccountComponentProps> = ({
}) => {
  return (
    <div>
      <h1>Add Bank Account</h1>
    </div>
  );
};

export default AddBankAccountComponent; 