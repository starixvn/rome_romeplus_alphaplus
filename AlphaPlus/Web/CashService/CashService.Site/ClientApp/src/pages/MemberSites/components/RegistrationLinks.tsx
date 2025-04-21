import React from 'react';
import { Domain } from '../../../services/memberSitesAPI';
import DomainCard from './DomainCard';

interface Props {
  domains: Domain[];
}

const RegistrationLinks: React.FC<Props> = ({ domains }) => {
  return (
    <div className="space-y-4">
      <h2 className="text-lg font-medium mb-2">Registration Links</h2>
      <p className="text-sm text-gray-600 mb-4">If both links are blocked, request a new link.</p>
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {domains.map((domain, index) => (
          <DomainCard key={index} domain={domain} />
        ))}
      </div>
    </div>
  );
};

export default RegistrationLinks; 