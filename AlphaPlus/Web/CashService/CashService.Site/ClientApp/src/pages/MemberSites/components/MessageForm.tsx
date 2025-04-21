import React, { useState, useEffect } from 'react';

interface MessageFormProps {
  mobile: string;
  lineId: string;
  announcement: string;
  onMessageChange: (messages: { mobile: string; lineId: string; announcement: string }) => void;
}

const MessageForm: React.FC<MessageFormProps> = ({ 
  mobile: initialMobile,
  lineId: initialLineId,
  announcement: initialAnnouncement,
  onMessageChange
}) => {
  const [mobile, setMobile] = useState(initialMobile);
  const [lineId, setLineId] = useState(initialLineId);
  const [announcement, setAnnouncement] = useState(initialAnnouncement);

  useEffect(() => {
    setMobile(initialMobile);
    setLineId(initialLineId);
    setAnnouncement(initialAnnouncement);
  }, [initialMobile, initialLineId, initialAnnouncement]);

  const handleChange = () => {
    onMessageChange({ mobile, lineId, announcement });
  };

  return (
    <div className="space-y-4">
      <h2 className="text-lg font-medium mb-4">Contact Information</h2>
      <div className="space-y-6">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Mobile Number
          </label>
          <input
            type="text"
            value={mobile}
            onChange={(e) => {
              setMobile(e.target.value);
              handleChange();
            }}
            placeholder="+66XXXXXXXXX"
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-[#1e4d8c]"
          />
          <p className="mt-1 text-sm text-gray-500">Format: +66XXXXXXXXX</p>
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            LINE ID
          </label>
          <input
            type="text"
            value={lineId}
            onChange={(e) => {
              setLineId(e.target.value);
              handleChange();
            }}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-[#1e4d8c]"
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Announcement
          </label>
          <textarea
            value={announcement}
            onChange={(e) => {
              setAnnouncement(e.target.value);
              handleChange();
            }}
            rows={4}
            maxLength={1000}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-[#1e4d8c]"
          />
          <p className="mt-1 text-sm text-gray-500">
            {announcement.length}/1000 characters
          </p>
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 