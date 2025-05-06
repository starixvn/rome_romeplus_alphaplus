import React, { useState, useEffect } from 'react';
import { useTranslation } from 'react-i18next';

interface ManualAddWithdrawalFormProps {
  onClose: () => void;
  onSubmit: (data: {
    memberLoginName: string;
    from: number;
    to: number;
    withdrawalAmount: number;
    remarks: string;
    image?: File;
  }) => void;
}

const ManualAddWithdrawalForm: React.FC<ManualAddWithdrawalFormProps> = ({ onClose, onSubmit }) => {
  const { t } = useTranslation('pendingWithdrawal');
  const [memberLoginName, setMemberLoginName] = useState('999b20a00001');
  const [from, setFrom] = useState(12345678);
  const [to, setTo] = useState(1493993018);
  const [withdrawalAmount, setWithdrawalAmount] = useState('');
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState<string | null>(null);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      memberLoginName,
      from,
      to,
      withdrawalAmount: parseFloat(withdrawalAmount),
      remarks,
      image: image || undefined
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
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      <div className="bg-gray-100 p-4">
        <h2 className="text-lg font-medium">{t('manualWithdrawalForm.title')}</h2>
      </div>

      <form id='manual-withdrawal-form' onSubmit={handleSubmit} className="p-6 flex-1">
        <div className="flex flex-row space-x-10">
          {/* Left side - Account information */}
          <div className="">
            <div className="mb-5 w-[900px]">
              <h3 className="text-md font-medium text-blue-900 mb-4">{t('manualWithdrawalForm.accountInformation')}</h3>
              
              <div className="flex flex-col space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label htmlFor="memberLoginName" className="block text-sm font-medium text-gray-700 mb-1 relative">
                      {t('manualWithdrawalForm.memberLoginName')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <div className="relative">
                      <select
                        id="memberLoginName"
                        value={memberLoginName}
                        onChange={(e) => setMemberLoginName(e.target.value)}
                        className="border border-gray-300 rounded-md px-2 w-full h-[35px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                      >
                        <option value="999b20a00001">999b20a00001</option>
                      </select>
                    </div>
                  </div>

                  <div>
                    <label htmlFor="from" className="block text-sm font-medium text-gray-700 mb-1 relative">
                      {t('manualWithdrawalForm.from')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <div className="relative">
                      <select
                        id="from"
                        value={from}
                        onChange={(e) => setFrom(parseInt(e.target.value))}
                        className="border border-gray-300 rounded-md px-2 w-full h-[35px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                      >
                        <option value="12345678">12345678</option>
                      </select>
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label htmlFor="to" className="block text-sm font-medium text-gray-700 mb-1 relative">
                      {t('manualWithdrawalForm.to')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <div className="relative">
                      <select
                        id="to"
                        value={to}
                        onChange={(e) => setTo(parseInt(e.target.value))}
                        className="border border-gray-300 rounded-md px-2 w-full h-[35px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] appearance-auto"
                      >
                        <option value="1493993018">1493993018</option>
                      </select>
                    </div>
                  </div>

                  <div>
                    <label htmlFor="withdrawalAmount" className="block text-sm font-medium text-gray-700 mb-1 relative">
                      {t('manualWithdrawalForm.withdrawalAmount')}
                      <span className="text-red-500 absolute">*</span>
                    </label>
                    <input
                      id="withdrawalAmount"
                      type="number"
                      value={withdrawalAmount}
                      onChange={(e) => setWithdrawalAmount(e.target.value)}
                      className="border border-gray-300 rounded-md px-2 w-full h-[35px] focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184]"
                      placeholder="0"
                    />
                  </div>
                </div>

                <div>
                  <label htmlFor="remarks" className="block text-sm font-medium text-gray-700 mb-1">
                    {t('manualWithdrawalForm.remarks')}
                  </label>
                  <textarea
                    id="remarks"
                    value={remarks}
                    onChange={(e) => setRemarks(e.target.value)}
                    className="border border-gray-300 rounded-md p-2 w-full focus:outline-none focus:ring-1 focus:ring-[#214184] focus:border-[#214184] min-h-[120px]"
                    placeholder={t('manualWithdrawalForm.remarksPlaceholder')}
                  />
                </div>
              </div>
            </div>
          </div>

          {/* Right side - Upload image and buttons */}
          <div className="flex flex-col items-center pl-20">
            <div className="w-[350px] h-[450px] border-[2px] border-dashed border-[#163f78] rounded-[5px] flex flex-col items-center justify-center mb-6 relative">
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
                    <span className="text-[#163f78] cursor-pointer underline">Click to upload</span> <span className="text-[#333]">or drag</span>
                  </p>
                  <span className="text-sm font-medium text-[#333] mb-2">and drop</span>
                  <span className="text-xs font-medium text-[#333]">Upload jpeg, jpg, png</span>
                  <span className="text-xs font-medium text-[#333]">(Max 1 MB)</span>
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
            
            <div className="flex justify-center space-x-4 mt-3 w-full">
              <button
                type="button"
                onClick={onClose}
                className="w-[140px] h-[35px] border border-gray-300 rounded-full text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none"
              >
                {t('manualWithdrawalForm.cancel')}
              </button>
              <button
                type="submit"
                className="w-[140px] h-[35px] border border-transparent rounded-full text-sm font-medium text-white bg-[#163f78] hover:bg-[#0f2d5c] focus:outline-none"
              >
                {t('manualWithdrawalForm.approve')}
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ManualAddWithdrawalForm;