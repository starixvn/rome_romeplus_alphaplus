import i18next, { changeLanguage } from 'i18next';
import { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { deletePTTemplate, getPTSettings, getPTTemplates } from '@/api/position-taking';
import { bootBoxConfirm } from '@/constants';
import { SubMessagesResult } from '@/types';

import ErrorBlock from '../Shared/ErrorBlock';
import Loader from '../Shared/Loader';
import WrapperTooltip from '../Shared/WrapperTooltip';
import AddRow from './AddRow';
import EditingRow from './EditingRow';
import Header from './Header';
import usePTTemplateStore from './ptTemplateStore';
import styles from './PTTemplateTable.module.scss';
import ReadOnlyRow from './ReadOnlyRow';

export default function PTTemplateTable() {
  const { t } = useTranslation();

  const [loading, setLoading] = useState(false);
  const [theme, setTheme] = useState('light');
  const [error, setError] = useState<SubMessagesResult>();
  const updateIsSpecialSite = usePTTemplateStore((state) => state.updateIsSpecialSite);
  const updateIs3DecimalSite = usePTTemplateStore((state) => state.updateIs3DecimalSite);
  const updatePTSettings = usePTTemplateStore((state) => state.updatePTSettings);
  const templates = usePTTemplateStore((state) => state.templates);
  const updateTemplates = usePTTemplateStore((state) => state.updateTemplates);
  const editingTemplateId = usePTTemplateStore((state) => state.editingTemplateId);
  const [actorLevelId] = useState(Number((document.getElementById('ActorLevelId') as HTMLInputElement).value));

  useEffect(() => {
    var timer = setTimeout(() => window.top?.indexManager.initFeedbackFunction(7, undefined, 500), 1000);
    return () => clearTimeout(timer);
  }, []);

  useEffect(() => {
    updateIsSpecialSite((document.getElementById('IsSpecialSite') as HTMLInputElement).value === 'true');
    updateIs3DecimalSite((document.getElementById('Is3DecimalSite') as HTMLInputElement).value === 'true');
  }, [updateIsSpecialSite]);

  useEffect(() => {
    const searchParams = new URLSearchParams(document.location.search);
    if (searchParams.has('isDarkMode')) {
      const isDarkMode = searchParams.get('isDarkMode') === 'true';
      setTheme(isDarkMode ? 'dark' : 'light');
    }

    if (searchParams.has('lng')) {
      const lng = searchParams.get('lng') ?? 'en-US';
      const { languages } = i18next;
      if (!languages.includes(lng)) {
        changeLanguage('en-US');
      }
    }
  }, [setTheme, changeLanguage]);

  useEffect(() => {
    setLoading(true);
    const fetchData = async () => {
      updatePTSettings(await getPTSettings(actorLevelId));
      const { data } = await getPTTemplates();
      updateTemplates(data);
    };

    fetchData().then(() => setLoading(false));
  }, [setLoading, updatePTSettings, updateTemplates, actorLevelId]);

  const handleDeleteTemplate = (templateId: number) => {
    bootBoxConfirm(t('DeletePTTemplateConfirmMessage'), null, async () => {
      const { data: result } = await deletePTTemplate(templateId as number);
      if (result.isSuccessful) {
        updateTemplates(templates.filter((template) => template.id !== templateId));
      } else {
        setError(result as SubMessagesResult);
      }
    });
  };

  const handlerCreateOrEditFailed = async (result: SubMessagesResult) => {
    setError(result);
  };

  const clearError = () => {
    setError(undefined);
  };

  if (loading) {
    return <Loader />;
  }

  return (
    <>
      <div className="page-title">
        {`${t('PTTemplate')} `}
        <span className="icon icon-information-outline" data-tooltip-id="pt-template-note" />
        <WrapperTooltip id="pt-template-note">
          <ul>
            {[`TemplateNote1_Level_${actorLevelId}`, `TemplateNote2_Level_${actorLevelId}`].map((noteKey) => (
              <li key={noteKey}>{t(noteKey)}</li>
            ))}
          </ul>
        </WrapperTooltip>
      </div>
      <div className={styles['pt-template']} data-theme={theme}>
        {error && <ErrorBlock error={error} />}
        <table className="tblRpt tblRpt-bordered tblRpt-striped tblRpt-hover">
          <Header actorLevelId={actorLevelId} />
          <tbody>
            <AddRow
              onCreateFailed={handlerCreateOrEditFailed}
              onCreateSuccess={clearError}
              refreshSettings={async () => updatePTSettings(await getPTSettings(actorLevelId))}
            />
            {templates.map((template, index) =>
              template.id === editingTemplateId ? (
                <EditingRow
                  key={template.id}
                  index={index}
                  template={template}
                  onEditSuccess={clearError}
                  onEditFailed={handlerCreateOrEditFailed}
                  refreshSettings={async () => updatePTSettings(await getPTSettings(actorLevelId))}
                />
              ) : (
                <ReadOnlyRow key={template.id} index={index} template={template} onDeleteClick={handleDeleteTemplate} />
              ),
            )}
          </tbody>
        </table>
      </div>
    </>
  );
}
