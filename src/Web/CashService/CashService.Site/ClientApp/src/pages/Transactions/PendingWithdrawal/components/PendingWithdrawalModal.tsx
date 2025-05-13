import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { WithdrawalAccountInfo } from '../../../../services/interfaces/WithdrawalAccountInfo';

interface PendingWithdrawalModalProps {
  withdrawal: WithdrawalAccountInfo;
  onClose: () => void;
  onApprove: (withdrawal: WithdrawalAccountInfo) => void;
  onReject: (withdrawal: WithdrawalAccountInfo) => void;
}

const PendingWithdrawalModal: React.FC<PendingWithdrawalModalProps> = ({ 
  withdrawal, 
  onClose, 
  onApprove, 
  onReject 
}) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [remarks, setRemarks] = useState('');
  const [withdrawalAmount, setWithdrawalAmount] = useState(withdrawal.amount.toString());
  const [image, setImage] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  const handleApprove = () => {
    onApprove({
      ...withdrawal,
      amount: parseFloat(withdrawalAmount),
      image: image || undefined
    });
  };

  const handleReject = () => {
    onReject({
      ...withdrawal,
      amount: parseFloat(withdrawalAmount)
    });
  };
  
  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      const selectedFile = e.target.files[0];
      setImage(selectedFile);
      
      const objectUrl = URL.createObjectURL(selectedFile);
      setImageUrl(objectUrl);
    }
  };

  const handleChangeForm = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const { value } = e.target;
    setWithdrawalAmount(value.toString());
  };

  useEffect(() => {
    return () => {
      if (imageUrl) {
        URL.revokeObjectURL(imageUrl);
      }
    };
  }, [imageUrl]);

  const resetImageUpload = () => {
    if (imageUrl) {
      URL.revokeObjectURL(imageUrl);
    }
    setImage(null);
    setImageUrl(null);
    
    const fileInput = document.getElementById('image-upload') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  };

  return (
    <div className="bg-[#00000066] fixed inset-0 flex items-center justify-center z-50">
      <div className="p-[20px] bg-white w-[850px] rounded-[4px] shadow-[0_2px_4px_-1px_#00000033,0_4px_5px_#00000024,0_1px_10px_#0000001f]">
        <div className="relative">
          <h5 className="text-[16px] text-[#163f78] font-bold pb-[15px]">
            {t('pendingWithdrawalModal.title')} ({withdrawal.referenceNo})
          </h5>
          <button type="button" className="absolute top-0 right-0 cursor-pointer" onClick={onClose}>
            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
            </svg>
          </button>
        </div>

        <div className="flex gap-6">
          {/* Main content */}
          <div className="w-[460px] h-full">
            <div className="border border-[#214184] rounded mb-6">
              <table className="w-full h-full">
                <thead>
                  <tr>
                    <th className="text-sm text-left p-3 text-gray-700">{t('pendingWithdrawalModal.username')}</th>
                    <th className="text-sm text-left p-3 text-gray-700">{t('pendingWithdrawalModal.loginName')}</th>
                    <th className="text-sm text-left p-3 text-gray-700">{t('pendingWithdrawalModal.name')}</th>
                    <th className="text-sm text-left p-3 text-gray-700">{t('pendingWithdrawalModal.betCredit')}</th>
                  </tr>
                </thead>
                <tbody>
                  <tr>
                    <td className="text-sm p-3">{withdrawal.username}</td>
                    <td className="text-sm p-3">{withdrawal.loginName}</td>
                    <td className="text-sm p-3">{`${withdrawal.firstName} ${withdrawal.lastName}`}</td>
                    <td className="text-sm p-3">{withdrawal.amount.toFixed(2)}</td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="grid grid-cols-2 gap-6">
              <div>
                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.to')}</div>
                  <div className="flex items-center">
                    <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-blue-100 text-blue-600">
                      <span>🔄</span>
                    </div>
                    <span>{withdrawal.to}</span>
                  </div>
                </div>

                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.withdrawalAmount')}</div>
                  <div className="mt-1">
                    <span>{withdrawalAmount} TB</span>
                  </div>
                </div>

                <div>
                  <div className="text-sm font-medium mb-1 relative">
                    {t('pendingWithdrawalModal.from')}
                    <span className="text-red-500 ml-1">*</span>
                  </div>
                  <div className="flex items-center">
                    <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-green-100 text-green-600">
                      <span>🔄</span>
                    </div>
                    <select 
                      className="border border-gray-300 rounded py-1 px-2 w-full h-[32px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
                      value={withdrawal.from}
                      onChange={handleChangeForm}
                    >
                      <option value={withdrawal.from}>{withdrawal.from}</option>
                    </select>
                  </div>
                </div>
              </div>

              <div>
                <div className="mb-4">
                  <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.dateAndTime')}</div>
                  <div>
                    {typeof withdrawal.dateAndTime === 'string' 
                      ? withdrawal.dateAndTime 
                      : withdrawal.dateAndTime.toLocaleString()}
                  </div>
                </div>
              </div>
            </div>

            <div className="mt-6 h-full">
              <div className="text-sm font-medium mb-1">{t('pendingWithdrawalModal.remarks')}</div>
              <textarea 
                value={remarks}
                onChange={(e) => setRemarks(e.target.value)}
                placeholder={t('pendingWithdrawalModal.remarksPlaceholder')}
                className="border border-gray-300 rounded-md p-2 w-full h-[120px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] text-gray-700"
              />
            </div>
          </div>
          
          {/* Right side - Upload image */}
          <div className="w-[325px]">
            <div className="w-full h-full border-[2px] border-dashed border-[#163f78] rounded-[5px] flex flex-col items-center justify-center relative">
              {imageUrl ? (
                <div className="absolute inset-0 flex flex-col items-center">
                  <img 
                    src={imageUrl} 
                    alt="Preview"
                    className="w-full h-full object-contain"
                  />
                  <button
                    type="button"
                    onClick={resetImageUpload}
                    className="absolute top-2 right-2 bg-white rounded-full p-1 shadow hover:bg-gray-200 z-20 cursor-pointer"
                    title={t('manualWithdrawalForm.removeImage')}
                  >
                    <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5 text-gray-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                      <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                    </svg>
                  </button>
                </div>
              ) : (
                <div className="flex flex-col items-center justify-center text-center p-6">
                  <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8 text-[#163f78] mb-4" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                  </svg>
                  <p className="text-sm font-medium mb-1">
                    <span className="text-[#163f78] cursor-pointer underline">{t('manualWithdrawalForm.uploadImage')}</span> <span className="text-[#333]">{t('manualWithdrawalForm.dragAndDrop')}</span>
                  </p>
                  <span className="text-xs font-medium text-[#333]">{t('manualWithdrawalForm.imageFormat')}</span>
                  <span className="text-xs font-medium text-[#333]">{t('manualWithdrawalForm.maxSize')}</span>
                </div>
              )}
              <input
                id="image-upload"
                type="file"
                accept=".jpeg,.jpg,.png"
                onChange={handleImageUpload}
                className="hidden"
              />
              <label
                htmlFor="image-upload"
                className={`absolute inset-0 w-full h-full ${imageUrl ? 'opacity-0 z-0' : 'opacity-0 cursor-pointer z-10'}`}
              />
            </div>
          </div>
        </div>
        
        <div className="flex justify-end space-x-3 mt-6">
          <button
            onClick={handleReject}
            className="w-[140px] capitalize font-bold h-[32px] rounded-[20px] px-[16px] text-[#163f78] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#ffffff] hover:bg-[#dee2e7] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
          >
            {t('pendingWithdrawalModal.reject')}
          </button>
          <button
            onClick={handleApprove}
            className="w-[140px] capitalize font-bold h-[32px] rounded-[20px] text-white px-[16px] py-[4px] text-[12px] border-[1px] border-[#163f78] mx-[4px] bg-[#163f78] hover:bg-[var(--button-background-hover)] cursor-pointer shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f]"
          >
            {t('pendingWithdrawalModal.approve')}
          </button>
        </div>
      </div>
    </div>
  );
};

export default PendingWithdrawalModal;