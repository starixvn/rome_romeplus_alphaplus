import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import RegistrationLinks from './components/RegistrationLinks';
import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import { memberSitesService } from '../../services/memberSitesAPI';
import { Setting, MessageData, MemberSiteManagementData, MemberSiteManagementDataDefault } from '../../services/interfaces/MemberSites';
import { useTranslation } from 'react-i18next';
import i18n from '../../i18n/i18n-memberSite';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import { ResultMessageComponent, ResultType } from '../../components/MessageComponents';
import '../../styles/common.css';
import '../../styles/global.scss';

import TitleComponent from '../../components/TitleComponent';
import { ButtonComponent } from '../../components/ButtonComponents';

const MemberSiteManagement: React.FC = () => {
  const { t } = useTranslation('memberSite');
  useCookieLanguage(i18n);
  const [originalData, setOriginalData] = useState<string>(JSON.stringify(MemberSiteManagementDataDefault));
  const [data, setData] = useState<MemberSiteManagementData>(MemberSiteManagementDataDefault);
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
      setOriginalData(JSON.stringify(response));
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
    const isDirty = JSON.stringify(newData) !== originalData;
    setData(newData);
    setIsDirty(isDirty);
  };

  const handleMessageChange = (messages: MessageData) => {
    const newData = {
      ...data,
      ...messages
    }
    const isDirty = JSON.stringify(newData) !== originalData;
    setData(newData);
    setIsDirty(isDirty);
  };

  const handleSave = async () => {
    try {
      setIsSaving(true);
      const requestData = {
        mobile: `${data.mobilePhone.code}-${data.mobilePhone.phone}`,
       // contacts: data.contacts.map(contact => `${contact.type}:${contact.value}`).join(','),
        contacts: JSON.stringify(data.contacts.map(contact => ({ type: contact.type, value: contact.value}))),
        announcement: data.announcement,
        generalSettings: data.generalSettings,
        
      }
      const response = await memberSitesService.updateMemberSiteData(requestData);


      setIsDirty(false);
      setResult({
        type: response.isSuccessful ? 'success' : 'error',
        text: response.isSuccessful ? t('messages.updateSuccess') : response.message
      });
      setTimeout(() => {
        setOriginalData(JSON.stringify(data));
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



  return (
    <div
      className=""
     
    >
      <div className="p-[12px]">
        <div className='mb-[8px]'>
          <TitleComponent title={t('title')} Tippy={false} />
        </div>
        <div className="bg-[#ffffff] rounded-[4px] max-w-[1200px]">
          {result && (
            <div className='pt-[24px] mx-[24px] max-[768px]:mx-[12px] max-[768px]:pt-[12px]'>
              <ResultMessageComponent
                type={result.type}
                message={result.text}
              />
            </div>
          )}

          <div className="p-[24px] max-[768px]:p-[12px]">
            <div className="">
              <RegistrationLinks
                domains={data.domains.map(domain => ({ ...domain, status: 'active' }))}
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
                contacts={data.contacts}
                mobilePhone={data.mobilePhone}
                announcement={data.announcement || ''}
                onMessageChange={handleMessageChange}
                loading={loading}
              />
            </div>

            <div className="flex justify-start py-[8px] mt-[8px]">
              <ButtonComponent className='w-[140px]' type="apply" style="rounded" disabled={!isDirty || isSaving} text={isSaving ? t('actions.saving') : t('actions.save')} onClick={handleSave}/>
            </div>
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
