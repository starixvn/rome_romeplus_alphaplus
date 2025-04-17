import React from 'react';

interface Domain {
  domain: string;
  url: string;
}

interface Props {
  domains: Domain[];
}

const RegistrationLinks: React.FC<Props> = ({ domains }) => {
  return (
    <div className="space-y-4">
      <h2 className="text-lg font-medium mb-2">Registration Links</h2>
      <p className="text-sm text-gray-600 mb-4">If both links are blocked, request a new link.</p>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {domains.map((item, index) => (
          <div key={index} className="bg-[#1e4d8c] text-white p-4 rounded-lg flex justify-between items-center">
            <div className="space-y-1">
              <h3 className="font-medium">{item.domain}</h3>
              <p className="text-sm text-gray-200">{item.url}</p>
            </div>
            <button className="bg-white text-[#1e4d8c] px-4 py-2 rounded text-sm hover:bg-gray-100 transition-colors">
              Download QR Code
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default RegistrationLinks; 