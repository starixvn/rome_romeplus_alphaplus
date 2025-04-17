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
    <div className="message-form">
      <h2 className="member-sites-title">Leave a Message</h2>
      <p className="text-gray-600 mb-6">Include your contact details and a message for your member.</p>
      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="space-y-2">
          <label className="block text-sm font-medium text-gray-700">Mobile</label>
          <div className="flex items-center">
            <span className="px-3 py-2 bg-gray-50 border border-r-0 border-gray-300 rounded-l-md text-gray-500">+66</span>
            <input
              type="text"
              value={mobile}
              onChange={(e) => setMobile(e.target.value)}
              className="form-input rounded-l-none"
              placeholder="Enter mobile number"
            />
          </div>
        </div>
        <div className="space-y-2">
          <label className="block text-sm font-medium text-gray-700">LINE ID</label>
          <input
            type="text"
            value={lineId}
            onChange={(e) => setLineId(e.target.value)}
            className="form-input"
            placeholder="Enter LINE ID"
          />
        </div>
        <div className="space-y-2">
          <label className="block text-sm font-medium text-gray-700">Announcement</label>
          <textarea
            value={announcement}
            onChange={(e) => setAnnouncement(e.target.value)}
            placeholder="Add your message here..."
            className="form-input min-h-[100px] resize-none"
          />
        </div>
        <button type="submit" className="form-button">
          Save
        </button>
      </form>
    </div>
  );
};

export default MessageForm; 