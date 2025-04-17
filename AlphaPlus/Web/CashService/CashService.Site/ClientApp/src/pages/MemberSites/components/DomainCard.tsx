import React from 'react';

interface DomainCardProps {
  domain: string;
  url: string;
}

const DomainCard: React.FC<DomainCardProps> = ({ domain, url }) => {
  return (
    <div className="domain-card">
      <div>
        <p className="font-semibold">{domain}</p>
        <p className="text-sm opacity-90 break-all">{url}</p>
      </div>
      <button className="domain-card-button">
        Download QR Code
      </button>
    </div>
  );
};

export default DomainCard; 