import React, { useEffect, useState } from 'react';
import '../../i18n/i18n';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService } from '../../services/memberSitesAPI';
import { Setting, Domain, MessageData, MemberSiteResponse } from '../../types/memberSite';
import './MemberSiteStyle.css'
import { useTranslation } from 'react-i18next';


const MemberSiteManagement: React.FC = () => {
  const { t } = useTranslation('memberSite');
  const [originalData, setOriginalData] = useState<MemberSiteResponse>({
    domains: [],
    generalSettings: [],
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [data, setData] = useState<MemberSiteResponse>({
    domains: [],
    generalSettings: [],
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [isDirty, setIsDirty] = useState(false);
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      setLoading(true);
      const response = await memberSitesService.getMemberSiteData();

      // Transform API response to our data format
      const transformedDomains: Domain[] = response.domains.map(domain => ({
        id: domain.url,
        url: domain.url,
        qrCode: domain.qrCode,
        name: `${t('registrationLinks.domain')} ${domain.url.split('.')[1]}`,
        status: 'active' as const
      }));

      const transformedSettings = response.generalSettings.map(setting => ({
        ...setting,
        value: Boolean((response as any)[setting.id])
      }));

      setData({
        domains: transformedDomains,
        generalSettings: transformedSettings,
        mobile: response.mobile || '',
        lineId: response.lineId || '',
        announcement: response.announcement || ''
      });
      setOriginalData({
        domains: transformedDomains,
        generalSettings: transformedSettings,
        mobile: response.mobile || '',
        lineId: response.lineId || '',
        announcement: response.announcement || ''
      });
      setError(null);
    } catch (err) {
      setError(t('errors.loadFailed'));
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
      // Convert our data format back to API format
      const newData: Partial<MemberSiteResponse> = {};

      const response = await memberSitesService.updateMemberSiteData(newData);

      // Transform API response back to our data format
      const transformedDomains: Domain[] = response.domains.map((domain, index) => ({
        id: domain.url,
        url: domain.url,
        qrCode: domain.qrCode,
        name: `${t('registrationLinks.domain')} ${index + 1}`,
        status: 'active' as const
      }));

      const transformedSettings = data.generalSettings.map((setting: Setting) => ({
        ...setting,
        value: Boolean((response as any)[setting.id])
      }));

      setData((prev: MemberSiteResponse) => ({
        ...prev,
        domains: transformedDomains,
        generalSettings: transformedSettings
      }));
      setIsDirty(false);
      setError(null);
    } catch (err) {
      setError(t('errors.updateFailed'));
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
      className="min-h-screen bg-gray-50 dark:bg-[#1E1E1E] p-6 transition-colors duration-200"
      onSubmit={(e) => {
        e.preventDefault();
        handleSave();
      }}
    >
      <div className="container mx-auto">
        <h1 className="text-xl font-semibold mb-3 text-gray-900 dark:text-white">{t('title')}</h1>

        <div className="bg-white dark:bg-[#2D2D2D] rounded-lg shadow-sm p-6">
          {error && (
            <div className="bg-red-100 dark:bg-red-900/30 border border-red-400 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded relative mb-6" role="alert">
              <strong className="font-bold">{t('errors.title')}</strong>
              <span className="block sm:inline"> {error}</span>
            </div>
          )}

          <div className="space-y-8">
            <div className="border-b-2 border-gray-200 dark:border-gray-700 pb-6">
              <RegistrationLinks
                domains={data.domains}
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
