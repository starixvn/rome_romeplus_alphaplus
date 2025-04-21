import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService, MemberSiteData } from '../../services/memberSitesAPI';
import './MemberSiteStyle.css'

const MemberSiteManagement: React.FC = () => {
  const [data, setData] = useState<MemberSiteData>({
    domains: [],
    allowRegistration: false,
    allowDeposit: false,
    mobile: '',
    lineId: '',
    announcement: ''
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await memberSitesService.getMemberSiteData();
        const newData = {
          domains: response.domains,
          allowRegistration: response.allowRegistration,
          allowDeposit: response.allowDeposit,
          mobile: response.mobile || '',
          lineId: response.lineId || '',
          announcement: response.announcement || ''
        }
        setData(newData);
        setError(null);
      } catch (err) {
        setError('Failed to load member site data');
        console.error('Error loading data:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const handleSettingsChange = async (newData: Partial<MemberSiteData>) => {
    try {
      const updatedData = { ...data, ...newData };
      const response = await memberSitesService.updateMemberSiteData(updatedData);
      setData(response);
    } catch (err) {
      setError('Failed to update member site data');
      console.error('Error updating data:', err);
    }
  };

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 p-6">
        <div className="max-w-5xl mx-auto">
          <div className="animate-pulse space-y-6">
            <div className="h-8 bg-gray-200 rounded w-1/4"></div>
            <div className="space-y-4">
              <div className="h-32 bg-gray-200 rounded"></div>
              <div className="h-32 bg-gray-200 rounded"></div>
              <div className="h-32 bg-gray-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="min-h-screen bg-gray-50 p-6">
        <div className="max-w-5xl mx-auto">
          <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded relative" role="alert">
            <strong className="font-bold">Error!</strong>
            <span className="block sm:inline"> {error}</span>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-5xl mx-auto space-y-6">
        <h1 className="text-xl font-semibold mb-6">Member Site Management</h1>
        <div className="space-y-6">
          <div className="bg-white rounded-lg shadow-sm p-6">
            <RegistrationLinks domains={data.domains} />
          </div>
          <div className="bg-white rounded-lg shadow-sm p-6">
            <GeneralSettings 
              allowRegistration={data.allowRegistration}
              allowDeposit={data.allowDeposit}
              onSettingsChange={(settings) => handleSettingsChange(settings)}
            />
          </div>
          <div className="bg-white rounded-lg shadow-sm p-6">
            <MessageForm 
              mobile={data.mobile}
              lineId={data.lineId}
              announcement={data.announcement}
              onMessageChange={(messages) => handleSettingsChange(messages)}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default MemberSiteManagement;

createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <MemberSiteManagement />
  </React.StrictMode>
);
