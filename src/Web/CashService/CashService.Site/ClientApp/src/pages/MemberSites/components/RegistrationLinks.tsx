import React from 'react';
import { Domain } from '../../../services/interfaces/MemberSites';
import DomainCard from './DomainCard';
import { useTranslation } from 'react-i18next';

interface Props {
  domains: Domain[];
  loading: boolean;
}

const RegistrationLinksSkeleton = () => {
  return (
    <div>
      {/* Title and subtitle skeleton */}
      <div>
        <div className="h-[22px] w-[180px] bg-gray-200 rounded mb-2 animate-pulse"></div>
        <div className="h-[16px] w-[320px] bg-gray-100 rounded mb-4 animate-pulse"></div>
      </div>
      {/* 2 domain card skeletons */}
      <div className="flex gap-[30px] w-[83.333%] max-[768px]:w-full max-[768px]:flex-col">
        <div className="h-[70px] w-full bg-gray-200 rounded animate-pulse"></div>
        <div className="h-[70px] w-full bg-gray-200 rounded animate-pulse"></div>
      </div>
    </div>
  );
};

const RegistrationLinks: React.FC<Props> = ({ domains, loading }) => {
  const { t } = useTranslation('memberSite');

  if (loading) {
    return <RegistrationLinksSkeleton />
  }

  return (
    <div className="">
      <div className="flex justify-between items-center">
        <div>
          <h2 className="text-[16px] text-[var(--text-color-primary)] font-semibold max-[768px]:text-[20px]">{t('registrationLinks.title')}</h2>
          <p className="pt-[4px] mb-[16px] font-semibold">
            {t('registrationLinks.note')}
          </p>
        </div>
      </div>

      <div className="flex gap-[30px] flex-wrap w-[100%] max-[768px]:w-full max-[768px]:flex-col">
        {domains.map((domain, index) => (
          <DomainCard
            key={domain.id}
            domain={domain}
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