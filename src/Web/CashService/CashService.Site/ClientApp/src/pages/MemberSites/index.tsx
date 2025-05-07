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
  const [originalData, setOriginalData] = useState<MemberSiteResponse>({
    domains: [],
    generalSettings: [],
    countryCode: '',
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [data, setData] = useState<MemberSiteResponse>({
    domains: [],
    generalSettings: [],
    countryCode: '',
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState<{type: ResultType; text: string} | null>(null);
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
      const response = await memberSitesService.updateMemberSiteData(data);
        

      setIsDirty(false);
      setResult({
        type: response.isSuccessful ? 'success' : 'error',
        text: response.isSuccessful ? t('messages.updateSuccess') : response.message
      });
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

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 dark:bg-[#1E1E1E] p-6">
        <div className="container mx-auto">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 dark:bg-gray-700 rounded w-1/4"></div>
            <div className="h-[600px] bg-gray-200 dark:bg-gray-700 rounded"></div>
          </div>
        </div>
      </div>
    );
  }

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
          <h1 className="flex items-center font-bold text-[15px] text-[#333333]">{t('title')}</h1>
        </div>
        <div className="bg-[#ffffff] rounded-[4px] max-w-[1200px]">
          {result && (
            <ResultMessage
              type={result.type}
              message={result.text}
              className="mb-6"
            />
          )}

          <div className="p-[24px]">
            <div className="border-b-2 border-gray-200 dark:border-gray-700 pb-6">
              <RegistrationLinks
                domains={data.domains.map(domain => ({...domain, status: 'active'}))}
                onRequestNewLink={handleRequestNewLink}
              />
            </div>

            <div className="border-b-2 border-gray-200 dark:border-gray-700 pb-6">
              <GeneralSettings
                settings={data.generalSettings}
                onSettingsChange={handleSettingChange}
              />
            </div>

            <div className="pb-6">
              <MessageForm
                mobile={data.mobile}
                lineId={data.lineId}
                announcement={data.announcement}
                onMessageChange={handleMessageChange}
              />
            </div>

            <div className="flex justify-end pt-4">
              <button
                type="submit"
                disabled={!isDirty || isSaving}
                className={`px-6 py-2 rounded-md text-white transition-colors
                  ${isDirty && !isSaving
                    ? 'bg-[#2C3E50] hover:bg-[#1e4d8c] dark:bg-[#1e4d8c] dark:hover:bg-[#173d70] cursor-pointer'
                    : 'bg-gray-400 dark:bg-gray-600 cursor-not-allowed'}`}
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
