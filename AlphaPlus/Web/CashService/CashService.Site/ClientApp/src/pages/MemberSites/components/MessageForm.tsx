import React from 'react';

interface MessageFormProps {
  mobile: string;
  lineId: string;
  announcement: string;
  onMessageChange: (messages: { mobile?: string; lineId?: string; announcement?: string }) => void;
}

const MessageForm: React.FC<MessageFormProps> = ({
  mobile,
  lineId,
  announcement,
  onMessageChange
}) => {
  return (
    <div className="space-y-4">
      <h2 className="text-lg font-semibold text-sky-700 mb-4">Leave a Message</h2>
      <p className="text-sm text-gray-600 mb-4">Include your contact details and a message for your member.</p>
      <div className="space-y-4">
          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1">
                Mobile Number
              </label>
              <div className="flex">
                <span className="inline-flex items-center px-3 py-2 rounded-l-md border border-r-0 border-gray-300 bg-gray-50 text-gray-500 text-sm">
                  +66
                </span>
                <input
                  type="text"
                  value={mobile}
                  onChange={(e) => onMessageChange({ mobile: e.target.value })}
                  placeholder="XXXXXXXXX"
                  className="flex-1 px-3 py-2 border border-gray-300 rounded-r-md focus:outline-none"
                />
              </div>
            </div>
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-1" htmlFor="lineId">
                LINE ID
              </label>
              <input
                id="lineId"
                type="text"
                value={lineId}
                onChange={(e) => onMessageChange({ lineId: e.target.value })}
                className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none"
              />
            </div>
      
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            Announcement
      
          </label>   
    
          <textarea
            value={announcement}
            onChange={(e) => onMessageChange({ announcement: e.target.value })}
            rows={4}
            maxLength={1000}
            className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none"
            placeholder="Add your message here..."
          />
        </div>
      </div>
    </div>
  );
};

export default MessageForm; 