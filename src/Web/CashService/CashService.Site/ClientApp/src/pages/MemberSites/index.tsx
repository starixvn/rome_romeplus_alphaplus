import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService } from '../../services/memberSitesAPI';
import { Setting, MessageData, MemberSiteResponse } from '../../types/memberSite';
import { useTranslation } from 'react-i18next';
import i18n from '../../i18n/i18n-memberSite';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import { ResultMessage, ResultType } from '../../components/ResultMessageComponent';
import '../../styles/common.css';
import '../../styles/global.scss';

import TitleCommon from '../../components/TitleComponent';

const MemberSiteManagement: React.FC = () => {
  const { t } = useTranslation('memberSite');
  useCookieLanguage(i18n);
  const defaultData = {
    domains: [],
    generalSettings: [],
    countryCodes: [],
    countryCode: '',
    mobileCode: '',
    mobilePhone: '',
    mobile: '',
    lineId: '',
    announcement: '',
    contacts: '',
    telegram: ''
  }
  const [originalData, setOriginalData] = useState<MemberSiteResponse>(defaultData);
  const [data, setData] = useState<MemberSiteResponse>(defaultData);
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState<{ type: ResultType; text: string } | null>(null);
  const [isDirty, setIsDirty] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const response = await memberSitesService.getMemberSiteData();
      setData(response);
      setOriginalData(response);
      setResult(null);
    } catch (err) {
      setResult({
        type: 'error',
        text: t('errors.loadFailed')
      });
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSettingChange = (settingId: string, value: boolean) => {
    const newData = {
      ...data,
      generalSettings: data.generalSettings.map((setting: Setting) =>
        setting.id === settingId ? { ...setting, value } : setting
      )
    }
    const isDirty = JSON.stringify(newData) !== JSON.stringify(originalData);
    setData(newData);
    setIsDirty(isDirty);
  };

  const handleMessageChange = (messages: MessageData) => {
    const newData = {
      ...data,
      ...messages
    }
    const isDirty = JSON.stringify(newData) !== JSON.stringify(originalData);
    setData(newData);
    setIsDirty(isDirty);
  };

  const handleSave = async () => {
    try {
      setIsSaving(true);
      const response = await memberSitesService.updateMemberSiteData({
        ...data,
        mobile: `${data.mobileCode}-${data.mobilePhone}`,
        contacts: JSON.stringify([{platform: 'tele', value: data.telegram}, {platform: 'line', value: data.lineId}]),
      });


      setIsDirty(false);
      setResult({
        type: response.isSuccessful ? 'success' : 'error',
        text: response.isSuccessful ? t('messages.updateSuccess') : response.message
      });
      setTimeout(() => {
        setOriginalData(data);
        setResult(null);
      }, 3000);

    } catch (err) {
      setResult({
        type: 'error',
        text: t('errors.updateFailed')
      });
      console.error('Error updating data:', err);
    } finally {
      setIsSaving(false);
    }
  };

  const handleRequestNewLink = () => {
    // Implement request new link functionality
    console.log('Requesting new link...');
  };


  return (
    <form
      className=""
      onSubmit={(e) => {
        e.preventDefault();
        handleSave();
      }}
    >
      <div className="p-[12px]">
        <div className='mb-[8px]'>
          <TitleCommon title={t('title')} Tippy={false} />
        </div>
        <div className="bg-[#ffffff] rounded-[4px] max-w-[1200px]">
          {result && (
            <div className='pt-[24px] mx-[24px] max-[768px]:mx-[12px] max-[768px]:pt-[12px]'>
              <ResultMessage
                type={result.type}
                message={result.text}
              />
            </div>
          )}

          <div className="p-[24px] max-[768px]:p-[12px]">
            <div className="">
              <RegistrationLinks
                domains={data.domains.map(domain => ({ ...domain, status: 'active' }))}
                onRequestNewLink={handleRequestNewLink}
                loading={loading}
              />
            </div>
            <hr className='my-[16px] border-[#0000001F]' />
            <div className="">
              <GeneralSettings
                settings={data.generalSettings}
                onSettingsChange={handleSettingChange}
                loading={loading}
              />
            </div>
            <hr className='my-[16px] border-[#0000001F]' />
            <div className="">
              <MessageForm
                telegram={data.telegram}
                countryCodes={data.countryCodes}
                mobilePhone={data.mobilePhone}
                mobileCode={data.mobileCode}
                lineId={data.lineId}
                announcement={data.announcement || ''}
                onMessageChange={handleMessageChange}
                loading={loading}
              />
            </div>

            <div className="flex justify-start py-[8px] mt-[8px]">
              <button
                type="submit"
                disabled={!isDirty || isSaving}
                className={`w-[140px] h-[30px] rounded-[20px] text-[var(--button-text)] px-[16px] py-[4px] text-[12px] border-[1px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:h-[38px] font-bold
                  ${isDirty && !isSaving
                    ? 'border-[var(--button-border)] bg-[var(--button-background)] hover:bg-[var(--button-background-hover)] hover:border-[#375A8A] cursor-pointer disabled:opacity-70'
                    : 'bg-[var(--button-background-hover)] border-[var(--button-border-hover)] cursor-not-allowed disabled:opacity-70'}`}
              >
                {isSaving ? t('actions.saving') : t('actions.save')}
              </button>
            </div>
          </div>
        </div>
      </div>
    </form>
  );
};

export default MemberSiteManagement;

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <MemberSiteManagement />
  </React.StrictMode>
);
