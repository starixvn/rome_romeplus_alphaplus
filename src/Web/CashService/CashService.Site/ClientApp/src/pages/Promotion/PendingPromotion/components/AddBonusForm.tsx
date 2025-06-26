import React, { useState } from 'react';
import {
  BackButtonComponent,
  ButtonApplyComponent,
  ButtonCancelComponent,
} from '../../../../components/Buttons';
import { TitleComponent, LabelTitleComponent, MediumTitleComponent } from '../../../../components/Title';
import { AmountInput } from '../../../../components/AmountInput';
import DropdownComponent, { DropdownOption } from '../../../../components/CustomDropdownComponent';
import { ResultMessageComponent } from '../../../../components/Message';

const mockMembers = [
  { value: '999tb20a00001', label: '999tb20a00001' },
  { value: '999tb20a00002', label: '999tb20a00002' },
];

interface AddBonusFormProps {
  onClose?: () => void;
}

const AddBonusForm: React.FC<AddBonusFormProps> = ({ onClose }) => {
  const [member, setMember] = useState(mockMembers[0].value);
  const [amount, setAmount] = useState<string>('');
  const [remark, setRemark] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [resultMessage, setResultMessage] = useState<{
    type: 'success' | 'error' | null;
    message: string;
  }>({ type: null, message: '' });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!member || !amount || isNaN(Number(amount.replace(/,/g, '')))) return;
    setIsSubmitting(true);
    setTimeout(() => {
      setResultMessage({ type: 'success', message: 'Bonus added successfully!' });
      setIsSubmitting(false);
      if (onClose) setTimeout(onClose, 1500);
    }, 1000);
  };

  const handleMemberChange = (option: DropdownOption) => {
    setMember(option.value as string);
  };

  return (
    <div className="">
      <div className="flex items-center justify-flex-start mb-[8px] py-[8px] border-[var(--border-control)] border-b-[1px] [&>:nth-child(2)]:border-0 [&>:nth-child(2)]:mb-[0] [&>:nth-child(1)]:mb-[0]">
        <BackButtonComponent onClick={onClose} />
        <TitleComponent title="Add Bonus" />
      </div>
      <div
        id="manual-withdrawal-form"
        className="max-w-[1100px] w-full max-[768px]:w-full pb-[16px] pt-[10px]"
      >
        {resultMessage.type && (
          <ResultMessageComponent
            type={resultMessage.type}
            message={resultMessage.message}
            className="mb-[16px] max-[768px]:mb-[0]"
          />
        )}
        <div className="flex justify-between max-[768px]:flex-col mb-[16px]">
          <div className="flex-1 max-w-[700px] max-[768px]:mb-[16px] mr-[30px] max-[768px]:mr-[0]">
            <MediumTitleComponent text={'Account Information'} />
            <div className="flex flex-wrap">
              <div className="flex flex-nowrap gap-[16px] mb-[4px] justify-between max-[768px]:gap-[0] max-[768px]:flex-wrap w-full">
                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={'Member Login Name'} required />

                  <div className="relative z-[9999]">
                    <DropdownComponent
                      options={mockMembers}
                      onSelect={handleMemberChange}
                      initialValue={member}
                      searchable={true}
                      placeholder={'Select Member'}
                    />
                  </div>
                </div>
                <div className="min-w-[230px] pt-[4px] w-[48%] mb-[8px]">
                  <LabelTitleComponent text={'Bonus Amount'} required />
                  <AmountInput
                    id="bonusAmount"
                    value={Number(amount)}
                    onChange={(value) => setAmount(value.toString())}
                    className="control-field"
                  />
                </div>
              </div>
              <div className="w-full">
                <LabelTitleComponent text={'Remarks'} />
                <textarea
                  id="remarks"
                  value={remark}
                  onChange={(e) => setRemark(e.target.value)}
                  maxLength={1000}
                  className="block control-field h-[200px]! py-[8px]! max-[768px]:h-[150px]!"
                  placeholder={'Remarks'}
                />
              </div>
            </div>
          </div>
        </div>
        <div className="flex gap-[16px]">
          <ButtonCancelComponent onClick={onClose} disabled={isSubmitting} text={`Cancel`} />
          <ButtonApplyComponent disabled={isSubmitting} text={`Approve`} onClick={handleSubmit} />
        </div>
      </div>
    </div>
  );
};

export default AddBonusForm;
