import { useState } from 'react';

import { editPTTemplate } from '@/api/position-taking';
import { SubMessagesResult } from '@/types';
import { PTTemplate } from '@/types/position-taking';

import PTSelect from '../NewAccount/PositionTaking/PTSelect';
import styles from './EditingRow.module.scss';
import usePTTemplateStore from './ptTemplateStore';

interface IProps {
  index: number;
  template: PTTemplate;
  onEditFailed: (result: SubMessagesResult) => void;
  onEditSuccess: () => void;
  refreshSettings: () => void;
}

export default function EditingRow({ index, template, onEditSuccess, onEditFailed, refreshSettings }: IProps) {
  const isSpecialSite = usePTTemplateStore((state) => state.isSpecialSite);
  const is3DecimalSite = usePTTemplateStore((state) => state.is3DecimalSite);
  const ptSettings = usePTTemplateStore((state) => state.ptSettings);
  const setEditingTemplateId = usePTTemplateStore((state) => state.setEditingTemplateId);
  const [editingTemplate, setEditingTemplate] = useState(template);
  const { templates, updateTemplates } = usePTTemplateStore();

  const handleCancelClick = () => {
    setEditingTemplateId(0);
  };

  const handleEditTemplate = async () => {
    const { data: result } = await editPTTemplate(editingTemplate);
    if (result.isSuccessful) {
      updateTemplates(
        templates.map((t) => {
          if (t.id === editingTemplate.id) {
            return editingTemplate;
          }

          if (editingTemplate.isDefault) {
            return { ...t, isDefault: false };
          }

          return t;
        }),
      );
      handleCancelClick();
      onEditSuccess();
    } else {
      onEditFailed(result);
    }

    refreshSettings();
  };

  const handlePTSelectChange = (groupId: number, subGroupId: number, pt: number) => {
    const setting = { groupId, subGroupId, pt };
    const newSettings = editingTemplate.settings.map((item) => {
      if (item.groupId === setting.groupId && item.subGroupId === setting.subGroupId) {
        return setting;
      }

      return item;
    });

    setEditingTemplate({ ...editingTemplate, settings: newSettings });
  };

  const handleY7PTSelectChange = (pt: number) => {
    const newSettings = editingTemplate.settings.map((item) => ({ ...item, pt }));

    setEditingTemplate({ ...editingTemplate, settings: newSettings });
  };

  const renderPTSelections = () => {
    if (isSpecialSite) {
      const subGroups = ptSettings.map((setting) => setting.subGroups).flat();
      const min = Math.max(...subGroups.map((group) => group.min));
      const max = Math.min(...subGroups.map((group) => group.max));
      const value = Math.min(...editingTemplate.settings.map((x) => x.pt));

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

    return ptSettings.map((setting) =>
      setting.subGroups.map((subGroup) => (
        <td key={subGroup.subGroupId} className="text-center">
          <PTSelect
            min={subGroup.min}
            max={subGroup.max}
            value={editingTemplate.settings.find((x) => x.subGroupId === subGroup.subGroupId)?.pt ?? 0}
            onSelectChange={(e) => handlePTSelectChange(setting.groupId, subGroup.subGroupId, Number(e.target.value))}
            is3DecimalSite={is3DecimalSite}
          />
        </td>
      )),
    );
  };

  return (
    <tr>
      <td className="text-center">{index + 1}</td>
      <td>{editingTemplate.name}</td>
      {renderPTSelections()}
      <td className="text-center">
        <input
          type="checkbox"
          checked={editingTemplate.isDefault}
          onChange={() => setEditingTemplate({ ...editingTemplate, isDefault: !editingTemplate.isDefault })}
        />
      </td>
      <td className={`text-center ${styles.action}`}>
        <span
          className={`icon icon-check`}
          style={{ fontSize: '18px', fontWeight: 'bold', color: '#52a220' }}
          onClick={handleEditTemplate}
          aria-hidden
        />
        <span
          className="icon icon-close"
          style={{ fontSize: '18px', fontWeight: 'bold', color: '#fe0000' }}
          onClick={handleCancelClick}
          aria-hidden
        />
      </td>
    </tr>
  );
}
