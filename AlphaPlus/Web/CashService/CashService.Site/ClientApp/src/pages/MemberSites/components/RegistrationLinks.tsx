import React from 'react';
import { Domain } from '../../../types/memberSite';
import DomainCard from './DomainCard';

interface Props {
  domains: Domain[];
  onRequestNewLink?: () => void;
}

const RegistrationLinks: React.FC<Props> = ({ domains, onRequestNewLink }) => {


  // check if all link is blocked
  const allDomainsBlocked = domains.every(d => d.status === 'blocked');
    
  return (
    <div className="space-y-4">
      <div className="flex justify-between items-center">
        <div>
          <h2 className="text-lg font-semibold text-sky-700">Registration Links</h2>
          <p className="text-sm text-gray-600">
            {allDomainsBlocked ? 'All links are blocked. You may request new ones.' : 'If bolth link are blocked request a new link'}
          </p>
        </div>
        {allDomainsBlocked && (
          <button
            onClick={onRequestNewLink}
            className="px-4 py-2 text-sm bg-[#1e4d8c] text-white rounded-md hover:bg-[#173d70] transition-colors"
          >
            Request New Link
          </button>
        )}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
        {domains.map((domain, index) => (
          <DomainCard 
            key={domain.id} 
            domain={domain}
            className={domain.status === 'blocked' ? 'opacity-50' : ''}
            index={index}
          />
        ))}
      </div>

      {domains.length === 0 && (
        <div className="text-center py-8 bg-gray-50 rounded-lg">
          <p className="text-gray-500">No registration links available</p>
        </div>
      )}
    </div>
  );
};

export default RegistrationLinks; 