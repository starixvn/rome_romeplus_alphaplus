import { ChangeEvent, useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';

import { createPTTemplate } from '@/api/position-taking';
import WrapperTooltip from '@/features/Shared/WrapperTooltip';
import { SubMessagesResult } from '@/types';
import { PTTemplateSetting } from '@/types/position-taking';

import PTSelect from '../NewAccount/PositionTaking/PTSelect';
import usePTTemplateStore from './ptTemplateStore';
import styles from './PTTemplateTable.module.scss';

interface IProps {
  onCreateFailed: (result: SubMessagesResult) => void;
  onCreateSuccess: () => void;
  refreshSettings: () => void;
}

export default function AddRow({ onCreateFailed, onCreateSuccess, refreshSettings }: IProps) {
  const setEditingTemplateId = usePTTemplateStore((state) => state.setEditingTemplateId);
  const isSpecialSite = usePTTemplateStore((state) => state.isSpecialSite);
  const is3DecimalSite = usePTTemplateStore((state) => state.is3DecimalSite);
  const initSettings = usePTTemplateStore((state) =>
    state.ptSettings
      .map((setting) =>
        setting.subGroups.map((subGroup) => ({
          groupId: setting.groupId,
          subGroupId: subGroup.subGroupId,
          pt: subGroup.max,
          min: subGroup.min,
          max: subGroup.max,
        })),
      )
      .flat(),
  );
  const { t } = useTranslation();

  const [name, setName] = useState<string>('');
  const [isDefault, setIsDefault] = useState<boolean>(false);
  const [settings, setSettings] = useState<PTTemplateSetting[]>(initSettings);
  const { addTemplate, templates } = usePTTemplateStore();
  const [errorMessage, setErrorMessage] = useState('');

  useEffect(() => {
    if (isSpecialSite) {
      const pt = Math.min(...settings.map((s) => s.pt));
      const newSettings = settings.map((item) => ({ ...item, pt }));
      setSettings(newSettings);
    }
  }, [isSpecialSite]);

  const handleAddTemplate = async () => {
    setEditingTemplateId(0);
    if (!name) {
      setErrorMessage(t('TemplateNameRequireMessage') as string);
      return;
    }

    if (templates.filter((template) => template.name === name).length) {
      setErrorMessage(t('TemplateNameExistedMessage') as string);
      return;
    }

    const template = { id: 0, name, isDefault, settings };
    const { data: result } = await createPTTemplate(template);
    if (result.isSuccessful) {
      setErrorMessage('');
      addTemplate({ ...template, id: result.templateId });
      setName('');
      setIsDefault(false);
      setSettings(initSettings);
      onCreateSuccess();
    } else {
      onCreateFailed(result);
    }

    refreshSettings();
  };

  const handlePTSelectChange = (groupId: number, subGroupId: number, pt: number) => {
    const setting = { groupId, subGroupId, pt };
    const newSettings = settings.map((item) => {
      if (item.groupId === setting.groupId && item.subGroupId === setting.subGroupId) {
        return setting;
      }

      return item;
    });

    setSettings(newSettings);
  };

  const handleY7PTSelectChange = (pt: number) => {
    const newSettings = settings.map((item) => ({ ...item, pt }));
    setSettings(newSettings);
  };

  const handleTemplateNameTextboxChange = (event: ChangeEvent<HTMLInputElement>) => {
    event.preventDefault();
    setName(event.target.value);

    if (templates.filter((template) => template.name === event.target.value).length) {
      setErrorMessage(t('TemplateNameExistedMessage') as string);
    } else {
      setErrorMessage('');
    }
  };

  const hasError = errorMessage !== '';

  const iconStyle = { fontSize: '18px', fontWeight: 'bold', paddingLeft: '5px', cursor: 'default' };

  const renderValidateIcon = () =>
    hasError ? (
      <span className="icon icon-close" style={{ ...iconStyle, color: '#fe0000' }} />
    ) : (
      name !== '' && <span className="icon icon-check" style={{ ...iconStyle, color: '#52a220' }} />
    );

  const renderPTSelections = () => {
    if (!isSpecialSite) {
      return initSettings.map((setting) => (
        <td key={setting.subGroupId} className="text-center">
          <PTSelect
            min={setting.min}
            max={setting.max}
            value={settings.find((s) => s.subGroupId === setting.subGroupId)?.pt ?? setting.max}
            onSelectChange={(e) => handlePTSelectChange(setting.groupId, setting.subGroupId, Number(e.target.value))}
            is3DecimalSite={is3DecimalSite}
          />
        </td>
      ));
    }

    if (initSettings.length) {
      const min = Math.max(...initSettings.map((s) => s.min));
      const max = Math.min(...initSettings.map((s) => s.max));
      const value = Math.min(...settings.map((s) => s.pt));
      return (
        <td className="text-center">
          <PTSelect
            min={min}
            max={max}
            value={value}
            onSelectChange={(e) => handleY7PTSelectChange(Number(e.target.value))}
            is3DecimalSite={is3DecimalSite}
          />
        </td>
      );
    }

    return null;
  };

  return (
    <tr>
      <td />
      <td>
        <div id="template-name" className={styles['template-name']}>
          <input
            type="text"
            className={hasError ? styles['error-input'] : ''}
            style={{ width: '220px' }}
            value={name}
            maxLength={50}
            onChange={handleTemplateNameTextboxChange}
          />
          {renderValidateIcon()}
        </div>
        <WrapperTooltip anchorSelect="#template-name" isOpen={hasError} type="error" content={errorMessage} />
      </td>
      {renderPTSelections()}
      <td className="text-center">
        <input type="checkbox" checked={isDefault} onChange={() => setIsDefault(!isDefault)} />
      </td>
      <td className="text-center">
        <span className="icon icon-save" onClick={handleAddTemplate} role="button" aria-hidden />
      </td>
    </tr>
  );
}
