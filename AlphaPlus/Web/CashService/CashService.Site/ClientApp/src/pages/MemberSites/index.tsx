import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService } from '../../services/memberSitesAPI';
import { MemberSiteData, Setting, Domain, MessageData } from '../../types/memberSite';
import './MemberSiteStyle.css'

const defaultSettings: Setting[] = [
  {
    id: 'allowRegistration',
    name: 'Registration',
    description: 'Allow member to register',
    value: false,
    category: 'Access Control'
  },
  {
    id: 'allowDeposit',
    name: 'Deposit',
    description: 'Allow member to deposit',
    value: false,
    category: 'Access Control'
  }
];

const MemberSiteManagement: React.FC = () => {
  const [originalData, setOriginalData] = useState<MemberSiteData>({
    domains: [],
    settings: defaultSettings,
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [data, setData] = useState<MemberSiteData>({
    domains: [],
    settings: defaultSettings,
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
        name: `Domain ${domain.url.split('.')[1]}`,
        status: 'active' as const
      }));

      const transformedSettings = defaultSettings.map(setting => ({
        ...setting,
        value: Boolean((response as any)[setting.id])
      }));

      setData({
        domains: transformedDomains,
        settings: transformedSettings,
        mobile: response.mobile || '',
        lineId: response.lineId || '',
        announcement: response.announcement || ''
      });
      setOriginalData({
        domains: transformedDomains,
        settings: transformedSettings,
        mobile: response.mobile || '',
        lineId: response.lineId || '',
        announcement: response.announcement || ''
      });
      setError(null);
    } catch (err) {
      setError('Failed to load member site data');
      console.error('Error loading data:', err);
    } finally {
      setLoading(false);
    }
  };

  const handleSettingChange = (settingId: string, value: boolean) => {
    const newData = {
      ...data,
      settings: data.settings.map(setting => 
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
      const newData: Partial<MemberSiteData> = {};

      const response = await memberSitesService.updateMemberSiteData(newData);
      
      // Transform API response back to our data format
      const transformedDomains: Domain[] = response.domains.map(domain => ({
        id: domain.url,
        url: domain.url,
        qrCode: domain.qrCode,
        name: `Domain ${domain.url.split('.')[1]}`,
        status: 'active' as const
      }));

      const transformedSettings = data.settings.map(setting => ({
        ...setting,
        value: Boolean((response as any)[setting.id])
      }));

      setData(prev => ({
        ...prev,
        domains: transformedDomains,
        settings: transformedSettings
      }));
      setIsDirty(false);
      setError(null);
    } catch (err) {
      setError('Failed to update member site data');
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
      <div className="min-h-screen bg-gray-50 p-6">
        <div className="max-w-5xl mx-auto">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 rounded w-1/4"></div>
            <div className="h-[600px] bg-gray-200 rounded"></div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <form 
      className="min-h-screen bg-gray-50 p-6"
      onSubmit={(e) => {
        e.preventDefault();
        handleSave();
      }}
    >
      <div className="container mx-auto">
        <h1 className="text-xl font-semibold mb-3">Member Site Management</h1>

        <div className="bg-white rounded-lg shadow-sm p-6">
        
          
          {error && (
            <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative mb-6" role="alert">
              <strong className="font-bold">Error!</strong>
              <span className="block sm:inline"> {error}</span>
            </div>
          )}

          <div className="space-y-8">
            <div className="border-b-2 border-gray-200 pb-6">
              <RegistrationLinks 
                domains={data.domains}
                onRequestNewLink={handleRequestNewLink}
              />
            </div>

            <div className="border-b-2 border-gray-200 pb-6">
              <GeneralSettings 
                settings={data.settings}
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
                    ? 'bg-[#1e4d8c] hover:bg-[#173d70] cursor-pointer' 
                    : 'bg-gray-400 cursor-not-allowed'}`}
              >
                {isSaving ? 'Saving...' : 'Save Changes'}
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
