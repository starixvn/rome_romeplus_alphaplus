import { PTTemplate } from '@/types/position-taking';

import usePTTemplateStore from './ptTemplateStore';

interface IProps {
  index: number;
  template: PTTemplate;
  onDeleteClick: (templateId: number) => void;
}

export default function ReadOnlyRow({ index, template, onDeleteClick }: IProps) {
  const isSpecialSite = usePTTemplateStore((state) => state.isSpecialSite);
  const setEditingTemplateId = usePTTemplateStore((state) => state.setEditingTemplateId);

  const renderPTValues = () => {
    if (isSpecialSite) {
      const pt = Math.min(...template.settings.map((s) => s.pt));
      return <td className="text-center">{pt / 100}</td>;
    }

    return template.settings.map((setting) => (
      <td key={setting.subGroupId} className="text-center">
        {setting.pt / 100}
      </td>
    ));
  };

  const handleEditClick = () => {
    setEditingTemplateId(template.id);
  };

  return (
    <tr key={template.id}>
      <td className="text-center">{index + 1}</td>
      <td>{template.name}</td>
      {renderPTValues()}
      <td className="text-center">
        <input type="checkbox" checked={template.isDefault} disabled />
      </td>
      <td className="text-center">
        <span className="icon icon-edit" aria-hidden onClick={handleEditClick} />
        <span className="icon icon-bin" aria-hidden onClick={() => onDeleteClick(template.id)} />
      </td>
    </tr>
  );
}
