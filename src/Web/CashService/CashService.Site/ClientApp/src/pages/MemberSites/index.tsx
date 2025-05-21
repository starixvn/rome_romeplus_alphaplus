  import React, { useEffect, useState } from 'react';
  import { createRoot } from 'react-dom/client';
  import { useForm, FormProvider } from 'react-hook-form';
  import RegistrationLinks from './components/RegistrationLinks';
  import GeneralSettings from './components/GeneralSettings';
  import MessageForm from './components/MessageForm';
  import { memberSitesService } from '../../services/memberSitesAPI';
  import { MemberSiteManagementData, MemberSiteManagementDataDefault } from '../../services/interfaces/MemberSites';
  import { useTranslation } from 'react-i18next';
  import i18n from '../../i18n/i18n-memberSite';
  import { useCookieLanguage } from '../../hooks/useCookieLanguage';
  import { ResultMessageComponent, ResultType } from '../../components/MessageComponents';
  import '../../styles/common.css';
  import '../../styles/global.scss';

  import TitleComponent from '../../components/TitleComponent';
  import { ButtonApply } from '../../components/ButtonComponents';

  const MemberSiteManagement: React.FC = () => {
    const { t } = useTranslation(['memberSite', 'global']);
    useCookieLanguage(i18n);
    const [loading, setLoading] = useState(true);
    const [result, setResult] = useState<{ type: ResultType; text: string } | null>(null);
    const [isSaving, setIsSaving] = useState(false);

    const methods = useForm<MemberSiteManagementData>({
      defaultValues: MemberSiteManagementDataDefault,
      mode: 'onChange',
      criteriaMode: 'all'
    });

    const { handleSubmit, reset, formState: { isDirty, errors } } = methods;

    useEffect(() => {
      fetchData();
    }, []);


    const showResult = (type: ResultType, text: string) => {
      setResult({
        type: type,
        text: text
      });
      setTimeout(() => {
        setResult(null);
      }, 3000);
    }

    const fetchData = async () => {
      try {
        setLoading(true);
        const response = await memberSitesService.getMemberSiteData();
        reset(response);
        setResult(null);
      } catch (err) {
        showResult('error', t('errors.loadFailed'));
        console.error('Error loading data:', err);
      } finally {
        setLoading(false);
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
          contacts: JSON.stringify(formData.contacts.map(contact => ({ type: contact.type, value: contact.value }))),
          announcement: formData.announcement,
          generalSettings: formData.generalSettings,
        }
        const response = await memberSitesService.updateMemberSiteData(requestData);

        showResult(response.isSuccessful ? 'success' : 'error', response.isSuccessful ? t('messages.updateSuccess') : response.message);

      } catch (err) {
        showResult('error', t('errors.updateFailed'));
        console.error('Error updating data:', err);
      } finally {
        setIsSaving(false);
      }
    };

    return (
      <div className="">
        <div className="py-[12px]">
          <div className='mb-[8px]'>
            <TitleComponent title={t('title')} showTippy={false} />
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

            <FormProvider {...methods}>
              <form onSubmit={handleSubmit(onSubmit)} className="p-[24px] max-[768px]:p-[12px]">
                <div className="">
                  <RegistrationLinks
                    domains={methods.watch('domains').map(domain => ({ ...domain, status: 'active' }))}
                    loading={loading}
                  />
                </div>
                <hr className='my-[16px] border-[#0000001F]' />
                <div className="">
                  <GeneralSettings
                    loading={loading}
                  />
                </div>
                <hr className='my-[16px] border-[#0000001F]' />
                <div className="">
                  <MessageForm
                    loading={loading}
                  />
                </div>

                <div className="flex justify-start py-[8px] mt-[8px]">
                  <ButtonApply 
                    className='w-[140px] [&>*]:w-[100%]' 
                    text={isSaving ? t('actions.saving') : t('actions.save')} 
                    disabled={isSaving}
                  />
                </div>
              </form>
            </FormProvider>
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
