import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService } from '../../services/memberSitesAPI';
import { Setting, MessageData, MemberSiteResponse } from '../../types/memberSite';
import '../../styles/common.css'
import { useTranslation } from 'react-i18next';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import { ResultMessage, ResultType } from '../../components/ResultMessage';

const MemberSiteManagement: React.FC = () => {
  const { t } = useTranslation('memberSite');
  useCookieLanguage();
  const defaultData = {
    domains: [],
    generalSettings: [],
    countryCodes: [],
    countryCode: '',
    mobileCode: '',
    mobilePhone: '',
    mobile: '',
    lineId: '',
    announcement: ''
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
        mobile: `${data.mobileCode}-${data.mobilePhone}`
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
          <h1 className="flex items-center font-bold text-[15px] text-[#333333] max-[768px]:text-[19px]">{t('title')}</h1>
        </div>
        <div className="bg-[#ffffff] rounded-[4px] max-w-[1200px]">
          {result && (
            <ResultMessage
              type={result.type}
              message={result.text}
            />
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
                countryCodes={data.countryCodes}
                mobilePhone={data.mobilePhone}
                mobileCode={data.mobileCode}
                lineId={data.lineId}
                announcement={data.announcement}
                onMessageChange={handleMessageChange}
                loading={loading}
              />
            </div>

            <div className="flex justify-end pt-4">
              <button
                type="submit"
                disabled={!isDirty || isSaving}
                className={`w-[140px] h-[30px] rounded-[20px] text-[#ffffff] px-[16px] py-[4px] text-[12px] border-[1px]  ml-[8px] shadow-[0_1px_5px_#0003,0_2px_2px_#00000024,0_3px_1px_-2px_#0000001f] max-[768px]:text-[16px] max-[768px]:leading-[16px] max-[768px]:h-[38px] font-bold
                  ${isDirty && !isSaving
                    ? 'border-[var(--primary-color)] bg-[var(--primary-color)] hover:bg-[#375A8A] hover:border-[#375A8A] cursor-pointer disabled:opacity-70'
                    : 'bg-[#375A8A] border-[#375A8A] cursor-not-allowed disabled:opacity-70'}`}
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
