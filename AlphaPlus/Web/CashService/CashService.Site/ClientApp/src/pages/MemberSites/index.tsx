import React from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';


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
    <div className="member-sites-container">
      <div className="member-sites-content">
        <h1 className="member-sites-title">Member Site Management</h1>
        <div className="member-sites-section">
          <RegistrationLinks domains={domains} />
          <GeneralSettings />
          <MessageForm />
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
