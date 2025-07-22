import React from 'react';
import { Domain } from '../../../services/interfaces/MemberSites';
import DomainCard from './DomainCard';
import { useTranslation } from 'react-i18next';
import { TitleComponent } from '../../../components/Title';
import { SetupChecklistModel } from '../../../services/interfaces/SetupChecklistModel';
import { IconInfo } from '../../../components/Icons';

declare global {
  interface Window {
    indexManager: any;
  }
}

interface Props {
  domains: Domain[];
  loading: boolean;
  setupChecklist: SetupChecklistModel | null; 
  isCashServiceEnabled: boolean;
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

const RegistrationLinks: React.FC<Props> = ({ domains, loading, setupChecklist, isCashServiceEnabled }) => {
  const { t } = useTranslation('memberSite');

  if (loading) {
    return <RegistrationLinksSkeleton />
  }

  const showChecklist = () => {
    window.top && window.top.indexManager.showHideCashServiceChecklist(true);
  }

  return (
    <>
      <div className="flex justify-between items-center">
        <div>
          <TitleComponent type='medium' text={t('registrationLinks.title')} />          
          <p className="pt-[4px] mb-[16px]">
            {t('registrationLinks.note')}
          </p>
        </div>
      </div>
      { isCashServiceEnabled && (!setupChecklist ||!setupChecklist.isCompleteAll) && (
        <div className="bg-[#E1F1FF] dark:bg-[#13314D] border-[#7FB3E0] dark:border-[#40A3FF] relative font-[500] border-[1px] rounded-[3px] pl-[35px] pr-[7px] py-[8px] leading-[18px] mb-[10px]"><IconInfo className="w-[20px] h-[20px] fill-[#1C62A3] absolute top-[7px] left-[10px] ml-0!" />{t('global:pleaseReferTo')} <button className='font-bold text-[var(--text-link-color)] underline cursor-pointer' type="button" onClick={showChecklist}>{t('global:checklist')}</button></div>
      )}
      <div className="flex gap-[30px] flex-wrap w-[100%] max-[768px]:flex-col">
        {domains.map((domain, index) => (
          <DomainCard
            key={`${domain.id}-${index}`}
            domain={domain}
            index={index}
            disabled={!setupChecklist?.isCompleteAll}
          />
        ))}
      </div>

      {domains.length === 0 && (
        <div className="text-center py-8 bg-[var(--background-button)] border-[1px] border-[var(--border-color)] rounded-[3px]">           
          <TitleComponent type='medium' text={t('registrationLinks.noRegistrationLinks')} />
        </div>
      )}
    </>
  );
};

export default RegistrationLinks; 