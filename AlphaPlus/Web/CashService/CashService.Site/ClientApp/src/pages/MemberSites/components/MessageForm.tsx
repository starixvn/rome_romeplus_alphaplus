import React, { useState } from 'react';

const MessageForm: React.FC = () => {
  const [mobile, setMobile] = useState('');
  const [lineId, setLineId] = useState('');
  const [announcement, setAnnouncement] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    // Handle form submission
  };

  return (
    <div className="space-y-4">
      <h2 className="text-lg font-medium mb-4">Leave a Message</h2>
      <p className="text-sm text-gray-600 mb-6">Include your contact details and a message for your member.</p>
      
      <form onSubmit={handleSubmit} className="space-y-6" data-testid="message-form">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div className="space-y-2">
            <label htmlFor="mobile" className="block text-sm font-medium text-gray-700">
              Mobile
            </label>
            <div className="flex">
              <span className="inline-flex items-center px-3 text-sm text-gray-900 bg-gray-100 border border-r-0 border-gray-300 rounded-l-md">
                +66
              </span>
              <input
                type="text"
                id="mobile"
                value={mobile}
                onChange={(e) => setMobile(e.target.value)}
                className="rounded-none rounded-r-lg border text-gray-900 block flex-1 min-w-0 w-full text-sm border-gray-300 p-2.5 focus:ring-blue-500 focus:border-blue-500"
                placeholder="Enter mobile number"
              />
            </div>
          </div>

          <div className="space-y-2">
            <label htmlFor="lineId" className="block text-sm font-medium text-gray-700">
              LINE ID
            </label>
            <input
              type="text"
              id="lineId"
              value={lineId}
              onChange={(e) => setLineId(e.target.value)}
              className="border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 focus:ring-blue-500 focus:border-blue-500"
              placeholder="Enter LINE ID"
            />
          </div>
        </div>

        <div className="space-y-2">
          <label htmlFor="announcement" className="block text-sm font-medium text-gray-700">
            Announcement
          </label>
          <textarea
            id="announcement"
            value={announcement}
            onChange={(e) => setAnnouncement(e.target.value)}
            rows={4}
            className="border border-gray-300 text-gray-900 text-sm rounded-lg block w-full p-2.5 focus:ring-blue-500 focus:border-blue-500"
            placeholder="Add your message here..."
          />
        </div>

        <div className="flex justify-end">
          <button
            type="submit"
            className="bg-[#1e4d8c] text-white px-6 py-2.5 rounded-lg text-sm font-medium hover:bg-[#173d70] transition-colors"
          >
            Save
          </button>
        </div>
      </form>
    </div>
  );
};

export default MessageForm; 