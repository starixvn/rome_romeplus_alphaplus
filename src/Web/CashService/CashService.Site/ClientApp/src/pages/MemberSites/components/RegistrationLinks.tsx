import React from 'react';
import { Domain } from '../../../types/memberSite';
import DomainCard from './DomainCard';
import { useTranslation } from 'react-i18next';

interface Props {
  domains: Domain[];
  onRequestNewLink?: () => void;
}

const RegistrationLinks: React.FC<Props> = ({ domains, onRequestNewLink }) => {
  const { t } = useTranslation('memberSite');

  // check if all link is blocked
  const allDomainsBlocked = domains.every(d => d.status === 'blocked');
    
  return (
    <div className="">
      <div className="flex justify-between items-center">
        <div>
          <h2 className="text-[16px] text-[var(--primary-color)] font-semibold leading-[21.6px] tracking-[0.2px] max-[768px]:text-[20px]">{t('registrationLinks.title')}</h2>
          <p className="text-[12px] pt-[4px] mb-[16px] font-semibold max-[768px]:text-[16px]">
            {t('registrationLinks.note')}
          </p>
        </div>
        {allDomainsBlocked && (
          <button
            onClick={onRequestNewLink}
            className="px-4 py-2 text-sm bg-[#1e4d8c] text-white rounded-md hover:bg-[#173d70] transition-colors"
          >
            {t('registrationLinks.requestNewLink')}
          </button>
        )}
      </div>

      <div className="flex gap-[30px] w-[83.333%] max-[768px]:w-full max-[768px]:flex-col">
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