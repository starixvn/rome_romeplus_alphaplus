import React from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import './MemberSiteStyle.css'
const MemberSiteManagement: React.FC = () => {
  const domains = [
    {
      domain: 'Domain 1',
      url: 'https://quickm-6358906.hdupceris78.com'
    },
    {
      domain: 'Domain 2',
      url: 'https://quickm-6358906.namnamwang.com'
    }
  ];

  return (
    <div className="min-h-screen bg-gray-50 p-6">
      <div className="max-w-5xl mx-auto space-y-6">
        <h1 className="text-xl font-semibold mb-6">Member Site Management</h1>
        <div className="space-y-6">
          <div className="bg-white rounded-lg shadow-sm p-6">
            <RegistrationLinks domains={domains} />
          </div>
          <div className="bg-white rounded-lg shadow-sm p-6">
            <GeneralSettings />
          </div>
          <div className="bg-white rounded-lg shadow-sm p-6">
            <MessageForm />
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
