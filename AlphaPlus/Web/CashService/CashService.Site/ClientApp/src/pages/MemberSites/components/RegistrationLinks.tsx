import React from 'react';
import DomainCard from './DomainCard';

interface RegistrationLinksProps {
  domains: Array<{ domain: string; url: string }>;
}

const RegistrationLinks: React.FC<RegistrationLinksProps> = ({ domains }) => {
  return (
    <div className="registration-links">
      <h2 className="member-sites-title">Registration Links</h2>
      <p className="text-gray-600 mb-6">If both links are blocked, request a new link.</p>
      <div className="grid grid-cols-1 gap-4">
        {domains.map((domain, index) => (
          <DomainCard key={index} {...domain} />
        ))}
      </div>
    </div>
  );
};

export default RegistrationLinks; 