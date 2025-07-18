import '../../styles/common.css';
import '../../styles/global.scss';

import React, { useEffect, useState } from 'react';
import { createRoot } from 'react-dom/client';
import { FormProvider, useForm } from 'react-hook-form';
import { useTranslation } from 'react-i18next';

import { ButtonComponent } from '../../components/Buttons/index';
import { ResultMessageComponent, ResultType } from '../../components/Message';
import { TitleComponent } from '../../components/Title';
import { useCookieLanguage } from '../../hooks/useCookieLanguage';
import i18n from '../../i18n/i18n-memberSite';
import {
  MemberSiteManagementData,
  MemberSiteManagementDataDefault,
} from '../../services/interfaces/MemberSites';
import { memberSitesService } from '../../services/memberSitesAPI';
import { baseAPI } from '../../services/commonAPI';

import GeneralSettings from './components/GeneralSettings';
import MessageForm from './components/MessageForm';
import RegistrationLinks from './components/RegistrationLinks';
import { SetupChecklistModel } from '../../services/interfaces/SetupChecklistModel';

const MemberSiteManagement: React.FC = () => {
  const { t } = useTranslation(['memberSite', 'global']);
  useCookieLanguage(i18n);
  const [loading, setLoading] = useState(true);
  const [result, setResult] = useState<{ type: ResultType; text: string } | null>(null);
  const [isSaving, setIsSaving] = useState(false);
  const [isCashServiceEnabled, setIsCashServiceEnabled] = useState(false);
  const [setupCheckList, setSetupCheckList] = useState<SetupChecklistModel | null>(null);

  const methods = useForm<MemberSiteManagementData>({
    defaultValues: MemberSiteManagementDataDefault,
    mode: 'onChange',
    criteriaMode: 'all',
  });

  const {
    handleSubmit,
    reset,
    formState: { isDirty, errors },
  } = methods;

  useEffect(() => {
    fetchData();
    const isCashServiceEnabled = document.getElementById('IsCashServiceEnabled')?.getAttribute('value');

    const enabled = isCashServiceEnabled === '1';
    setIsCashServiceEnabled(enabled);

    if(enabled) {
      fetchSetupCheckList();
    }
  }, []);

  const showResult = (type: ResultType, text: string) => {
    setResult({
      type: type,
      text: text,
    });
    setTimeout(() => {
      setResult(null);
    }, 3000);
  };

  const fetchData = async () => {
    try {
      setLoading(true);
      const response = await memberSitesService.getMemberSiteData();
      reset(response);
      setResult(null);
    } catch (err) {
      showResult('error', t('errors.loadFailed'));
    } finally {
      setLoading(false);
    }
  };

  const fetchSetupCheckList = async () => {
    try {
      const response = await baseAPI.getSetupCheckList();
      setSetupCheckList(response);
    } catch (err) {
      showResult('error', t('errors.loadFailed'));
    }
  };

  const onSubmit = async (formData: MemberSiteManagementData) => {
    try {
      if (Object.keys(errors).length > 0) {
        showResult('error', t('validation.pleaseCheckFields'));
        return;
      }
      if (!isDirty) {
        showResult('error', t('global:thereNoChangeToUpdate'));
        return;
      }

      setIsSaving(true);
      const requestData = {
        mobile: `${formData.mobilePhone.code}-${formData.mobilePhone.phone}`,
        contacts: JSON.stringify(
          formData.contacts.map((contact) => ({ type: contact.type, value: contact.value }))
        ),
        announcement: formData.announcement,
        generalSettings: formData.generalSettings,
      };
      const response = await memberSitesService.updateMemberSiteData(requestData);

      showResult(
        response.isSuccessful ? 'success' : 'error',
        response.isSuccessful ? t('messages.updateSuccess') : response.message
      );

      if (response.isSuccessful) {
        reset(formData);
      }
    } catch (err) {
      showResult('error', t('errors.updateFailed'));
    } finally {
      setIsSaving(false);
    }
  };

  return (
    <div className="">
 
        <div className="mb-[8px]">
          <TitleComponent title={t('title')} showTippy={false} />
        </div>
        <div className="max-w-[950px]">               
          {result && <ResultMessageComponent className="mb-[10px]" type={result.type} message={result.text} />}
          <FormProvider {...methods}>
            <form onSubmit={handleSubmit(onSubmit)} className="py-[5px] ">              
                <RegistrationLinks
                  domains={methods
                    .watch('domains')
                    .map((domain) => ({ ...domain, status: 'active' }))}
                  loading={loading}
                  isCashServiceEnabled={isCashServiceEnabled}
                  setupChecklist={setupCheckList}
                />             
              <div className="block w-[100%] h-[1px] bg-[var(--border-color)] my-[10px]" ></div>              
              <GeneralSettings loading={loading} />               
              <div className="block w-[100%] h-[1px] bg-[var(--border-color)] my-[10px]" ></div>             
              <MessageForm loading={loading} />  
              <div className="flex justify-start py-[8px] mt-[8px]">
                <ButtonComponent
                  variant="apply"
                  text={isSaving ? t('actions.saving') : t('actions.save')}
                  disabled={isSaving || !isCashServiceEnabled}
                  title={isCashServiceEnabled ? '' : t('global:cashServiceDisabled')}
                  type="submit"
                />
              </div>
            </form>
          </FormProvider>
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
