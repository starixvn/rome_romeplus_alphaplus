import React from 'react';
import { Domain } from '../../../services/interfaces/MemberSites';
import DomainCard from './DomainCard';
import { useTranslation } from 'react-i18next';
import { MediumTitleComponent } from '../../../components/TitleComponents';

interface Props {
  domains: Domain[];
  loading: boolean;
}

const RegistrationLinksSkeleton = () => {
  return (
    <div>
      {/* Title and subtitle skeleton */}
      <div>
        <div className="h-[22px] w-[180px] bg-gray-200 dark:bg-gray-700 rounded mb-2 animate-pulse"></div>
        <div className="h-[16px] w-[320px] bg-gray-100 dark:bg-gray-700 rounded mb-4 animate-pulse"></div>
      </div>
      {/* 2 domain card skeletons */}
      <div className="flex gap-[30px] w-[83.333%] max-[768px]:w-full max-[768px]:flex-col">
        <div className="h-[70px] w-full bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
        <div className="h-[70px] w-full bg-gray-200 dark:bg-gray-700 rounded animate-pulse"></div>
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
    <>
      <div className="flex justify-between items-center">
        <div>
          <MediumTitleComponent text={t('registrationLinks.title')} />          
          <p className="pt-[4px] mb-[16px]">
            {t('registrationLinks.note')}
          </p>
        </div>
      </div>

      <div className="flex gap-[30px] flex-wrap w-[100%] max-[768px]:flex-col">
        {domains.map((domain, index) => (
          <DomainCard
            key={`${domain.id}-${index}`}
            domain={domain}
            index={index}
          />
        ))}
      </div>

      {domains.length === 0 && (
        <div className="text-center py-8 bg-[var(--background-button)] border-[1px] border-[var(--border-color)] rounded-[3px]">           
          <MediumTitleComponent text={t('registrationLinks.noRegistrationLinks')} />
        </div>
      )}
    </>
  );
};

export default RegistrationLinks; 