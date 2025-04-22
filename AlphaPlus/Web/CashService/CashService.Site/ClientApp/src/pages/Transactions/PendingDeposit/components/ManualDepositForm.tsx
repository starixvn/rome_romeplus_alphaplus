import React, { useState } from 'react';

interface ManualDepositFormProps {
  onClose: () => void;
  onSubmit: (data: {
    memberLoginName: string;
    from: string;
    to: string;
    depositAmount: string;
    remarks: string;
    image?: File;
  }) => void;
}

const ManualDepositForm: React.FC<ManualDepositFormProps> = ({ onClose, onSubmit }) => {
  const [memberLoginName, setMemberLoginName] = useState('999b20a00001');
  const [from, setFrom] = useState('12345678');
  const [to, setTo] = useState('1493993018');
  const [depositAmount, setDepositAmount] = useState('');
  const [remarks, setRemarks] = useState('');
  const [image, setImage] = useState<File | null>(null);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit({
      memberLoginName,
      from,
      to,
      depositAmount,
      remarks,
      image: image || undefined
    });
  };

  const handleImageUpload = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (e.target.files && e.target.files[0]) {
      setImage(e.target.files[0]);
    }
  };

  return (
    <div className="fixed inset-0 bg-white flex flex-col z-50">
      <div className="bg-gray-100 p-4">
        <h2 className="text-lg font-medium">Add Manual Deposit</h2>
      </div>

      <form onSubmit={handleSubmit} className="p-6 flex-1">
        <div className="flex flex-row space-x-20">
          {/* Left side - Account information */}
          <div className="flex-1">
            <div className="mb-5">
              <h3 className="text-md font-medium text-gray-700 mb-4">Account Information</h3>
              
              <div className="flex flex-col space-y-4">
                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label htmlFor="memberLoginName" className="block text-sm font-medium text-gray-700 mb-1">
                      Member Login Name*
                    </label>
                    <div className="relative">
                      <select
                        id="memberLoginName"
                        value={memberLoginName}
                        onChange={(e) => setMemberLoginName(e.target.value)}
                        className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 appearance-none focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
                      >
                        <option value="999b20a00001">999b20a00001</option>
                      </select>
                      <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2">
                        <svg className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                        </svg>
                      </div>
                    </div>
                  </div>

                  <div>
                    <label htmlFor="from" className="block text-sm font-medium text-gray-700 mb-1">
                      From*
                    </label>
                    <div className="relative">
                      <div className="flex items-center">
                        <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-gray-200">
                          <span className="text-gray-600 text-xs">🌐</span>
                        </div>
                        <select
                          id="from"
                          value={from}
                          onChange={(e) => setFrom(e.target.value)}
                          className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 appearance-none focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
                        >
                          <option value="12345678">12345678</option>
                        </select>
                      </div>
                      <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2">
                        <svg className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                        </svg>
                      </div>
                    </div>
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label htmlFor="to" className="block text-sm font-medium text-gray-700 mb-1">
                      To*
                    </label>
                    <div className="relative">
                      <div className="flex items-center">
                        <div className="mr-2 flex items-center justify-center w-8 h-8 rounded-full bg-gray-200">
                          <span className="text-gray-600 text-xs">🌐</span>
                        </div>
                        <select
                          id="to"
                          value={to}
                          onChange={(e) => setTo(e.target.value)}
                          className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 appearance-none focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
                        >
                          <option value="1493993018">1493993018</option>
                        </select>
                      </div>
                      <div className="pointer-events-none absolute inset-y-0 right-0 flex items-center px-2">
                        <svg className="h-5 w-5 text-gray-400" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 9l-7 7-7-7" />
                        </svg>
                      </div>
                    </div>
                  </div>

                  <div>
                    <label htmlFor="depositAmount" className="block text-sm font-medium text-gray-700 mb-1">
                      Deposit Amount*
                    </label>
                    <input
                      id="depositAmount"
                      type="number"
                      value={depositAmount}
                      onChange={(e) => setDepositAmount(e.target.value)}
                      className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm"
                      placeholder="0"
                    />
                  </div>
                </div>

                <div>
                  <label htmlFor="remarks" className="block text-sm font-medium text-gray-700 mb-1">
                    Remarks
                  </label>
                  <textarea
                    id="remarks"
                    value={remarks}
                    onChange={(e) => setRemarks(e.target.value)}
                    className="block w-full rounded-md border border-gray-300 py-2 px-3 text-gray-900 focus:border-blue-500 focus:outline-none focus:ring-blue-500 sm:text-sm min-h-[120px]"
                    placeholder="Add a remark here..."
                  />
                </div>
              </div>
            </div>
          </div>

          {/* Right side - Upload image and buttons */}
          <div className="w-80 flex flex-col">
            <div className="h-[500px] border border-dashed border-gray-300 rounded-md flex flex-col items-center justify-center mb-6 relative">
              <div className="flex flex-col items-center justify-center text-center p-6">
                <svg className="h-10 w-10 text-gray-400 mb-4" stroke="currentColor" fill="none" viewBox="0 0 24 24" aria-hidden="true">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z" />
                </svg>
                <p className="text-sm text-blue-600 font-medium cursor-pointer">Click to upload</p>
                <p className="text-xs text-gray-500 mt-1">or drag and drop</p>
                <p className="text-xs text-gray-500">Upload jpeg, jpg, png</p>
                <p className="text-xs text-gray-500">(Max 1 MB)</p>
              </div>
              <input
                id="image-upload"
                type="file"
                accept=".jpeg,.jpg,.png"
                onChange={handleImageUpload}
                className="hidden"
              />
              <label
                htmlFor="image-upload"
                className="absolute inset-0 w-full h-full opacity-0 cursor-pointer z-10"
              />
            </div>
            
            <div className="flex justify-end space-x-3">
              <button
                type="button"
                onClick={onClose}
                className="px-6 py-2 border border-gray-300 rounded-md shadow-sm text-sm font-medium text-gray-700 bg-white hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="bg-[#1e4d8c] text-white px-6 py-2 rounded-md text-sm font-medium hover:bg-[#173d70] transition-colors"
              >
                Approve
              </button>
            </div>
          </div>
        </div>
      </form>
    </div>
  );
};

export default ManualDepositForm; 